using System;

namespace FixPoint
{
    public struct FixFraction : IEquatable<FixFraction>
    {
        public long nominal;

        public long denominal;

        public static FixFraction zero = new FixFraction(0L, 1L);
        public static FixFraction one = new FixFraction(10000L, 10000L);

        public static FixFraction pi = new FixFraction(31416L, 10000L);


        public FixFraction(long n, long d)
        {
            this.nominal = n;
            this.denominal = d;
        }


        public override bool Equals(object obj)
        {
            return obj != null && base.GetType() == obj.GetType() && this == (FixFraction) obj;
        }

        public bool Equals(FixFraction other)
        {
            return this == other;
        }

        public static bool operator <(FixFraction a, FixFraction b)
        {
            long num = a.nominal * b.denominal;
            long num2 = b.nominal * a.denominal;
            return (b.denominal > 0L ^ a.denominal > 0L) ? (num > num2) : (num < num2);
        }

        public static bool operator >(FixFraction a, FixFraction b)
        {
            long num = a.nominal * b.denominal;
            long num2 = b.nominal * a.denominal;
            return (b.denominal > 0L ^ a.denominal > 0L) ? (num < num2) : (num > num2);
        }

        public static bool operator <=(FixFraction a, FixFraction b)
        {
            long num = a.nominal * b.denominal;
            long num2 = b.nominal * a.denominal;
            return (b.denominal > 0L ^ a.denominal > 0L) ? (num >= num2) : (num <= num2);
        }

        public static bool operator >=(FixFraction a, FixFraction b)
        {
            long num = a.nominal * b.denominal;
            long num2 = b.nominal * a.denominal;
            return (b.denominal > 0L ^ a.denominal > 0L) ? (num <= num2) : (num >= num2);
        }

        public static bool operator ==(FixFraction a, FixFraction b)
        {
            return a.nominal * b.denominal == b.nominal * a.denominal;
        }

        public static bool operator !=(FixFraction a, FixFraction b)
        {
            return a.nominal * b.denominal != b.nominal * a.denominal;
        }

        public static bool operator <(FixFraction a, long b)
        {
            long num = a.nominal;
            long num2 = b * a.denominal;
            return (a.denominal > 0L) ? (num < num2) : (num > num2);
        }

        public static bool operator >(FixFraction a, long b)
        {
            long num = a.nominal;
            long num2 = b * a.denominal;
            return (a.denominal > 0L) ? (num > num2) : (num < num2);
        }

        public static bool operator <=(FixFraction a, long b)
        {
            long num = a.nominal;
            long num2 = b * a.denominal;
            return (a.denominal > 0L) ? (num <= num2) : (num >= num2);
        }

        public static bool operator >=(FixFraction a, long b)
        {
            long num = a.nominal;
            long num2 = b * a.denominal;
            return (a.denominal > 0L) ? (num >= num2) : (num <= num2);
        }

        public static bool operator ==(FixFraction a, long b)
        {
            return a.nominal == b * a.denominal;
        }

        public static bool operator !=(FixFraction a, long b)
        {
            return a.nominal != b * a.denominal;
        }

        public static FixFraction operator +(FixFraction a, FixFraction b)
        {
            if (a.denominal == b.denominal)
            {
                return new FixFraction
                {


                    nominal = a.nominal + b.nominal,
                    denominal = b.denominal
                };
            }

            return new FixFraction
            {
                nominal = a.nominal * b.denominal + b.nominal * a.denominal,
                denominal = a.denominal * b.denominal
            };
        }

        public static FixFraction operator +(FixFraction a, long b)
        {
            a.nominal += b * a.denominal;
            return a;
        }

        public static FixFraction operator -(FixFraction a, FixFraction b)
        {
            if (a.denominal == b.denominal)
            {
                return new FixFraction
                {
                    nominal = a.nominal - b.nominal,
                    denominal = b.denominal
                };
            }
            return new FixFraction
            {
                nominal = a.nominal * b.denominal - b.nominal * a.denominal,
                denominal = a.denominal * b.denominal
            };
        }

        public static FixFraction operator -(FixFraction a, long b)
        {
            a.nominal -= b * a.denominal;
            return a;
        }

        public static FixFraction operator *(FixFraction a, long b)
        {
            a.nominal *= b;
            return a;
        }

        public static FixFraction operator /(FixFraction a, long b)
        {
            a.denominal *= b;
            return a;
        }

        public static FixInt3 operator *(FixInt3 v, FixFraction f)
        {
            return FixMath.Divide(v, f.nominal, f.denominal);
        }

        public static FixInt2 operator *(FixInt2 v, FixFraction f)
        {
            return FixMath.Divide(v, f.nominal, f.denominal);
        }

        public static FixInt3 operator /(FixInt3 v, FixFraction f)
        {
            return FixMath.Divide(v, f.denominal, f.nominal);
        }

        public static FixInt2 operator /(FixInt2 v, FixFraction f)
        {
            return FixMath.Divide(v, f.denominal, f.nominal);
        }

        public static int operator *(int i, FixFraction f)
        {
            return (int) FixMath.DivLong((long) i * f.nominal, f.denominal);
        }

        public static FixFraction operator -(FixFraction a)
        {
            a.nominal = -a.nominal;
            return a;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (nominal.GetHashCode() * 397) ^ denominal.GetHashCode();
            }
        }
    }
}