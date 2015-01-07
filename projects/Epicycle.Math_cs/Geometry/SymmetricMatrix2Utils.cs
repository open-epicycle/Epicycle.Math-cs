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

using Epicycle.Commons;

namespace Epicycle.Math.Geometry
{
    using System;

    public static class SymmetricMatrix2Utils
    {
        public static Vector2 RowX(this SymmetricMatrix2 @this)
        {
            return new Vector2(@this.XX, @this.XY);
        }

        public static Vector2 RowY(this SymmetricMatrix2 @this)
        {
            return new Vector2(@this.XY, @this.YY);
        }

        public static double Trace(this SymmetricMatrix2 @this)
        {
            return @this.XX + @this.YY;
        }

        public static double Det(this SymmetricMatrix2 @this)
        {
            return @this.XX * @this.YY - @this.XY * @this.XY;
        }

        public static double LowEigenvalue(this SymmetricMatrix2 @this)
        {
            var halfTrace = @this.Trace() / 2;

            return halfTrace - BasicMath.Sqrt(halfTrace * halfTrace - @this.Det());
        }

        public static double HighEigenvalue(this SymmetricMatrix2 @this)
        {
            var halfTrace = @this.Trace() / 2;

            return halfTrace + BasicMath.Sqrt(halfTrace * halfTrace - @this.Det());
        }

        public static void ComputeEigenvalues(this SymmetricMatrix2 @this, out double low, out double high)
        {
            var halfTrace = @this.Trace() / 2;
            var disc = BasicMath.Sqrt(halfTrace * halfTrace - @this.Det());

            low = halfTrace - disc;
            high = halfTrace + disc;
        }

        private static Vector2 Eigenvector(this SymmetricMatrix2 @this, double lambda)
        {
            var rowX = new Vector2(@this.XX - lambda, @this.XY);
            var rowY = new Vector2(@this.XY, @this.YY - lambda);

            var normX2 = rowX.Norm2;
            var normY2 = rowY.Norm2;

            if (normX2 > normY2)
            {
                if (normX2 > BasicMath.Epsilon)
                {
                    return rowX.Rotate90() / Math.Sqrt(normX2);
                }
                else
                {
                    return Vector2.UnitX;
                }
            }
            else
            {
                if (normY2 > BasicMath.Epsilon)
                {
                    return rowY.Rotate90() / Math.Sqrt(normY2);
                }
                else
                {
                    return Vector2.UnitX;
                }
            }
        }

        public static Vector2 LowEigenvector(this SymmetricMatrix2 @this)
        {
            return @this.Eigenvector(@this.LowEigenvalue());
        }

        public static Vector2 HighEigenvector(this SymmetricMatrix2 @this)
        {
            return @this.Eigenvector(@this.HighEigenvalue());
        }

        public static void ComputeEigenvectors(this SymmetricMatrix2 @this, out Vector2 low, out Vector2 high)
        {
            low = @this.LowEigenvector();
            high = low.Rotate90();
        }

        public static void ComputeLowEigendata(this SymmetricMatrix2 @this, out double eigenvalue, out Vector2 eigenvector)
        {
            eigenvalue = @this.LowEigenvalue();
            eigenvector = @this.Eigenvector(eigenvalue);
        }

        public static void ComputeHighEigendata(this SymmetricMatrix2 @this, out double eigenvalue, out Vector2 eigenvector)
        {
            eigenvalue = @this.HighEigenvalue();
            eigenvector = @this.Eigenvector(eigenvalue);
        }

        public static void ComputeEigendata(this SymmetricMatrix2 @this, out double lowEvalue, out double highEvalue, out Vector2 lowEvector)
        {
            @this.ComputeEigenvalues(out lowEvalue, out highEvalue);
            lowEvector = @this.Eigenvector(lowEvalue);
        }        
    }
}
