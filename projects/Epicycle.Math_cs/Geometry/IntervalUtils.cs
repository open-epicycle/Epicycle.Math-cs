using Epicycle.Commons;

namespace Epicycle.Math.Geometry
{
    public static class IntervalUtils
    {
        public static double Distance2(this Interval @this, double value)
        {
            var high = value > @this.Min;
            var low = value < @this.Max;

            if (low && high)
            {
                return 0;
            }
            else
            {
                if (low)
                {
                    return BasicMath.Sqr(value - @this.Min);
                }
                else
                {
                    return BasicMath.Sqr(value - @this.Max);
                }
            }
        }

        public sealed class YamlSerialization
        {
            public double Min { get; set; }
            public double Max { get; set; }

            public YamlSerialization() { }

            public YamlSerialization(Interval v)
            {
                Min = v.Min;
                Max = v.Max;
            }

            public Interval Deserialize()
            {
                return new Interval(Min, Max);
            }
        }

        public static double ToIntervalCoord(this double @this, Interval interval)
        {
            return (@this - interval.Min) / interval.Length;
        }
    }
}
