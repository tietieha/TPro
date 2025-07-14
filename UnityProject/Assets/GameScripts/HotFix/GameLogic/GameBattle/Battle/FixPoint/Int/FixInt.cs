using System;

namespace FixPoint
{
    public struct FixInt : IEquatable<FixInt>
    {
        private int m_value;

        public int Value
        {
            get
            {
                return this.m_value;
            }
            set
            {
                this.m_value = value;
            }
        }

        public FixInt(int i)
        {
            this.m_value = i;
        }

        public bool Equals(FixInt other)
        {
            return m_value == other.m_value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))return false;
            return obj is FixInt other && Equals(other);
        }

        public override int GetHashCode()
        {
            return m_value.GetHashCode();
        }

        public static implicit operator FixInt(int i)
        {
            return new FixInt(i);
        }

        public static FixInt operator +(FixInt a, FixInt b)
        {
            return new FixInt(a.m_value + b.m_value);
        }

        public static FixInt operator -(FixInt a, FixInt b)
        {
            return new FixInt(a.m_value - b.m_value);
        }

        public static FixInt operator *(FixInt a, FixInt b)
        {
            return new FixInt(a.m_value * b.m_value);
        }

        public static bool operator ==(FixInt lhs, FixInt rhs)
        {
            return lhs.m_value == rhs.m_value;
        }

        public static bool operator !=(FixInt lhs, FixInt rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator >(FixInt lhs, FixInt rhs)
        {
            return lhs.Value > rhs.Value;
        }

        public static bool operator <(FixInt lhs, FixInt rhs)
        {
            return lhs.Value < rhs.Value;
        }
    }

}