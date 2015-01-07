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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry
{

using System;

public struct Interval
{
    public double Min
    {
        get { return _min; }
    }

    public double Max
    {
        get { return _max; }
    }

    public double Center
    {
        get { return (_min + _max) / 2; }
    }

    private readonly double _min;
    private readonly double _max;

    public Interval(double min, double max)
    {
        if (min > max)
        {
            _min = double.PositiveInfinity;
            _max = double.NegativeInfinity;
        }
        else
        {
            _min = min;
            _max = max;
        }
    }

    public enum EndType
    {
        Min = 0,
        Max = 1,
        Count = 2 // used in for loops
    }

    public double End(EndType end)
    {
        switch(end)
        {
            case EndType.Min:
                return Min;
            
            case EndType.Max:
                return Max;

            default:
                throw new ArgumentOutOfRangeException("end", end, "Illegal EndType " + end.ToString());
        }
    }

    public double Length
    {
        get 
        {
            if (this.IsEmpty)
            {
                return 0;
            }
 
            return Max - Min;
        }
    }

    public bool IsEmpty
    {
        get { return Min > Max; }
    }

    public static Interval Empty
    {
        get { return _emptyInterval; }
    }

    private static readonly Interval _emptyInterval;

    public static Interval EntireLine
    {
        get { return _entireLine; }
    }

    private static readonly Interval _entireLine;

    static Interval()
    {
        _emptyInterval = new Interval(double.PositiveInfinity, double.NegativeInfinity);
        _entireLine = new Interval(double.NegativeInfinity, double.PositiveInfinity);
    }

    public bool Contains(Interval t)
    {
        return Min <= t.Min && Max >= t.Max;
    }

    public bool Contains(double x)
    {
        return x >= Min && x <= Max;
    }

    public static Interval Hull(double x, double y)
    {
        if (x < y)
        {
            return new Interval(x, y);
        }
        else
        {
            return new Interval(y, x);
        }
    }

    public static Interval Hull(Interval a, Interval b)
    {
        return new Interval(Math.Min(a.Min, b.Min), Math.Max(a.Max, b.Max));
    }

    public static Interval Hull(IEnumerable<double> values)
    {
        var min = double.PositiveInfinity;
        var max = double.NegativeInfinity;

        foreach (var value in values)
        {
            min = Math.Min(min, value);
            max = Math.Max(max, value);
        }

        return new Interval(min, max);
    }

    public static Interval Intersection(Interval a, Interval b)
    {
        return new Interval(Math.Max(a.Min, b.Min), Math.Min(a.Max, b.Max));
    }

    public static bool DoIntersect(Interval a, Interval b)
    {
        return a.Max > b.Min && b.Max > a.Min;
    }

    public override string ToString()
    {
        return string.Format("({0}, {1})", Min, Max);
    }
}

}
