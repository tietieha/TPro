using System;
using System.Collections.Generic;
using FixPoint;
using XLua;

namespace M.PathFinding
{

    using PositionConstrainFunc = Func<FixInt2, bool>;

    //每个单位同一时刻只能占据一个格子
    [LuaCallCSharp]
    public enum EUnitState
    {
        Stop = 0, //停在原地（占据格子）
        SearchEnemy, //以10个敌方单位为目标寻敌，挡路的时候会稍微停一下，然后继续寻敌（占据格子）
        // SlipTo, //滑向某个位置，碰到其它障碍物会停下来（占据格子）
        // ForceSlip, //根据力阻尼滑动，碰到其它障碍物会停下来（占据格子）
        Attack,
        StopVoid, //原地消失状态（不占据）
        MoveStraightTo, //直线移动到某处（不占据）
        MoveTo, // 直线移动到某处/目标前，遇到工事停下（不占据）

        JumpTo, //预约某个位置，跳跃到那里（起跳点不占据、落地点占据，直线跳跃过去，跳跃中不占据）

        FollowHero, //跟随英雄

        VirtualHeroMovePathTo,
        SoldierFollowVirtualHero,
        SoldierSearchEnemy,

        MoveWithTurnAngle,
        HeroMovePathTo,
        HeroSearchEnemy,
        HorseFollowVirtualHero,
        HeroMoveStraightTo,

        DashMove, //冲锋
        // Coerced, //裹挟

        // AttackFortification, //攻击工事
        // SearchFortificationEnemy, //寻找工事目标
        // Guard, // 防守

        // AppointTarget,//指定攻击目标
        // CharmSearch,//寻找己方目标

        // StopAndOccupy, // 停住并生成工事节点占据

        // Vigilance, //巡逻
        // GuardWallAttack, //守护城墙反击
        /// <summary>
        /// 虚拟英雄指定攻击目标
        /// </summary>
        VirtualHeroAssignTarget,
        /// <summary>
        /// 等待虚拟英雄指定攻击目标
        /// </summary>
        WaitVirtualHeroAssignTarget,
        /// <summary>
        /// 寻路移动到目标
        /// </summary>
        MoveToTarget,

        AllState, // 状态总数
    }

    public enum EReachTargetSt
    {
        None = -1, // 未到达
        Normal = 0, // 正常到达
        BeHit = 1, // 被打结束
        BeIntercept = 2, // 被工事拦截
        TargetDisappear = 3, //  目标消失（死亡等）
        BeStop = 4, // 被其他单位主档
        BeExternalStop = 5, // 被外部打断
    }

    public partial class Unit
    {
        //单位当前状态
        private EUnitState _unitState = EUnitState.StopVoid;

        //建立各状态基于地地图是否可以行走,大于0表示可行走
        public static readonly int[,] StateLayoutArray = new int[(int)EUnitState.AllState, (int)ELayerType.Count]
        {
            { -1, -1, -1 },//Stop = 0, //停在原地（占据格子）
            { -1, -1, -1 },//SearchEnemy, //以10个敌方单位为目标寻敌，挡路的时候会稍微停一下，然后继续寻敌（占据格子）
            // { -1, -1, -1 },//SlipTo, //滑向某个位置，碰到其它障碍物会停下来（占据格子）
            // { -1, -1, -1 },//ForceSlip, //根据力阻尼滑动，碰到其它障碍物会停下来（占据格子）
            { -1, -1, -1 },//Attack,
            { 1, 1, 1 },//StopVoid, //原地消失状态（不占据）
            { -1, -1, -1 }, //MoveStraightTo, //直线移动到某处（不占据）
            { 1, -1, -1 }, //MoveTo, // 直线移动到某处/目标前，遇到工事停下（不占据）

            { -1, -1, -1 },//JumpTo, //预约某个位置，跳跃到那里（起跳点不占据、落地点占据，直线跳跃过去，跳跃中不占据）

            { -1, -1, -1 },//FollowHero, //跟随英雄

            { -1, -1, -1 },//VirtualHeroMovePathTo,
            { -1, -1, -1 },//SoldierFollowVirtualHero,
            { -1, -1, -1 },//SoldierSearchEnemy,

            { -1, -1, -1 },//MoveWithTurnAngle,
            { -1, -1, -1 },//HeroMovePathTo,
            { -1, -1, -1 },//HeroSearchEnemy,
            { -1, -1, -1 },//HorseFollowVirtualHero,
            { -1, -1, -1 },//HeroMoveStraightTo,

            { -1, -1, -1 },//DashMove, //冲锋
            // { -1, -1, -1 },//Coerced, //裹挟

            // { -1, -1, -1 },//AttackFortification, //攻击工事
            // { -1, -1, -1 },//SearchFortificationEnemy, //寻找工事目标
            // { -1, -1, -1 },//Guard, // 防守

            // { -1, -1, -1 },//AppointTarget,//指定攻击目标
            // { -1, -1, -1 },//CharmSearch,//寻找己方目标

            // { -1, -1, -1 },//StopAndOccupy, // 停住并生成工事节点占据
            // { -1, -1, -1}, //Vigilance, //巡逻
            // { -1, -1, -1}, //Vigilance, //GuardWallAttack, //守护城墙反击
            { -1, -1, -1}, //VirtualHeroAssignTarget, //虚拟英雄指定攻击目标
            { -1, -1, -1}, //StateWaitVirtualHeroAssignTarget, //等待虚拟英雄指定攻击目标
            { -1, -1, -1}, //MoveToTarget, //寻路移动到目标
        };

        public EUnitState UnitState
        {
            get { return _unitState; }
            set
            {
                _unitState = value;
                LuaArray.SetInt((int)UnitShareIndex.State, (int)_unitState);
            }
        }

        private State _curState = null;

        private StateStop _stateStop = null;
        private StateSearchEnemy _stateSearchEnemy = null;
        // private StateSlipTo _stateSlipTo = null;
        // private StateForceSlip _stateForceSlip = null;
        //private StateMovePathTo _stateMovePathTo = null;
        private StateStopVoid _stateStopVoid = null;
        private StateMoveStraightTo _stateMoveStraightTo = null;
        private StateJumpTo _stateJumpTo = null;
        private StateFollowHero _stateFollowHero = null;
        private StateAttack _stateAttack = null;

        //2020新增状态
        private StateVirtualHeroMovePathTo _stateVirtualHeroMovePathTo = null;
        private StateSoldierFollowVirtualHero _stateSoldierFollowVirtualHero = null;
        private StateSoldierSearchEnemy _stateSoldierSearchEnemy = null;

        private StateMoveWithTurnAngle _stateMoveWithTurnAngle = null;
        private StateHeroMovePathTo _stateHeroMovePathTo = null;
        private StateHeroSearchEnemy _stateHeroSearchEnemy = null;
        private StateHorseFollowVirtualHero _stateHorseFollowVirtualHero = null;
        private StateHeroMoveStraightTo _stateHeroMoveStraightTo = null;
        private StateDashMove _stateDashMove = null;
        // private StateCoerced _stateCoerced = null;
        // private StateAttackFortification _stateAttackFortifiction = null;
        // private StateHeroOrVirtualGuard _stateHeroOrVirtualGuard = null;
        private StateMoveTo _stateMoveTo = null;
        // private StateAppointTarget _stateAppointTarget = null;
        // private StateCharmSearch _stateCharmSearch = null;
        // private StateHeroOrVirtualVigilance _stateHeroOrVirtualVigilance = null;
        // private StateGuardWallAttck _stateGuardWallAttck = null;

        private StateVirtualHeroAssignTarget _stateVirtualHeroAssignTarget = null;
        private StateWaitVirtualHeroAssignTarget _stateWaitVirtualHeroAssignTarget = null;
        private StateMoveToTarget _stateMoveToTarget = null;
        private void InitState()
        {
            _stateStop = new StateStop(this);
            _stateSearchEnemy = new StateSearchEnemy(this);
            // _stateSlipTo = new StateSlipTo(this);
            // _stateForceSlip = new StateForceSlip(this);
            //_stateMovePathTo = new StateMovePathTo(this);
            _stateStopVoid = new StateStopVoid(this);
            _stateMoveStraightTo = new StateMoveStraightTo(this);
            _stateJumpTo = new StateJumpTo(this);
            _stateFollowHero = new StateFollowHero(this);
            _stateAttack = new StateAttack(this);

            _stateVirtualHeroMovePathTo = new StateVirtualHeroMovePathTo(this);
            _stateSoldierFollowVirtualHero = new StateSoldierFollowVirtualHero(this);
            _stateSoldierSearchEnemy = new StateSoldierSearchEnemy(this);

            _stateMoveWithTurnAngle = new StateMoveWithTurnAngle(this);
            _stateHeroMovePathTo = new StateHeroMovePathTo(this);
            _stateHeroMoveStraightTo = new StateHeroMoveStraightTo(this);
            _stateHeroSearchEnemy = new StateHeroSearchEnemy(this);
            _stateHorseFollowVirtualHero = new StateHorseFollowVirtualHero(this);
            _stateDashMove = new StateDashMove(this);
            // _stateCoerced = new StateCoerced(this);
            // _stateAttackFortifiction = new StateAttackFortification(this);
            // _stateHeroOrVirtualGuard = new StateHeroOrVirtualGuard(this);
            _stateMoveTo = new StateMoveTo(this);
            // _stateAppointTarget = new StateAppointTarget(this);
            // _stateCharmSearch = new StateCharmSearch(this);
            // _stateHeroOrVirtualVigilance = new StateHeroOrVirtualVigilance(this);
            // _stateGuardWallAttck = new StateGuardWallAttck(this);
            _stateVirtualHeroAssignTarget = new StateVirtualHeroAssignTarget(this);
            _stateWaitVirtualHeroAssignTarget = new StateWaitVirtualHeroAssignTarget(this);
            _stateMoveToTarget = new StateMoveToTarget(this);
        }

        private void TickState()
        {
            if (_curState != null)
                _curState.Tick();
        }

        public int ComputeMoveDisInOneFrame()
        {
            return ComputeMoveDisInOneFrameBySpeed(GetSpeed());
        }

        public int ComputeMoveDisInOneFrameBySpeed(int speed)
        {
            int moveDis = _map.GetFrameDeltaMs() * speed / 1000;
            return moveDis;
        }

        private bool EstimateMoveResultBySpeed(FixInt2 target, ref FixInt2 newPos, FixFraction disScale, int speed)
        {
            int moveDis = ComputeMoveDisInOneFrameBySpeed(speed) * disScale;
            FixInt2 dir = (target - WorldPos);

            long disToTarget = dir.sqrMagnitude;
            if (disToTarget <= moveDis * moveDis)
            {
                newPos = target;
                return true;
            }

            dir.Normalize();
            newPos = WorldPos + dir * moveDis / FixInt2.Scale;
            return false;

        }

        private bool EstimateMoveResult(FixInt2 target, ref FixInt2 newPos, FixFraction disScale)
        {
            return EstimateMoveResultBySpeed(target, ref newPos, disScale, GetSpeed());
        }

        //查看点是否被别人占用（会先移除自身的占用）
        private bool IsGridOccupiedByOther(int x, int y)
        {
            bool needRecover = false;
            if (_isOccupyGrid)
            {
                Internal_RemoveCurrentGridOccupy_Temp(_unitType);
                needRecover = true;
            }

            Node node = _map.GetNode(x, y);
            Utils.Assert(node != null);

            bool isOccupied = node.IsOccupied(_unitType);

            if (needRecover)
            {
                Internal_AddCurrentGridOccupy_Temp(_unitType);
            }

            return isOccupied;
        }


        //会改变worldPos。返回移动是否成立。同时支持占据格子和不占据格子的
        //PosConstrainFunc：结果位置是否合法的函数。如果不合法，函数返回false
        //自身占据格子的情况下，目标点有人也会返回false
        public bool TryToMove(FixInt2 targetPos, ref bool isReachTarget, FixFraction disScale, PositionConstrainFunc PosConstrainFunc, bool needFace=false)
        {
            return TryToMoveBySpeed(targetPos, GetSpeed(), ref isReachTarget, disScale, PosConstrainFunc, needFace);
        }

        public bool TryToMoveBySpeed(FixInt2 targetPos, int speed, ref bool isReachTarget, FixFraction disScale, PositionConstrainFunc PosConstrainFunc, bool needFace = false)
        {
            bool bRet = false;
            FixInt2 newPos = FixInt2.zero;
            isReachTarget = EstimateMoveResultBySpeed(targetPos, ref newPos, disScale, speed);
            Integer2 newGridPos = _map.WorldPosToGrid(newPos);

            Integer2 ObstacledPos = new Integer2();
            Integer2 openPos = new Integer2();
            if (GetMap().RaycastObstacled(UnitState, _gridPos, newGridPos, ref ObstacledPos, ref openPos))
            {
                isReachTarget = true;
                bRet = true;
                newGridPos = openPos;
                Utils.Assert(openPos.x >= 0 && openPos.x < GetMap().GetWidth());
                Utils.Assert(openPos.y >= 0 && openPos.y < GetMap().GetHeight());
                newPos = _map.GetGridCenter(openPos.x, openPos.y);
            }
            else if (_isOccupyGrid)
            {
                bool isOccupiedByOther = IsGridOccupiedByOther(newGridPos.x, newGridPos.y);
                if (isOccupiedByOther)
                {
                    isReachTarget = false;
                    return false;
                }
                bRet = true;
            }

            if (PosConstrainFunc != null)
            {
                bool isNewPosOK = PosConstrainFunc(newPos);
                if (!isNewPosOK)
                {

                }

            }
            if (needFace)
            {
                SetFaceDirToTargetVec2(newPos);
            }
            WorldPos = newPos;
            SetGridPos(newGridPos);// _map.WorldPosToGrid(WorldPos);

            //更改格子的占用
            if (_isOccupyGrid)
                Internal_ChangeOccupyGrid(newGridPos.x, newGridPos.y);

            return bRet;
        }

        //会改变worldPos。返回移动是否成立。同时支持占据格子和不占据格子的
        //PosConstrainFunc：结果位置是否合法的函数。如果不合法，函数返回false
        //自身占据格子的情况下，目标点有人也会返回false
        public bool TryToMoveAndReach(FixInt2 targetPos, int speed, ref EReachTargetSt eReachTarget, FixFraction disScale, PositionConstrainFunc PosConstrainFunc, bool needFace = false)
        {
            bool bRet = false;
            FixInt2 newPos = FixInt2.zero;
            bool isReachTarget = EstimateMoveResultBySpeed(targetPos, ref newPos, disScale, speed);
            Integer2 newGridPos = _map.WorldPosToGrid(newPos);
            if (isReachTarget)
                eReachTarget = EReachTargetSt.Normal;

            Integer2 ObstacledPos = new Integer2();
            Integer2 openPos = new Integer2();
            if (GetMap().RaycastObstacled(UnitState, _gridPos, newGridPos, ref ObstacledPos, ref openPos))
            {
                eReachTarget = EReachTargetSt.BeIntercept;
                bRet = true;
                //不能站在障碍物上，需要回退到没有障碍物那格
                if(!(openPos.x >= 0 && openPos.x < GetMap().GetWidth()) || !(openPos.y >= 0 && openPos.y < GetMap().GetHeight()))
                {
                    var i = 0;
                }
                newGridPos = openPos;
                newPos = _map.GetGridCenter(openPos.x, openPos.y);
            }
            else if (_isOccupyGrid)
            {
                bool isOccupiedByOther = IsGridOccupiedByOther(newGridPos.x, newGridPos.y);
                if (isOccupiedByOther)
                {
                    eReachTarget = EReachTargetSt.BeStop;
                    return false;
                }
                bRet = true;
            }

            if (PosConstrainFunc != null)
            {
                bool isNewPosOK = PosConstrainFunc(newPos);
                if (!isNewPosOK)
                {

                }

            }
            if (needFace)
            {
                SetFaceDirToTargetVec2(newPos);
            }
            WorldPos = newPos;
            SetGridPos(newGridPos);// _map.WorldPosToGrid(WorldPos);

            //更改格子的占用
            if (_isOccupyGrid)
                Internal_ChangeOccupyGrid(newGridPos.x, newGridPos.y);

            return bRet;
        }

        public void SysPos(FixInt2 targetPos)
        {
            var oldGrid = _gridPos;
            targetPos = _map.ClampWorldPosInMap(targetPos);
            WorldPos = targetPos;
            SetGridPos(_map.WorldPosToGrid(targetPos));
            if (oldGrid != _gridPos)
            {
                if (_isOccupyGrid)
                    Internal_ChangeOccupyGrid(_gridPos.x, _gridPos.y);
            }
        }

        public bool TryToMove_JumpTo(FixInt2 targetPos, ref bool isReachTarget)
        {
            FixInt2 newPos = FixInt2.zero;
            isReachTarget = EstimateMoveResult(targetPos, ref newPos, FixFraction.one);
            Integer2 newGridPos = _map.WorldPosToGrid(newPos);

            WorldPos = newPos;
            SetGridPos(_map.WorldPosToGrid(WorldPos));

            return true;
        }

        //切换状态为停在原地（可以发呆、施法）
        public void ChangeState_Stop()
        {
            if (_curState != null)
                _curState.Exit();
            _stateStop.Enter(-1);
            UnitState = EUnitState.Stop;
            _curState = _stateStop;
        }

        public void ChangeState_StopAttackTo(int targetUnitIndex)
        {
            if (_curState != null)
                _curState.Exit();
            _stateAttack.Enter(targetUnitIndex);
            UnitState = EUnitState.Attack;
            _curState = _stateAttack;
        }

        //切换状态为攻击
        public void ChangeState_Attack(int targetUnitIndex)
        {
            if (_curState != null)
                _curState.Exit();
            _stateAttack.Enter(targetUnitIndex);
            UnitState = EUnitState.Attack;
            _curState = _stateAttack;
        }

        //切换状态为攻击一个队伍的人。
        //只要传入队伍ID即可，具体攻击哪个敌人、怎么找到前往敌人的最优路线，都是寻路系统自己控制的。
        public void ChangeState_SearchEnemy(int teamIndex)
        {
            if (_curState != null)
                _curState.Exit();
            _stateSearchEnemy.Enter(teamIndex);
            UnitState = EUnitState.SearchEnemy;
            _curState = _stateSearchEnemy;
        }

        //返回敌对Team且在射程内的敌人。如果有，用户可以切换到Stop进行攻击、施法等行为
        public Unit GetTargetInRange_TargetTeam()
        {
            IGetEnemyInRange s = _curState as IGetEnemyInRange;
            if (s != null)
                return s.GetEnemyInRange();
            return null;
        }

        public int GetMoveTargetTeamId_TargetTeam()
        {
            IGetMoveTargetTeamId s = _curState as IGetMoveTargetTeamId;
            if (s != null)
                return s.GetMoveTargetTeamId();
            return 0;
        }

        // public void ChangeState_SlipTo(double x, double y)
        // {
        //     if (_curState != null)
        //         _curState.Exit();
        //     _stateSlipTo.Enter(new FixInt2((int)x * FixInt2.Scale, (int)y * FixInt2.Scale));
        //     UnitState = EUnitState.SlipTo;
        //     _curState = _stateSlipTo;
        // }

        // public void ChangeState_ForceSlip(int sourceId, int x, int y, double force, double acceleration, double mass)
        // {
        //     if (_curState != null)
        //         _curState.Exit();
        //     _stateForceSlip.Enter(sourceId, new FixInt2(x, y), (int)(force * FixInt2.Scale), (int)(acceleration * FixInt2.Scale), (int)(mass * FixInt2.Scale));
        //     UnitState = EUnitState.ForceSlip;
        //     _curState = _stateForceSlip;
        // }

        public void ChangeState_MoveTo(int worldPosX, int worldPosY, int speed, bool isMoveToPos)
        {
            if (_curState != null)
                _curState.Exit();
            _stateMoveTo.Enter(new FixInt2(worldPosX, worldPosY), speed, isMoveToPos);
            UnitState = EUnitState.MoveTo;
            _curState = _stateMoveTo;
        }

        public void ChangeState_StopVoid()
        {
            if (UnitState == EUnitState.StopVoid)
                return;
            if (_curState != null)
                _curState.Exit();
            _stateStopVoid.Enter();
            UnitState = EUnitState.StopVoid;
            _curState = _stateStopVoid;
        }
        public void ChangeState_MoveStraight(int worldPosX, int worldPosY)
        {
            if (_curState != null)
                _curState.Exit();
            _stateMoveStraightTo.Enter(new FixInt2(worldPosX, worldPosY));
            UnitState = EUnitState.MoveStraightTo;
            _curState = _stateMoveStraightTo;
        }

        public void ChangeState_JumpTo(int worldPosX, int worldPosY)
        {
            FixInt2 worldPos = (new FixInt2(worldPosX, worldPosY));
            if (_curState != null)
                _curState.Exit();
            _stateJumpTo.Enter(worldPos);
            UnitState = EUnitState.JumpTo;
            _curState = _stateJumpTo;
        }

        public void ChangeState_FollowHero()
        {
            if (_curState != null)
                _curState.Exit();
            _stateFollowHero.Enter();
            UnitState = EUnitState.FollowHero;
            _curState = _stateFollowHero;
        }

        //虚拟英雄移动到某个单位
        public void ChangeState_VirtualHeroMovePathTo(int targetUnitIndex)
        {
            if (_curState != null)
                _curState.Exit();
            _stateVirtualHeroMovePathTo.Enter(targetUnitIndex);
            UnitState = EUnitState.VirtualHeroMovePathTo;
            _curState = _stateVirtualHeroMovePathTo;
        }

        /// <summary>
        /// 虚拟英雄指定攻击目标
        /// </summary>
        /// <param name="attackTargetVirtualHeroId"></param>
        public void ChangeState_VirtualHeroAssignTarget(int attackTargetVirtualHeroId)
        {
            if (_curState != null)
                _curState.Exit();
            _stateVirtualHeroAssignTarget.Enter(attackTargetVirtualHeroId);
            UnitState = EUnitState.VirtualHeroAssignTarget;
            _curState = _stateVirtualHeroAssignTarget;
        }
        /// <summary>
        /// 小兵等待虚拟英雄指定攻击目标
        /// </summary>
        public void ChangeState_WaitVirtualHeroAssignTarget()
        {
            if (_curState != null)
                _curState.Exit();
            _stateWaitVirtualHeroAssignTarget.Enter();
            UnitState = EUnitState.WaitVirtualHeroAssignTarget;
            _curState = _stateWaitVirtualHeroAssignTarget;
        }

        public bool RequestVirtualHeroAssignTarget(int unitId)
        {
            if (_curState != null)
            {
                if (_curState is StateVirtualHeroAssignTarget)
                {
                    _stateVirtualHeroAssignTarget.RequestAssignTarget(unitId);
                    return true;
                }
            }
            return false;
        }

        public int GetVirtualHeroAssignTarget(int unitId)
        {
            if (_curState != null)
            {
                if (_curState is StateVirtualHeroAssignTarget)
                {
                    return _stateVirtualHeroAssignTarget.GetSoldierTarget(unitId);
                }
            }
            return 0;
        }

        //小兵跟随本队的虚拟英雄移动
        public void ChangeState_SoldierFollowVirtualHero()
        {
            if (_curState != null)
                _curState.Exit();
            _stateSoldierFollowVirtualHero.Enter();
            UnitState = EUnitState.SoldierFollowVirtualHero;
            _curState = _stateSoldierFollowVirtualHero;
        }

        //小兵自由寻敌
        public void ChangeState_SoldierSearchEnemy(int targetUnitIndex)
        {
            if (_curState != null)
                _curState.Exit();
            _stateSoldierSearchEnemy.Enter(targetUnitIndex);
            UnitState = EUnitState.SoldierSearchEnemy;
            _curState = _stateSoldierSearchEnemy;
        }

        //带转向速度限制的移动（骑兵队长使用）
        public void ChangeState_MoveWithTurnAngle(FixInt2 target, int initDirInRadian, int turnAngleSpeedRadianPerSecend, int circleRadius)
        {
            if (_curState != null)
                _curState.Exit();
            _stateMoveWithTurnAngle.Enter(target, initDirInRadian, turnAngleSpeedRadianPerSecend, circleRadius);
            UnitState = EUnitState.MoveWithTurnAngle;
            _curState = _stateMoveWithTurnAngle;
        }
        //带转向速度的小兵跟随武将移动（骑兵使用）
        public void ChangeState_HorseFollowVirtualHero(int initDirInRadian, int turnAngleSpeedRadianPerSecend)
        {
            if (_curState != null)
                _curState.Exit();
            _stateHorseFollowVirtualHero.Enter(initDirInRadian, turnAngleSpeedRadianPerSecend);
            UnitState = EUnitState.HorseFollowVirtualHero;
            _curState = _stateHorseFollowVirtualHero;
        }

        //英雄开战寻路无视碰撞移动到目标
        public void ChangeState_HeroMoveStraightTo(int worldPosX, int worldPosY)
        {
            if (_curState != null)
                _curState.Exit();
            _stateHeroMoveStraightTo.Enter(new FixInt2(worldPosX, worldPosY));
            UnitState = EUnitState.HeroMoveStraightTo;
            _curState = _stateHeroMoveStraightTo;
        }
        //英雄寻路移动到目标
        public void ChangeState_HeroMovePathTo(int targetUnitId)
        {
            if (_curState != null)
                _curState.Exit();
            _stateHeroMovePathTo.Enter(targetUnitId);
            UnitState = EUnitState.HeroMovePathTo;
            _curState = _stateHeroMovePathTo;
        }

        //英雄开始自由寻敌
        public void ChangeState_HeroSearchEnemy(int targetUnitIndex)
        {
            if (_curState != null)
                _curState.Exit();
            _stateHeroSearchEnemy.Enter(targetUnitIndex);
            UnitState = EUnitState.HeroSearchEnemy;
            _curState = _stateHeroSearchEnemy;
        }

        // 小兵开始冲撞
        public void ChangeState_DashMove(FixInt faceOffSet, FixInt dashRadius,
            int damageRadius, int dashJumpDamgeTime,
            int dashHeroExtraW, int dashHeroExtraH,
            FixInt maxSlidDur)
        {
            if (_curState != null)
                _curState.Exit();
            _stateDashMove.Enter(faceOffSet, dashRadius, damageRadius, dashJumpDamgeTime,
                dashHeroExtraW, dashHeroExtraH, maxSlidDur);
            UnitState = EUnitState.DashMove;
            _curState = _stateDashMove;
        }

        // 切换到被裹挟状态
        // public void ChangeState_StateCoerced(int ownuidx, int offsetDist, int coercedRadius, int speed)
        // {
        //     if (_curState != null)
        //         _curState.Exit();
        //     _stateCoerced.Enter(ownuidx, offsetDist, coercedRadius, speed);
        //     UnitState = EUnitState.Coerced;
        //     _curState = _stateCoerced;
        // }

        // 切换到攻击工事状态
        // public void ChangeState_StateAttackFortification(int x, int y)
        // {
        //     if (_curState != null)
        //         _curState.Exit();
        //     _stateAttackFortifiction.Enter(x, y);
        //     UnitState = EUnitState.AttackFortification;
        //     _curState = _stateAttackFortifiction;
        // }

        // 切换到防守状态
        // public void ChangeState_StateHeroOrVirtualGuard()
        // {
        //     if (_curState != null)
        //         _curState.Exit();
        //     _stateHeroOrVirtualGuard.Enter();
        //     UnitState = EUnitState.Guard;
        //     _curState = _stateHeroOrVirtualGuard;
        // }

        // public void ChangeState_UnitAppointSearch(int uid)
        // {
        //     if (_curState != null)
        //         _curState.Exit();
        //     _stateAppointTarget.Enter(uid);
        //     UnitState = EUnitState.AppointTarget;
        //     _curState = _stateAppointTarget;
        // }

        // public void ChangeState_UnitCharmSearch()
        // {
        //     if (_curState != null)
        //         _curState.Exit();
        //     _stateCharmSearch.Enter();
        //     UnitState = EUnitState.CharmSearch;
        //     _curState = _stateCharmSearch;
        // }

        // 切换到巡逻状态
        // public void ChangeState_StateHeroOrVirtualVigilance(int toPosX, int toPosY)
        // {
        //     if (_curState != null)
        //         _curState.Exit();
        //     _stateHeroOrVirtualVigilance.Enter(toPosX, toPosY);
        //     UnitState = EUnitState.Vigilance;
        //     _curState = _stateHeroOrVirtualVigilance;
        // }

        // 切换到防守城墙状态
        // public void ChangeState_StateGuardWallAttack()
        // {
        //     if (_curState != null)
        //         _curState.Exit();
        //     _stateGuardWallAttck.Enter();
        //     UnitState = EUnitState.GuardWallAttack;
        //     _curState = _stateGuardWallAttck;
        // }

        public void ChangeState_MoveToTargetById(int targetId)
        {
            if (targetId == -1)
                return;
            if (_curState != null)
                _curState.Exit();
            _stateMoveToTarget.Enter(targetId);
            UnitState = EUnitState.MoveToTarget;
            _curState = _stateMoveToTarget;
        }

        public EUnitState GetCurrentStateEnum()
        {
            return _unitState;
        }

        public State GetCurrentStateObject()
        {
            return _curState;
        }

        public Unit GetTargetInRange()
        {
            IGetEnemyInRange s = _curState as IGetEnemyInRange;
            if(s != null)
                return s.GetEnemyInRange();
            return null;
        }

        public void SetTarget(int x, int y)
        {
            var p = new FixInt2(x, y);
            ISetMoveTargetPosition s = _curState as ISetMoveTargetPosition;
            if(s != null)
                s.SetMoveTargetPosition(p);
        }

        public void SetTargetUnit(Unit u)
        {
            var s = _curState as ISetMoveTargetUnit;
            if(s != null)
                s.SetMoveTargetUnit(u);
        }

        public List<int> GetDashEnemyList()
        {
            IGetDashEnemy s = _curState as IGetDashEnemy;
            if (s != null)
                return s.GetDashEnemy();
            return null;
        }

        public List<int> GetDashDamageEnemyList()
        {
            IGetDashDamageEnemy s = _curState as IGetDashDamageEnemy;
            if (s != null)
                return s.GetDashDamageEnemy();
            return null;
        }

        public Node GetFortificationNode()
        {
            IGetFortificationNode s = _curState as IGetFortificationNode;
            if (s != null)
                return s.GetFortificationNode();
            return null;
        }
    }
}