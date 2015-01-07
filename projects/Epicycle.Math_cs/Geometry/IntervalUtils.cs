// [[[[INFO>
// Copyright 2015 Epicycle (http://epicycle.org, https://github.com/open-epicycle)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// For more information check https://github.com/open-epicycle/Epicycle.Math-cs
// ]]]]

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
