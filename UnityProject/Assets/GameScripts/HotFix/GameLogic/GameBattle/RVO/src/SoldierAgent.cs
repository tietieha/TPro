using System;
using M.PathFinding;
using FixPoint;
using M.Battle;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace RVO
{
    enum PathFindingBlockType
    {
        KNone = 0,
        KNoPath = 1,
        KBlocked = 2,
        KBlockedByFriend = 3,
        KBlockedByEnemy = 4,
    }

    enum GroupAIType
    {
        kNone = 0,
        kCavalry = 4,
    }

    public class SoldierAgent : Agent
    {
        //Lua共享内存数组
        private LuaArrAccess _luaArray;

        //目标位置
        private Vector2 _targetPos;

        //是否移动
        private bool _isMoving = false;

        //是否正在跟随
        private bool _isFollowing = false;

        private bool _isFirst = false;
        private bool _revive = false;

        //Team
        private int _teamId;

        private int _groupId;

        //是否近战
        private bool _isMelee;

        private int _aiType = 0;

        //是否虚拟
        private bool _isVirtual;

        private float _targetStopDis = 0f;

        EReachTargetSt _reachTargetSt = EReachTargetSt.None;

        private Simulator _simulator;

        private Vector2 _followPrePos = Vector2.Zero;
        private Vector2 _prePos = Vector2.Zero;
        private List<int> _soldiers = new List<int>();
        private int _virtualTargetSoldierId = 0;

        /// <summary>
        /// 这个是为了绕路处理的
        /// </summary>
        private Vector2 _middlePos = Vector2.Zero;

        public bool IsMoving
        {
            get { return _isMoving; }
            set
            {
                _isMoving = value;
                _luaArray.SetInt((int)UnitShareIndex.IsMoving, _isMoving ? 1 : 0);
            }
        }

        public bool IsRevive
        {
            get { return _revive; }
            set { _revive = value; }
        }

        public EReachTargetSt ReachTargetSt
        {
            get { return _reachTargetSt; }
            set
            {
                _reachTargetSt = value;
                _luaArray.SetInt((int)UnitShareIndex.ReachTargetSt, (int)_reachTargetSt);
            }
        }


        public bool IsMelee
        {
            get { return _isMelee; }
        }

        public int TeamId
        {
            get { return _teamId; }
        }

        public override bool IsVirtual
        {
            get { return _isVirtual; }
        }

        public int AiType
        {
            get { return _aiType; }
            set { _aiType = value; }
        }

        public Vector2 TargetPos
        {
            get { return _targetPos; }
        }

        public int GroupId
        {
            set { _groupId = value; }
            get { return _groupId; }
        }

        public SoldierAgent(Simulator simulator, int id, int teamId, float attackRange,
            int side,
            bool isVirtual, bool isMelee, float stopDis,
            LuaArrAccess luaArray)
        {
            _simulator = simulator;
            Id = id;
            _teamId = teamId;
            _luaArray = luaArray;
            _attackRange = attackRange;
            // _attackRangeSqr = attackRange * attackRange;
            _side = side;
            _isMelee = isMelee;
            _stopDis = stopDis;
            _isVirtual = isVirtual;
        }

        public void AddSoldier(int agentId)
        {
            _soldiers.Add(agentId);
        }

        public List<int> GetSoldiers()
        {
            return _soldiers;
        }

        public int GetSide()
        {
            return _side;
        }

        public void SetTargetPos(float x, float y)
        {
            _targetPos = new Vector2(x, y);
            IsMoving = true;
        }

        public override void SetTargetId(int targetId, bool isFollow = false, bool isFirst = false)
        {
            _luaArray.SetInt((int)UnitShareIndex.NearestTargetID, 0);
            _targetId = targetId;
            _isFollowing = isFollow;
            _isFirst = isFirst;
            _virtualTargetSoldierId = 0;
            _luaArray.SetInt((int)UnitShareIndex.IsPathFindingBlocked, 0);
            if (_isFollowing && _targetId != 0)
            {
                var targetPos = _simulator.GetAgentPosition(_targetId);
                _followPrePos = new Vector2(targetPos.x(), targetPos.y());
            }

            if (targetId > 0)
            {
                _luaArray.SetInt((int)UnitShareIndex.NearestTargetID, targetId);
                _luaArray.SetInt((int)UnitShareIndex.WillAttackTargetId, 0);
                ReachTargetSt = EReachTargetSt.None;
                UpdateTargetPos();
            }
            else
            {
                _targetPos = Position;
                IsMoving = false;
                _targetStopDis = 0f;
            }
        }

        internal override void Update(float timeStep)
        {
            // if (!IsMoving)
            //     return;
            velocity_ = newVelocity_;
            _prePos = Position;
            Position += velocity_ * timeStep;
            SyncLuaPos();
            CheckBlock(velocity_ * timeStep);
            CheckReachTargetSt();
            CheckMeleeEngagedInCombat();
            UpdateTargetPos();
            ClearAgentExtraRange();
            TryAddAgentExtraRange();
        }

        private void CheckReachTargetSt()
        {
            if (!IsMoving || _isFollowing)
            {
                return;
            }

            if (IsVirtual && IsMelee) return;

            // 计算目标方向
            Vector2 toTarget = _targetPos - Position;
            float distToTarget = RVOMath.absSq(toTarget); // 目标距离的平方
            if (toTarget == Vector2.Zero)
            {
                ReachTargetPosition();
                return;
            }

            if (_targetId == 0)
            {
                if (IsClosestTargetPosition(radius_))
                {
                    ReachTargetPosition();
                }

                return;
            }

            float stopDistanceSqr = _isFollowing
                ? 0
                : (_attackRange + _targetStopDis) * (_attackRange + _targetStopDis);
            var targetAgent = _simulator.GetAgent(_targetId);
            // 判断代理是否接近目标
            if (distToTarget < stopDistanceSqr)
            {
                ReachTargetPosition();
            }
            else if (blockageCounter >= blockageThreshold)
            {
                var attackRange = AttackRange;
                foreach (var neighbor in agentNeighbors_)
                {
                    SoldierAgent agent = (SoldierAgent)neighbor.Value;
                    if (agent.IsVirtual) continue;
                    if (agent.Side == _side) continue;
                    float distance =
                        RVOMath.abs(Position - neighbor.Value.Position);
                    if (distance <= attackRange + neighbor.Value.StopDis)
                    {
                        ReachTargetPosition(neighbor.Value.Id);
                        return;
                    }
                }
            }

            var newTargetId = NeedChangeNearestAgent(_targetId);
            if (newTargetId > 0)
            {
                SetTargetId(newTargetId);
            }
        }

        /// <summary>
        /// 近战军团接战，一个小兵接战了就整体都接战，不用军团的位置判断了
        /// </summary>
        private void CheckMeleeEngagedInCombat()
        {
            if (_isVirtual || !_isMelee || !_isFollowing) return;
            var followTarget = _simulator.GetAgent(_targetId);
            if (followTarget.ReachTargetSt == EReachTargetSt.Normal) return;
            var hateTargetId = followTarget.TargetId;
            var hateTargetAgent = _simulator.GetAgent(hateTargetId);
            if (hateTargetAgent != null)
            {
                float groupDistance =
                    RVOMath.abs(followTarget.Position - followTarget._targetPos);
                if (groupDistance <= followTarget.radius_ + hateTargetAgent.radius_ + 1)
                {
                    followTarget.ReachTargetPosition();
                    return;
                }
            }

            var attackRange = AttackRange;
            foreach (var neighbor in agentNeighbors_)
            {
                SoldierAgent agent = (SoldierAgent)neighbor.Value;
                if (agent.IsVirtual) continue;
                if (agent.Side == Side) continue;
                float distance =
                    RVOMath.abs(Position - neighbor.Value.Position);
                if (distance <= attackRange + neighbor.Value.StopDis)
                {
                    followTarget.ReachTargetPosition(agent.GroupId);
                    break;
                }
            }
        }

        private void UpdateTargetPos()
        {
            if (_targetId > 0)
            {
                if (!_simulator.IsAgentNo(_targetId))
                {
                    SetTargetId(0);
                    return;
                }

                SoldierAgent targetAgent = _simulator.GetAgent(_targetId);
                var targetPos = _simulator.GetAgentPosition(_targetId);
                var stopDis = _simulator.GetAgentStopDis(_targetId);
                if (_isFollowing)
                {
                    var deltaPos = targetPos - _followPrePos;
                    SetTargetPos(Position.x() + deltaPos.x(), Position.y() + deltaPos.y());
                }
                else if (targetAgent.IsVirtual)
                {
                    if (_isVirtual && _isFirst && 4 == _aiType)
                    {
                        bool reachedMiddleScene = _side == 1 ? Position.x() > 0 : Position.x() < 0;
                        bool inSameSide = Position.x() * targetPos.x() > 0;
                        if (!reachedMiddleScene && !inSameSide)
                        {
                            var deltaPos = targetPos - _followPrePos;
                            SetTargetPos(Position.x() + deltaPos.x(), Position.y());
                            return;
                        }
                    }

                    if (_virtualTargetSoldierId == 0 || _simulator.GetAgent(_virtualTargetSoldierId) == null)
                    {
                        var agent = GetNearestSoldierAgentAroundVirtual(targetAgent);
                        _virtualTargetSoldierId = agent.Id;
                        targetPos = agent.Position;
                    }
                    else
                    {
                        targetPos = _simulator.GetAgentPosition(_virtualTargetSoldierId);
                    }

                    // var eightPoints = BattleUtils.GetCircleEightDirectionPoints(stopDis, _targetPos);
                    // var point = BattleUtils.FindNearestTargetPoint(_targetPos, targetPos, eightPoints);
                    _targetStopDis = stopDis;
                    SetTargetPos(targetPos.x(), targetPos.y());
                }
                else
                {
                    // var eightPoints = BattleUtils.GetCircleEightDirectionPoints(stopDis, targetPos);
                    // var point = BattleUtils.FindNearestTargetPoint(_targetPos, targetPos, eightPoints);
                    _targetStopDis = stopDis;
                    SetTargetPos(targetPos.x(), targetPos.y());
                }
            }
        }

        public void SyncLuaPos()
        {
            var x = (int)(Position.x() * FixInt2.Scale);
            var y = (int)(Position.y() * FixInt2.Scale);
            _luaArray.SetInt((int)UnitShareIndex.PosX, x);
            _luaArray.SetInt((int)UnitShareIndex.PosY, y);
        }

        // 新增的计数器变量
        private int blockageCounter = 0;
        private const int blockageThreshold = 3; // 达到这个次数后判断为堵塞
        private float CheckAndRerouTethresholdDistance = 2.0f; // 阻挡检测阈值

        internal override void ComputeNewVelocity(float timeStep)
        {
            prefVelocity_ = Vector2.Zero;
            if (!_isMoving)
            {
                base.ComputeNewVelocity(timeStep);
                return;
            }

            if (_isFollowing)
            {
                if (_targetId != 0)
                {
                    var followTarget = _simulator.GetAgent(_targetId);
                    if (followTarget == null)
                    {
                        base.ComputeNewVelocity(timeStep);
                        return;
                    }

                    prefVelocity_ = _simulator.GetAgentPrefVelocity(_targetId);
                    var hateTargetId = followTarget.TargetId;
                    var hateTarget = _simulator.GetAgent(hateTargetId);
                    if (hateTarget != null)
                    {
                        var followTargetPos = followTarget.Position;
                        var followTargetAttackRange = followTarget.AttackRange;

                        var stopDis = _simulator.GetAgentStopDis(hateTargetId);
                        var dis = RVOMath.absSq(Position - hateTarget.Position);
                        if (stopDis * stopDis >= dis)
                        {
                            prefVelocity_ = Vector2.Zero;
                        }
                        else if (!_isFirst && RVOMath.absSq(Position - followTargetPos) >
                                 followTargetAttackRange * followTargetAttackRange)
                        {
                            prefVelocity_ = RVOMath.safenormalize(followTargetPos - Position) * maxSpeed_;
                        }
                        else
                        {
                            if (!_isFirst)
                            {
                                prefVelocity_ = RVOMath.safenormalize(hateTarget.Position - Position) * maxSpeed_;
                            }
                        }
                    }
                }

                base.ComputeNewVelocity(timeStep);
                return;
            }

            var toTarget = _targetPos - Position;
            if (toTarget == Vector2.Zero)
            {
                base.ComputeNewVelocity(timeStep);
                return;
            }

            // 这步是为了防止virtualhero 跟目标重叠的
            if (_isVirtual && _isMelee && RVOMath.abs(toTarget) < 2 * radius_)
            {
                base.ComputeNewVelocity(timeStep);
                return;
            }

            var routeVelocity = CalculateRouteVelocity();
            // // // 计算朝目标方向的速度
            Vector2 finalDirection = RVOMath.safenormalize(toTarget);
            // //
            if (routeVelocity != Vector2.Zero)
            {
                routeVelocity = RVOMath.safenormalize(routeVelocity);
                if (RVOMath.Dot(finalDirection, routeVelocity) < -0.9f)
                {
                    Vector2 tangent = new Vector2(-finalDirection.y(), finalDirection.x()); // 逆时针垂直方向
                    finalDirection = RVOMath.safenormalize(finalDirection + tangent);
                }
                else
                {
                    finalDirection = RVOMath.safenormalize(finalDirection + routeVelocity * 0.5f); // 80%朝目标前进，20%避开障碍
                }

                if (blockageCounter >= blockageThreshold)
                {
                    blockageCounter = 0;
                    prefVelocity_ = finalDirection * maxSpeed_;
                    newVelocity_ = prefVelocity_;
                    return;
                }
            }

            prefVelocity_ = finalDirection * maxSpeed_;

            base.ComputeNewVelocity(timeStep);
        }

        /// <summary>
        /// 判断是否被阻挡  被阻挡要求是障碍物或者并没有在寻路的兵，如果是在寻路的有可能他会跑前去，并不是阻碍
        /// </summary>
        /// <param name="toTarget"></param>  自身到目标点向量
        /// <returns></returns>
        private SoldierAgent IsAgentBlocked()
        {
            float radius = this.StopDis;
            foreach (var neighbor in this.agentNeighbors_)
            {
                SoldierAgent agent = (SoldierAgent)neighbor.Value;
                if (agent.IsMoving) continue;
                float distance =
                    RVOMath.abs(this.Position - neighbor.Value.Position);
                float neightborRaidus = neighbor.Value.StopDis;
                if (distance <= radius + radius_ + neightborRaidus &&
                    IsLineSegmentIntersectingCircle(Position, _targetPos, agent.Position, neighbor.Value.radius_))
                {
                    return agent;
                }
            }

            return null; // 没有被阻挡
        }

        private Vector2 CalculateRouteVelocity()
        {
            float radius = StopDis;
            var newDirection = Vector2.Zero;
            foreach (var neighbor in agentNeighbors_)
            {
                SoldierAgent agent = (SoldierAgent)neighbor.Value;
                if (agent.IsMoving) ;
                float distance =
                    RVOMath.abs(Position - neighbor.Value.Position);
                float neightborRaidus = neighbor.Value.StopDis;
                float dis = radius + radius_ + neightborRaidus;
                if (distance <= radius + radius_ + neightborRaidus)
                {
                    Vector2 avoidDir = RVOMath.safenormalize(Position - agent.Position) * (dis - distance);
                    newDirection += avoidDir;
                }
            }

            return newDirection;
        }

        public bool IsLineSegmentIntersectingCircle(Vector2 p1, Vector2 p2, Vector2 center, float radius)
        {
            Vector2 d = p2 - p1; // 线段方向
            Vector2 f = p1 - center; // 从圆心指向线段起点

            float a = RVOMath.Dot(d, d);
            float b = 2 * RVOMath.Dot(f, d);
            float c = RVOMath.Dot(f, f) - radius * radius;

            float discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                // 没有交点
                return false;
            }

            discriminant = RVOMath.sqrt(discriminant);

            float t1 = (-b - discriminant) / (2 * a);
            float t2 = (-b + discriminant) / (2 * a);

            // 判断是否在 0~1 范围内（线段范围）
            if ((t1 >= 0 && t1 <= 1) || (t2 >= 0 && t2 <= 1))
            {
                return true; // 有交点在线段上
            }

            return false; // 有交点，但不在段上
        }


        /// <summary>
        /// 到达目标点之后的逻辑处理
        /// </summary>
        private void ReachTargetPosition(int otherTarget = 0)
        {
            if (otherTarget > 0 || _targetId > 0)
            {
                _luaArray.SetInt((int)UnitShareIndex.WillAttackTargetId, otherTarget > 0 ? otherTarget : _targetId);
            }

            SetTargetId(0); // 清空目标
            ReachTargetSt = EReachTargetSt.Normal;
        }

        /// <summary>
        /// 向军团移动的过程中也许 小兵离军团很远 这个时候选一个小兵的位置
        /// </summary>
        /// <returns></returns>
        private SoldierAgent GetNearestSoldierAgentAroundVirtual(SoldierAgent targetAgent)
        {
            SoldierAgent nearestAgent = null;
            bool hasNearestAgent = false;
            // foreach (var neighbor in targetAgent.agentNeighbors_)
            // {
            //     SoldierAgent agent = (SoldierAgent)neighbor.Value;
            //     if (agent.IsVirtual) continue;
            //     if (agent.GroupId != targetAgent.Id) continue;
            //     float distance =
            //         RVOMath.abs(targetAgent.Position - neighbor.Value.Position);
            //     float neightborRaidus = neighbor.Value.StopDis;
            //     float dis = neighbor.Value.radius_ * 2 + targetAgent.radius_ + neightborRaidus;
            //     if (distance <= dis)
            //     {
            //         hasNearestAgent = true;
            //         break;
            //     }
            //
            //     nearestAgent = nearestAgent ?? (SoldierAgent)neighbor.Value;
            // }

            float minDist = float.MaxValue;
            int soldierId = 0;
            // if (!hasNearestAgent && nearestAgent == null)
            // {
            List<int> allSoldiers = targetAgent.GetSoldiers();
            foreach (var agentId in allSoldiers)
            {
                if (_simulator.GetAgent(agentId) != null)
                {
                    float distance =
                        RVOMath.absSq(Position - _simulator.GetAgentPosition(agentId));
                    if (distance < minDist)
                    {
                        soldierId = agentId;
                        minDist = distance;
                    }
                }
            }

            if (soldierId != 0)
            {
                nearestAgent = _simulator.GetAgent(soldierId);
            }
            // }

            return nearestAgent ?? targetAgent;
        }

        /// <summary>
        /// 当前以位置为动过，设置为被阻挡
        /// 如果X/Y的变化值均小于0.001，那么视为阻挡
        /// </summary>
        /// <param name="velocity"></param>
        private void CheckBlock(Vector2 velocity)
        {
            if (!IsMoving) return;
            var deltaPos = Position - _prePos;
            if (deltaPos == Vector2.Zero || (deltaPos.x() < 0.001 && deltaPos.y() < 0.001))
            {
                blockageCounter++;
                return;
            }

            blockageCounter = 0;
        }

        /// <summary>
        /// 被挡住了 尝试增加攻击范围
        /// </summary>
        private void TryAddAgentExtraRange()
        {
            if (!IsMoving) return;
            if (_isFollowing) return;
            if (blockageCounter < blockageThreshold) return;
            if (_isVirtual || !_isMelee) return;
            var direction = _targetPos - Position;
            var dis = RVOMath.absSq(direction);
            List<int> friendIds = new List<int>();
            List<int> enemyIds = new List<int>();
            foreach (var neighbor in agentNeighbors_)
            {
                var agent = (SoldierAgent)neighbor.Value;
                if (agent.IsVirtual) continue;
                var t = RVOMath.Dot(agent.Position - Position, direction) / dis;
                if (t >= 0 && t <= 1)
                {
                    if (agent.Side == Side)
                    {
                        friendIds.Add(agent.Id);
                    }
                    else
                    {
                        enemyIds.Add(agent.Id);
                    }
                }
            }

            if (friendIds.Count > 0 && enemyIds.Count == 0)
            {
                _luaArray.SetInt((int)UnitShareIndex.TryScaleAtkRange, 1);
            }
            else if (friendIds.Count == 0 && enemyIds.Count > 0)
            {
                foreach (var enemyId in enemyIds)
                {
                    var enemyAgent = (SoldierAgent)_simulator.GetAgent(enemyId);
                    if (enemyAgent != null)
                    {
                        float distance =
                            RVOMath.abs(Position - enemyAgent.Position);

                        if (distance < enemyAgent.StopDis + _attackRange)
                        {
                            ReachTargetPosition(enemyId);
                            break;
                        }
                    }
                }
            }
            else
            {
                _luaArray.SetInt((int)UnitShareIndex.TryScaleAtkRange, 3);
            }
        }

        /// <summary>
        /// 清空攻击范围
        /// </summary>
        private void ClearAgentExtraRange()
        {
            _luaArray.SetInt((int)UnitShareIndex.TryScaleAtkRange, 0);
        }

        /// <summary>
        /// 判断是不是被包围了 没有路可以走了
        /// </summary>
        public int IsEncircled(float attackRange)
        {
            float x = Position.x();
            float y = Position.y();
            float range = attackRange + _stopDis;
            List<Vector2> directions = new List<Vector2>()
            {
                new Vector2(x, y + range),
                new Vector2(x, y - range),
                new Vector2(x + range, y),
                new Vector2(x - range, y),
                new Vector2(x - range, y - range),
                new Vector2(x - range, y + range),
                new Vector2(x + range, y - range),
                new Vector2(x + range, y + range),
            };
            Dictionary<int, List<Agent>> surroundAgents = new Dictionary<int, List<Agent>>();
            foreach (var neighbor in agentNeighbors_)
            {
                var agent = (SoldierAgent)neighbor.Value;
                if (agent.IsVirtual) continue;
                var agentDir = agent.Position - Position;
                for (int i = 0; i < directions.Count; i++)
                {
                    var t = RVOMath.Dot(agentDir, directions[i]) / range;
                    if (t >= 0 && t <= 1)
                    {
                        if (!surroundAgents.ContainsKey(i))
                        {
                            surroundAgents.Add(i, new List<Agent>());
                        }

                        surroundAgents[i].Add(agent);
                    }
                }
            }

            for (int i = 0; i < directions.Count; i++)
            {
                if (!surroundAgents.ContainsKey(i))
                {
                    return 0;
                }
            }

            return 1;
        }

        /// <summary>
        /// 如果我与目标之间有阻挡，且附近有敌人 换一个目标打
        /// </summary>
        /// <returns></returns>
        public int NeedChangeNearestAgent(int targetId)
        {
            var targetAgent = (SoldierAgent)_simulator.GetAgent(targetId);
            if (targetAgent == null) return 0;
            var direction = targetAgent.Position - Position;
            var dis = RVOMath.absSq(direction);
            List<int> friendIds = new List<int>();
            List<int> enemyIds = new List<int>();
            foreach (var neighbor in agentNeighbors_)
            {
                var agent = (SoldierAgent)neighbor.Value;
                if (agent.IsVirtual) continue;
                var t = RVOMath.Dot(agent.Position - Position, direction) / dis;
                if (t >= 0 && t <= 1)
                {
                    if (agent.Side == Side)
                    {
                        friendIds.Add(agent.Id);
                    }
                    else
                    {
                        enemyIds.Add(agent.Id);
                    }
                }
            }

            if (friendIds.Count > 0 && enemyIds.Count == 0)
            {
                return 0;
            }

            var minDist = float.MaxValue;
            int enemyId = 0;
            foreach (var neighbor in agentNeighbors_)
            {
                var agent = (SoldierAgent)neighbor.Value;
                if (agent.IsVirtual) continue;
                //|| agent.GroupId != targetAgent.GroupId
                if (agent.Side == Side) continue;
                var distance = RVOMath.abs(agent.Position - Position);
                if (distance < minDist)
                {
                    enemyId = agent.Id;
                    minDist = distance;
                }
            }

            if (enemyId > 0 && minDist <= 3 * _attackRange)
            {
                _luaArray.SetInt((int)UnitShareIndex.TryScaleAtkRange, Mathf.CeilToInt(minDist / _attackRange));
            }

            return enemyId;
        }

        /// <summary>
        /// 获取被阻挡的类型
        /// </summary>
        /// <returns></returns>
        private PathFindingBlockType GetBlockedType()
        {
            bool isBlockedByFriend = false;
            bool isBlockedByEnemy = false;
            foreach (var neighbor in agentNeighbors_)
            {
                isBlockedByFriend = isBlockedByFriend || neighbor.Value.Side == Side;
                isBlockedByEnemy = isBlockedByEnemy || neighbor.Value.Side != Side;
                if (isBlockedByEnemy && isBlockedByFriend) break;
            }

            return isBlockedByFriend && isBlockedByEnemy
                ? PathFindingBlockType.KBlocked
                : (isBlockedByEnemy ? PathFindingBlockType.KBlockedByEnemy : PathFindingBlockType.KBlockedByFriend);
        }

        /// <summary>
        /// 设置绕的中间位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetMiddlePosition(float x, float y)
        {
            _middlePos = new Vector2(x, y);
        }

        /// <summary>
        /// 是否接近目标点了
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        private bool IsClosestTargetPosition(float distance)
        {
            // 计算目标方向
            Vector2 toTarget = _targetPos - Position;
            if (toTarget == Vector2.Zero)
            {
                return true;
            }

            float distToTarget = RVOMath.absSq(toTarget); // 目标距离的平方
            return distToTarget <= distance;
        }

        // 绕行逻辑
        public void CheckAndRerouteIfBlocked(SoldierAgent targetAgent,
            Vector2 targetPosition, float thresholdDistance)
        {
            int blockingAgentsCount =
                CountBlockingAgents(targetAgent, targetPosition, thresholdDistance);
            if (blockingAgentsCount > 0)
            {
                // 路径被阻塞，执行绕行
                RerouteAgent(targetAgent, targetPosition);
                blockageCounter = 0; // 重置计数器
            }
            else
            {
                // 没有阻塞，正常前进
                Vector2 directionToTarget = targetPosition - targetAgent.Position;
                targetAgent.prefVelocity_ = RVOMath.normalize(directionToTarget) *
                                            targetAgent.maxSpeed_;
            }
        }

        // 计算连线中有多少个Agent阻挡路径
        public int CountBlockingAgents(SoldierAgent targetAgent, Vector2 targetPosition,
            float thresholdDistance)
        {
            int blockingAgentsCount = 0;
            Vector2 lineStart = targetAgent.Position;
            Vector2 lineEnd = targetPosition;

            foreach (var neighbor in targetAgent.agentNeighbors_)
            {
                Vector2 neighborPosition = neighbor.Value.Position;
                float distanceToLine =
                    RVOMath.distSqPointLineSegment(lineStart, lineEnd, neighborPosition);
                if (distanceToLine < thresholdDistance)
                {
                    blockingAgentsCount++;
                }
            }

            return blockingAgentsCount;
        }

        // 绕行目标Agent
        public void RerouteAgent(SoldierAgent targetAgent, Vector2 targetPosition)
        {
            // 计算绕行的方向：需要避开当前邻近的Agent并朝着目标前进
            Vector2 newDirection = Vector2.Zero;

            // 遍历所有邻居并避开它们
            foreach (var neighbor in targetAgent.agentNeighbors_)
            {
                // 计算目标Agent与邻居的方向，并加上推力
                if (targetAgent.Position == neighbor.Value.Position)
                    continue;
                Vector2 directionToNeighbor = targetAgent.Position - neighbor.Value.Position;
                newDirection += RVOMath.normalize(directionToNeighbor);
            }

            // 计算目标位置的方向
            Vector2 toTarget = targetPosition - targetAgent.Position;
            if (newDirection == Vector2.Zero)
            {
                // 如果没有邻居，直接朝目标前进
                newDirection = RVOMath.normalize(toTarget);
            }

            // 如果绕行的方向和目标的方向差距太大，调整新的方向，确保目标位置不会偏离太远
            // 保证绕行后能够回到目标位置附近
            newDirection = RVOMath.normalize(newDirection);
            Vector2 finalDirection = RVOMath.normalize(toTarget) * 0.7f + newDirection * 0.3f; // 80%朝目标前进，20%避开障碍
            // Vector2 finalDirection = RVOMath.normalize(toTarget) * 0.9f + newDirection * 0.1f; // 90%朝目标前进，10%避开障碍

            // 规范化并设置最终的速度
            targetAgent.prefVelocity_ = RVOMath.normalize(finalDirection) * targetAgent.maxSpeed_;
        }

        public Vector2 GetBestBypassMidpoint(
            Vector2 B, Vector2 C,
            float rB, float buffer = 0.05f)
        {
            List<(Vector2 point, float score)> candidates = new();
            List<SoldierAgent> obstacles = new();
            foreach (var neighbor in agentNeighbors_)
            {
                SoldierAgent agent = (SoldierAgent)neighbor.Value;
                if (agent.IsMoving) continue;
                float distance =
                    RVOMath.abs(Position - neighbor.Value.Position);
                float neightborRaidus = neighbor.Value.StopDis;
                if (distance <= _stopDis + radius_ + neightborRaidus)
                {
                    obstacles.Add(agent);
                }
            }

            foreach (var obs in obstacles)
            {
                float dist = DistanceToSegment(B, C, obs.Position);
                float avoidDist = rB + obs.radius_ + buffer;

                if (dist < avoidDist)
                {
                    // 有阻挡，生成两个候选中转点（偏移绕行点）
                    Vector2 BA = obs.Position - B;
                    Vector2 perp = RVOMath.normalize(RVOMath.Perpendicular(BA));

                    float offset = rB + obs.radius_ + buffer;

                    Vector2 mid1 = obs.Position + perp * offset;
                    Vector2 mid2 = obs.Position - perp * offset;

                    // 对每个候选点打分（越接近目标方向越好，离障碍越远越好）
                    candidates.Add((mid1, Score(mid1, B, C, obstacles)));
                    candidates.Add((mid2, Score(mid2, B, C, obstacles)));
                }
            }

            // 没有阻挡，不需要绕行
            if (candidates.Count == 0) return Vector2.Zero;

            // 选择得分最高的那个点
            candidates.Sort((a, b) => b.score.CompareTo(a.score));
            return candidates[0].point;
        }

        private float Score(Vector2 point, Vector2 B, Vector2 C, List<SoldierAgent> obstacles)
        {
            float toTargetScore = RVOMath.Dot(RVOMath.normalize(C - point), RVOMath.normalize(C - B)); // 方向相似度

            float minDist = float.MaxValue;
            foreach (var obs in obstacles)
            {
                float d = point.Distance(obs.Position) - obs.radius_;
                if (d < minDist)
                    minDist = d;
            }

            // 越靠近目标方向 & 越远离障碍，分数越高
            return toTargetScore + 0.2f * minDist;
        }

        private float DistanceToSegment(Vector2 A, Vector2 B, Vector2 P)
        {
            Vector2 AB = B - A;
            float t = RVOMath.Dot(P - A, AB) / RVOMath.absSq(AB);
            t = Mathf.Clamp01(t);
            Vector2 closest = A + t * AB;
            return P.Distance(closest);
        }
    }
}