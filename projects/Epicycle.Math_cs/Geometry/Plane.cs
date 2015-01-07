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

namespace Epicycle.Math.Geometry
{

    // oriented plane in 3D Euclidean space
    public struct Plane
    {
        public Plane(double freeTerm, Vector3 linearTerms)
        {
            var norm = linearTerms.Norm;

            _normal = linearTerms / norm;
            _freeTerm = freeTerm / norm;
        }

        public Plane(Vector3 point, Vector3 normal)
        {
            _normal = normal.Normalized;
            _freeTerm = -point * _normal;
        }

        private readonly Vector3 _normal;
        private readonly double _freeTerm;

        public Vector3 Normal
        {
            get { return _normal; }
        }

        public double FreeTerm
        {
            get { return _freeTerm; }
        }

        public Plane Reverse
        {
            get { return new Plane(-FreeTerm, -Normal); }
        }

        public static Plane Parametric(Vector3 a, Vector3 u, Vector3 v)
        {
            return new Plane(a, u.Cross(v));
        }

        public static Plane Through(Vector3 a, Vector3 b, Vector3 c)
        {
            return Parametric(a, b - a, c - a);
        }

        public static Plane ParallelTo(Plane plane, double signedDistance)
        {
            return new Plane(plane.FreeTerm - signedDistance, plane.Normal);
        }
    }
}
