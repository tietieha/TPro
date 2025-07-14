using FixPoint;
using System.Collections.Generic;
using XLua;

namespace M.PathFinding
{
    [LuaCallCSharp]
    public abstract class State
    {
        protected Unit _unit = null;
        public abstract void Tick();
        public abstract void Exit();
        public abstract void Reset();

        //寻路请求结果返回的时候，这个会被调用。
        public virtual void OnPathFindFinished(PathState ps) { }
    }

    //可以设置目标位置
    public interface ISetMoveTargetPosition
    {
        void SetMoveTargetPosition(FixInt2 target);
        bool IsReachTarget();
        EReachTargetSt GetReachTargetSt();
    }

    //可以设置目标单位
    public interface ISetMoveTargetUnit
    {
        void SetMoveTargetUnit(Unit u);
    }

    public interface ISetMoveTargetTeam
    {
        void SetMoveTargetTeam(int targetTeamId);
    }

    public interface IGetMoveTargetTeamId
    {
        int GetMoveTargetTeamId();
    }

    //可以查询范围内的目标
    public interface IGetEnemyInRange
    {
        Unit GetEnemyInRange();
    }

    public interface IGetDashEnemy
    {
        List<int> GetDashEnemy();
    }

    public interface IGetDashDamageEnemy
    {
        List<int> GetDashDamageEnemy();
    }

    public interface IGetFortificationNode
    {
        Node GetFortificationNode();
    }
}