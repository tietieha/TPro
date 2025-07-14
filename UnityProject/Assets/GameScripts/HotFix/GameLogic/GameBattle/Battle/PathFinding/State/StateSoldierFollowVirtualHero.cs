//小兵跟随虚拟英雄移动
using System;
using System.Collections.Generic;
using FixPoint;

namespace M.PathFinding
{
    public class StateSoldierFollowVirtualHero : State
    {
        private int _pathTestNodeMax = 20;
        //目标位置
        private FixInt2 _targetPos;
        private ERequestPriority _pathPriority = ERequestPriority.Low;
        private readonly int _pathRecomputeInterval = 4;

        //目标Unit。一般是敌对军团的Hero，可能是虚拟
        public StateSoldierFollowVirtualHero(Unit unit)
        {
            _unit = unit;
        }

        //进入这个状态
        public void Enter()
        {
            //_unit.Log("Enter State: StateSoldierFollowVirtualHero");
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
            _unit.ClearWillAttackTarget();
            _unit.InternalEnableOccupy_Stand();

            _pathPriority = ERequestPriority.Low;
        }

        public override void Tick()
        {
            //找到虚拟武将，试图移动到虚拟武将。
            //把衣架落脚点最近的位置作为移动目标
            //寻路路径非常短
            //开启碰撞

            Utils.Assert(!_unit.IsHero());

            // 确保站在格子上
            _unit.InternalEnableOccupy_Stand();

            var hero = _unit.GetMap().GetTeam(_unit.GetTeamId()).Hero;
            var p = _unit.GetPosRelativeToHero();

            FixInt2 d = hero.FaceDirNormalized;
            int newX = (p.x * d.x - p.y * d.y) / FixInt2.Scale;
            int newY = (p.x * d.y + p.y * d.x) / FixInt2.Scale;

            var t = new FixInt2(newX, newY) + hero.GetWorldPos();
            t = _unit.GetMap().ClampWorldPosInMap(t);
            _targetPos = t;

            int stopDis = Math.Max(_unit.GetRadius(), 500);
            var waitingPathId = _unit.PathState.GetCurrentWaitingPathId();
            var path = _unit.PathState.GetCurrentPath();

            if (path.Count == 0)
            {
                if (waitingPathId < 0) //如果没有在等待路径计算
                {
                    //如果没有路，并且没有在等待路径返回。说明是在发呆
                    _unit.SendPathFindingRequest_Position(_targetPos, stopDis,_pathTestNodeMax, ERequestPriority.Medium);
                    _pathPriority = ERequestPriority.Low;

                }
                else //如果正在等待路径计算
                {

                }

            }
            else
            {
                if (path.Count > 0)
                {
                    FixFraction disScale = FixFraction.one;
                    int moveDisInOneFrame = _unit.ComputeMoveDisInOneFrame();
                    int movedDisAccum = 0;
                    while (true)
                    {
                        if (path.Count <= 0)
                            break;
                        var startPos = _unit.GetWorldPos();
                        var targetPos = path.Peek();
                        bool isReachTarget = false;
                        bool moveOk = _unit.TryToMove(targetPos, ref isReachTarget, disScale, null, true);
                        if (!moveOk)
                        {
                            _unit.PathState.ClearPath();
                            break;
                        }

                        //达到了这个路点，从路径中移除
                        if (isReachTarget)
                        {
                            int movedDis = (targetPos - startPos).magnitude;
                            movedDisAccum += movedDis;
                            disScale = FixFraction.one - new FixFraction(movedDisAccum, moveDisInOneFrame); // movedDisAccum / moveDisInOneFrame;
                            path.Dequeue();
                        }
                        else
                            break;
                    }
                }

                if (_unit.PathState.GetPathLife() > _pathRecomputeInterval && waitingPathId < 0)
                {
                    _unit.SendPathFindingRequest_Position(_targetPos, stopDis, _pathTestNodeMax, ERequestPriority.Low);
                }
            }
        }

        public override void Exit()
        {
            _unit.PathState.CancelPathFinding();
            _unit.PathState.ClearPath();
        }

        public override void Reset()
        {
            _pathTestNodeMax = 20;
            _targetPos = FixInt2.zero;
            _pathPriority = ERequestPriority.Low;
        }
    }
}