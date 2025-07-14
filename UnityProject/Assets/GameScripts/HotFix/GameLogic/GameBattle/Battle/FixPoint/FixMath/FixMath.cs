using System;

namespace FixPoint
{
    public class FixMath
    {
        //  fast sqrt computation https://stackoverflow.com/questions/4930307/fastest-way-to-get-the-integer-part-of-sqrtn, http://vault.embedded.com/98/9802fe2.htm


        public static uint SqrtInt(uint a)
        {
            long rem = 0;
            uint root = 0;
            uint i;

            for (i = 0; i < 16; i++)
            {
                root <<= 1;
                rem <<= 2;
                rem += a >> 30;
                a <<= 2;

                if (root < rem)
                {
                    root++;
                    rem -= root;
                    root++;
                }
            }

            return root >> 1;
        }


        public static ulong SqrtLong(ulong a)
        {
            ulong rem = 0;
            ulong root = 0;
            uint i;

            for (i = 0; i < 32; i++)
            {
                root <<= 1;
                rem <<= 2;
                rem += a >> 62;
                a <<= 2;

                if (root < rem)
                {
                    root++;
                    rem -= root;
                    root++;
                }
            }

            return root >> 1;
        }

        public static int Sqrt(long a)
        {
            if (a <= 0L)
            {
                return 0;
            }

            if (a <= (long) (0xffffffffu))
            {
                return (int) FixMath.SqrtInt((uint) a);
            }

            return (int) FixMath.SqrtLong((ulong) a);
        }

        public static long DivLong(long a, long b)
        {
            // get first bit,negative or positive
            long bit = (long) ((ulong) ((a ^ b) & -9223372036854775808L) >> 63);
            long sign = bit * -2L + 1L;
            long test = (a + b / 2L * sign) / b;
            //  long tet2 = a / b;

            return test;
        }

        public static int DivInt(int a, int b)
        {
            // get first bit,negative or positive
            int bit = (int) ((uint) ((a ^ b) & -2147483648) >> 32);
            int sign = bit * -2 + 1;
            int test = (a + b / 2 * sign) / b;
            //  long tet2 = a / b;

            return test;
        }

        public static FixInt3 Divide(FixInt3 a, long m, long b)
        {
            a.x = (int) FixMath.DivLong((long) a.x * m, b);
            a.y = (int) FixMath.DivLong((long) a.y * m, b);
            a.z = (int) FixMath.DivLong((long) a.z * m, b);
            return a;
        }

        public static FixInt2 Divide(FixInt2 a, long m, long b)
        {
            a.x = (int) FixMath.DivLong((long) a.x * m, b);
            a.y = (int) FixMath.DivLong((long) a.y * m, b);
            return a;
        }

        public static FixInt3 Divide(FixInt3 a, int b)
        {
            a.x = FixMath.DivInt(a.x, b);
            a.y = FixMath.DivInt(a.y, b);
            a.z = FixMath.DivInt(a.z, b);
            return a;
        }

        public static FixInt3 Divide(FixInt3 a, long b)
        {
            a.x = (int) FixMath.DivLong((long) a.x, b);
            a.y = (int) FixMath.DivLong((long) a.y, b);
            a.z = (int) FixMath.DivLong((long) a.z, b);
            return a;
        }

        public static FixInt2 Divide(FixInt2 a, long b)
        {
            a.x = (int) FixMath.DivLong((long) a.x, b);
            a.y = (int) FixMath.DivLong((long) a.y, b);
            return a;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        public static int Sin()
        {
            return 1;
        }

        public static int Cos()
        {
            return 1;
        }


        //  inspire from https://github.com/riven8192/LibBase/blob/a70af645e9b35df824b9214b0ef2749bfb2b5df0/src/craterstudio/math/FastMath.java
        public static FixFraction Atan2(long y, long x)
        {
            int add, mul;

            if (x < 0)
            {
                if (y < 0)
                {
                    x = -x;
                    y = -y;

                    mul = 1;
                }
                else
                {
                    x = -x;
                    mul = -1;
                }

                add = -31416; // pi*TriangleLut.LUT_SCALE
            }
            else
            {
                if (y < 0)
                {
                    y = -y;
                    mul = -1;
                }
                else
                {
                    mul = 1;
                }

                add = 0;
            }

            long demoninal = (long) ((x < y) ? y : x);

            int xi = demoninal != 0?(int) FixMath.DivLong((long) (x * TriangleLut.ATAN2_DIM_MINUS_1), demoninal):0;
            int yi = demoninal != 0?(int) FixMath.DivLong((long) (y * TriangleLut.ATAN2_DIM_MINUS_1), demoninal):0;
            //int xi = (int)x * TriangleLut.ATAN2_DIM_MINUS_1/ demoninal;
            //int yi = (int)y * TriangleLut.ATAN2_DIM_MINUS_1/ demoninal;
            //return (TriangleLut.Atan2_LUT[yi * TriangleLut.ATAN2_DIM + xi] + add) * mul;

            return new FixFraction
            {
                nominal = (TriangleLut.Atan2_LUT[yi * TriangleLut.ATAN2_DIM + xi] + add) * mul,
                denominal = 10000L
            };
        }


        public static FixFraction Sin(long nom, long den)
        {
            long radFull = 31416 * 2L;

            int RadToIndex = (int) (TriangleLut.SIN_COUNT * TriangleLut.LUT_SCALE / radFull);

            int index = (int) (RadToIndex * nom / den);

            return new FixFraction(TriangleLut.Sin_LUT[index & TriangleLut.SIN_MASK], TriangleLut.LUT_SCALE);
        }

        public static int Sin(long radScale)
        {
            long radFull = 31416 * 2L;

            int RadToIndex = (int) (TriangleLut.SIN_COUNT * TriangleLut.LUT_SCALE / radFull);

            int index = (int) (RadToIndex * radScale / TriangleLut.LUT_SCALE);

            return TriangleLut.Sin_LUT[index & TriangleLut.SIN_MASK];
        }


        public static FixFraction Cos(long nom, long den)
        {
            long radFull = 31416 * 2L;

            int RadToIndex = (int) (TriangleLut.SIN_COUNT * TriangleLut.LUT_SCALE / radFull);

            int index = (int) (RadToIndex * nom / den);

            return new FixFraction(TriangleLut.Cos_LUT[index & TriangleLut.SIN_MASK], TriangleLut.LUT_SCALE);
        }


        public static int Cos(long radScale)
        {
            long radFull = 31416 * 2L;

            int RadToIndex = (int) (TriangleLut.SIN_COUNT * TriangleLut.LUT_SCALE / radFull);

            int index = (int) (RadToIndex * radScale / TriangleLut.LUT_SCALE);

            return TriangleLut.Cos_LUT[index & TriangleLut.SIN_MASK];
        }

        public static void Sincos(out FixFraction sin, out FixFraction cos, long nominal, long denominal)
        {
            long radFull = 31416 * 2L;

            int RadToIndex = (int) (TriangleLut.SIN_COUNT * TriangleLut.LUT_SCALE / radFull);

            int index = (int) (RadToIndex * nominal / denominal);

            sin = new FixFraction((long) TriangleLut.Sin_LUT[index], (long) TriangleLut.LUT_SCALE);
            cos = new FixFraction((long) TriangleLut.Cos_LUT[index], (long) TriangleLut.LUT_SCALE);
        }

        public static int Acos(long radScale)
        {
            long AcosIndex = FixMath.DivLong(radScale * TriangleLut.ACOS_HALF_COUNT, TriangleLut.LUT_SCALE) +
                             TriangleLut.ACOS_HALF_COUNT; // [-1,1] map to[0,1]


            if (AcosIndex < 0)
                AcosIndex = 0;
            else if (AcosIndex > TriangleLut.ACOS_COUNT)
                AcosIndex = TriangleLut.ACOS_COUNT;
            return TriangleLut.ACos_LUT[AcosIndex];
        }

        public static FixFraction Acos(long nom, long den)
        {
            int num = (int) FixMath.DivLong(nom * (long) TriangleLut.ACOS_HALF_COUNT, den) +
                      TriangleLut.ACOS_HALF_COUNT;
            num = Clamp(num, 0, TriangleLut.ACOS_COUNT);
            return new FixFraction
            {
                nominal = (long) TriangleLut.ACos_LUT[num],
                denominal = 10000L
            };
        }
    }
}