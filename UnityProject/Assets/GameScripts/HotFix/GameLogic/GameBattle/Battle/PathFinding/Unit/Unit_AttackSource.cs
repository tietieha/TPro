using System.Collections.Generic;

namespace M.PathFinding
{
    public partial class Unit
    {
        //我的攻击目标。正在打的，或者正准备打的。SearchEnemy相关的状态中，会在找到目标的时候设置这个值。可以被清空
        public Unit WillAttackTarget { set; get; } = null;

        public HashSet<Unit> WillAttackSourceSet { get; set; } = new HashSet<Unit>(); //攻击来源

        //设置接下来要攻击谁
        public void SetWillAttackTarget(Unit u)
        {
            if (u == WillAttackTarget || this.IsVirtual() || (u != null && (u.IsVirtual() || u.IsDead())))
                return;

            //减去上次的攻击目标
            var oldTarget = WillAttackTarget;
            if (oldTarget != null)
            {
                this.WillAttackTarget = null;
                this.Team.RemoveWillAttackUnit(oldTarget);

                oldTarget.WillAttackSourceSet.Remove(this);
                oldTarget.Team.RemoveWillAttackSourceUnit(this);
            }

            WillAttackTarget = u;
            _SetWillAttackUnitId(u?.GetId() ?? 0);
            if (u != null)
            {
                this.Team.AddWillAttackUnit(u);

                u.WillAttackSourceSet.Add(this);
                u.Team.AddWillAttackSourceUnit(this);
            }

        }

        //清空WillAttack目标
        public void ClearWillAttackTarget()
        {
            SetWillAttackTarget(null);
        }

        readonly private List<Unit> outMelleeAttackSource = new List<Unit>(100);
        //获取近战攻击来源
        public List<Unit> GetMeleeWillAttackSource()
        {
            outMelleeAttackSource.Clear();
            foreach (var u in WillAttackSourceSet)
            {
                if (u.IsMelee())
                    outMelleeAttackSource.Add(u);
            }
            return outMelleeAttackSource;
        }

        //获取远程攻击来源
        public List<Unit> GetLongRangeWillAttackSource()
        {
            outMelleeAttackSource.Clear();
            foreach (var u in WillAttackSourceSet)
            {
                if (!u.IsMelee())
                    outMelleeAttackSource.Add(u);
            }
            return outMelleeAttackSource;
        }
    }

}