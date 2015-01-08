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

using Epicycle.Math.LinearAlgebra;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.Geometry
{
    using System;

    public static class Vector3Utils
    {
        public static double Angle(Vector3 a, Vector3 b, Vector3 c)
        {
            return Vector3.Angle(a - b, c - b);
        }

        // returns arbitrary vector orthogonal to given
        // guaranteed to return non-zero vector for non-zero input (actually norm cannot decrease by more than 20%)
        public static Vector3 VectorOrthogonalTo(Vector3 v)
        {
            var absX = Math.Abs(v.X);
            var absY = Math.Abs(v.Y);
            var absZ = Math.Abs(v.Z);

            var xLessThanY = absX < absY;
            var xLessThanZ = absX < absZ;

            if (xLessThanY && xLessThanZ)
            {
                return new Vector3(0, v.Z, -v.Y);
            }
            else
            {
                var yLessThanZ = absY < absZ;

                if (yLessThanZ && !xLessThanY)
                {
                    return new Vector3(v.Z, 0, -v.X);
                }
                else
                {
                    return new Vector3(v.Y, -v.X, 0);
                }
            }
        }

        // v.DualMatrix() * w = v.Cross(w)
        public static OSquareMatrix DualMatrix(this Vector3 v)
        {
            return new SquareMatrix(new double[,] 
            {
                { 0,  -v.Z, v.Y},
                { v.Z, 0,  -v.X},
                {-v.Y, v.X, 0  }
            });
        }

        // finds least-squares solution to a system of orthogonality equations (i.e. result is fitted to be orthogonal to vectors)
        public static void FitOrthogonal(IEnumerable<Vector3> vectors, out Vector3 result, out double residual)
        {
            var mat = SymmetricMatrix3.TensorSquare(vectors.First());

            foreach (var vector in vectors.Skip(1))
            {
                mat += SymmetricMatrix3.TensorSquare(vector);
            }

            mat.ComputeLowestEigendata(out residual, out result);
        }

        public static Vector3 FitOrthogonal(IEnumerable<Vector3> vectors)
        {
            double residual;
            Vector3 answer;

            FitOrthogonal(vectors, out answer, out residual);

            return answer;
        }

        public sealed class YamlSerialization
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }

            public YamlSerialization() { }

            public YamlSerialization(Vector3 v)
            {
                X = v.X;
                Y = v.Y;
                Z = v.Z;
            }

            public Vector3 Deserialize()
            {
                return new Vector3(X, Y, Z);
            }
        }
    }
}
