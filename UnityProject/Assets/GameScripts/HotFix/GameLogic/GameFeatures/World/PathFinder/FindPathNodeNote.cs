using System;
using System.Collections;

namespace World.PathFinder
{
    public class FindPathNodeNote : IComparable<FindPathNodeNote>
    {
        public int index;
        public int[] qr;
        public FindPathNodeNote parent;

        /**
        * A*算法的F G H值
        * F = G + H
        */
        public float f;

        public float g;
        public float h;

        /**
        * 射线算法的阻挡方向，记录在最后一个可达点上，标识射线往哪个方向受到了阻拦，便于后续绕路或重新打射线
        */
        public int noMoveDir;

        public FindPathNodeNote(int index, int[] qr)
        {
            this.index = index;
            this.qr = qr;
        }

        public void BuildPath(ArrayList path)
        {
            parent?.BuildPath(path);
            path.Add(index);
        }

        public int CompareTo(FindPathNodeNote other)
        {
            if (f > other.f)
            {
                return 1;
            }

            if (f < other.f)
            {
                return -1;
            }

            return 0;
        }
    }
}
