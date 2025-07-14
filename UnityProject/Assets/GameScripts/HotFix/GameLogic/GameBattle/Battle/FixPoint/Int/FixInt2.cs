using System;


namespace FixPoint
{
    public struct FixInt2 : IEquatable<FixInt2>
    {
        public static FixInt2 zero = default(FixInt2);
        public static readonly FixInt2 right = new FixInt2(1000,0);
        public static readonly FixInt2 left = new FixInt2(-1000, 0);
        public const int Scale = 1000;
        public const double InverseScale = 0.001;
        private int m_X;
        private int m_Y;

        public int x
        {
            get
            {
                return this.m_X;
            }
            set
            {
                this.m_X = value;
            }
        }

        public int y
        {
            get
            {
                return this.m_Y;
            }
            set
            {
                this.m_Y = value;
            }
        }

        public int magnitude
        {
            get { return FixMath.Sqrt(sqrMagnitude); }
        }

        public long sqrMagnitude
        {
            get
            {
                long x_long = (long)m_X;
                long y_long = (long)m_Y;
                return x_long * x_long + y_long * y_long;
            }
        }

        public FixInt2(int x, int y)
        {
            this.m_X = x;
            this.m_Y = y;
        }


        public void Normalize()
        {
            long num = (long)(this.x * 100);
            long num2 = (long)(this.y * 100);
            ulong num3 = (ulong)(num * num + num2 * num2);
            if (num3 == 0L)
            {
                return;
            }
            long b = (long)FixMath.SqrtLong(num3);
            this.x = (int)FixMath.DivLong(num * 1000L, b);
            this.y = (int)FixMath.DivLong(num2 * 1000L, b);
        }

        public FixInt2 NormalizeTo(int newMagn)
        {
            long num = (long)(this.x * 100);
            long num2 = (long)(this.y * 100);
            long num4 = num * num + num2 * num2;
            if (num4 == 0L)
            {
                return this;
            }
            long b = (long)FixMath.Sqrt(num4);
            long num5 = (long)newMagn;
            this.x = (int)FixMath.DivLong(num * num5, b);
            this.y = (int)FixMath.DivLong(num2 * num5, b);
            return this;
        }

        public static FixFraction RadianInt(FixInt2 lhs, FixInt2 rhs)
        {
            long den = (long)lhs.magnitude * (long)rhs.magnitude;
            return FixMath.Acos((long)FixInt2.Dot(ref lhs, ref rhs), den);
        }

        public  FixInt2 Rotate(ref FixFraction radian)
        {
            FixMath.Sincos(out FixFraction sin, out FixFraction cos, radian.nominal, radian.denominal);
            long num = cos.nominal * sin.denominal;
            long num2 = cos.denominal * sin.nominal;
            long b = cos.denominal * sin.denominal;
            FixInt2 ret = FixInt2.zero;
            ret.x = (int)FixMath.DivLong((long)this.x * num + (long)this.y * num2, b);
            ret.y = (int)FixMath.DivLong(-(long)this.x * num2 + (long)this.y * num, b);

            return ret;

        }


        public static FixInt2 ClampMagnitude(FixInt2 v, int maxLength)
        {
            long sqrMagnitudeLong = v.sqrMagnitude;
            long num = (long)maxLength;
            if (sqrMagnitudeLong > num * num)
            {
                long b = (long)FixMath.Sqrt(sqrMagnitudeLong);
                int num2 = (int)FixMath.DivLong((long)(v.x * num), b);
                int num3 = (int)FixMath.DivLong((long)(v.y * num), b);
                return new FixInt2(num2, num3);
            }
            return v;
        }

        public static int Dot(ref FixInt2 a, ref FixInt2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static long DotLong( FixInt2 a,  FixInt2 b)
        {
            return (long)a.x * (long)b.x + (long)a.y * (long)b.y;
        }

        public static long CrossLong( FixInt2 a,  FixInt2 b)
        {
            return (long)a.x * (long)b.y - (long)a.y * (long)b.x;

        }

        public override bool Equals(object other)
        {
            if (!(other is FixInt2))
                return false;
            return this.Equals((FixInt2)other);
        }

        public bool Equals(FixInt2 other)
        {
            return this.m_X == other.m_X && m_Y == other.m_Y;
        }

        public override int GetHashCode()
        {
            return this.m_X.GetHashCode() ^ this.m_Y.GetHashCode() << 2;
        }

        public static FixInt2 Min(FixInt2 lhs, FixInt2 rhs)
        {
            return new FixInt2(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));
        }

        public static FixInt2 Max(FixInt2 lhs, FixInt2 rhs)
        {
            return new FixInt2(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));
        }


        public static FixInt2 operator +(FixInt2 a, FixInt2 b)
        {
            return new FixInt2(a.m_X + b.m_X, a.m_Y + b.m_Y);
        }

        public static FixInt2 operator -(FixInt2 a, FixInt2 b)
        {
            return new FixInt2(a.m_X - b.m_X, a.m_Y - b.m_Y);
        }

        public static FixInt2 operator *(FixInt2 a, FixInt2 b)
        {
            return new FixInt2(a.m_X * b.m_X, a.m_Y * b.m_Y);
        }

        public static FixInt2 operator *(FixInt2 a, int b)
        {
            return new FixInt2(a.m_X * b, a.m_Y * b);
        }

        public static FixInt2 operator -(FixInt2 lhs)
        {
            lhs.x = -lhs.x;
            lhs.y = -lhs.y;
            return lhs;
        }

        public static bool operator ==(FixInt2 lhs, FixInt2 rhs)
        {
            return lhs.m_X == rhs.m_X && lhs.m_Y == rhs.m_Y;
        }

        public static bool operator !=(FixInt2 lhs, FixInt2 rhs)
        {
            return !(lhs == rhs);
        }

        public static FixInt2 operator /(FixInt2 lhs, int rhs)
        {
            lhs.x /= rhs;
            lhs.y /= rhs;
            return lhs;
        }



    }
}

