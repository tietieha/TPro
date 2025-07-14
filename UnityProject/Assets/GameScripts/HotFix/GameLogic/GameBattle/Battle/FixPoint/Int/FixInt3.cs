using System;

namespace FixPoint
{

    public struct FixInt3 : IEquatable<FixInt3>
    {
        private int m_X;
        private int m_Y;
        private int m_Z;

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

        public int z
        {
            get
            {
                return this.m_Z;
            }
            set
            {
                this.m_Z = value;
            }
        }

        public void Set(int x, int y, int z)
        {
            this.m_X = x;
            this.m_Y = y;
            this.m_Z = z;
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.x;
                    case 1:
                        return this.y;
                    case 2:
                        return this.z;
                    default:
                        throw new IndexOutOfRangeException("Invalid FixInt3 adressable");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.x = value;
                        break;
                    case 1:
                        this.y = value;
                        break;
                    case 2:
                        this.z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid FixInt3 adressable");
                }
            }
        }
        public long sqrMagnitude
        {
            get
            {
                long num = (long)this.x;
                long num2 = (long)this.y;
                long num3 = (long)this.z;
                return num * num + num2 * num2 + num3 * num3;
            }
        }




        public static FixInt3 Min(FixInt3 lhs, FixInt3 rhs)
        {
            return new FixInt3(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));
        }

        public static FixInt3 Max(FixInt3 lhs, FixInt3 rhs)
        {
            return new FixInt3(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));
        }

        public static FixInt3 Scale(FixInt3 a, FixInt3 b)
        {
            return new FixInt3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public FixInt3(int x, int y, int z)
        {
            this.m_X = x;
            this.m_Y = y;
            this.m_Z = z;
        }


        public static FixInt3 operator +(FixInt3 a, FixInt3 b)
        {
            return new FixInt3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static FixInt3 operator -(FixInt3 a, FixInt3 b)
        {
            return new FixInt3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static FixInt3 operator *(FixInt3 a, FixInt3 b)
        {
            return new FixInt3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static FixInt3 operator *(FixInt3 a, int b)
        {
            return new FixInt3(a.x * b, a.y * b, a.z * b);
        }

        public static bool operator ==(FixInt3 lhs, FixInt3 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator !=(FixInt3 lhs, FixInt3 rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            if (!(other is FixInt3))
                return false;
            return this.Equals((FixInt3)other);
        }


        public bool Equals(FixInt3 other)
        {
            return this.m_X == other.m_X && m_Y == other.m_Y && m_Z == other.m_Z;
        }

        public override int GetHashCode()
        {
            int hashCode1 = this.y.GetHashCode();
            int hashCode2 = this.z.GetHashCode();
            return this.x.GetHashCode() ^ hashCode1 << 4 ^ hashCode1 >> 28 ^ hashCode2 >> 4 ^ hashCode2 << 28;
        }
    }
}

