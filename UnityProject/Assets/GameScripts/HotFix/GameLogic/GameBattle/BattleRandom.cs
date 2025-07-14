using System;


namespace M.PathFinding
{
    public class BattleRandom
    {
        private Random m_Rand = null;

        public BattleRandom(int seed)
        {
            m_Rand = new Random(seed);
        }

        public virtual int Next(int minValue, int maxValue)
        {
            return m_Rand.Next(minValue, maxValue);
        }
    }
}