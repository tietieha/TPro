using System;
 

namespace M.PathFinding
{
    public struct Integer2
    {
        public int x;
        public int y;

        public Integer2(int ix, int iy)
        {
            x = ix;
            y = iy;
        }

        public void Set(int X, int Y)
        {
            x = X;
            y = Y;
        }

        public static bool operator ==(Integer2 a, Integer2 b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Integer2 a, Integer2 b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public override bool Equals(Object obj)
        {
            return obj is Integer2 integer && this == integer;
        }
        public override int GetHashCode()
        {
            return x + y;
        }

        public static Integer2 operator+(Integer2 a, Integer2 b)
        {
            Integer2 r;
            r.x = a.x + b.x;
            r.y = a.y + b.y;
            return r;
        }

    }



}