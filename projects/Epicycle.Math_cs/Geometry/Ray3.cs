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
    using System;

    public struct Ray3
    {
        public Ray3(Vector3 origin, Vector3 direction)
        {
            _origin = origin;
            _direction = direction.Normalized;
        }

        private readonly Vector3 _origin;
        private readonly Vector3 _direction;

        public Vector3 Origin
        {
            get { return _origin; }
        }

        public Vector3 Direction
        {
            get { return _direction; }
        }

        public Vector3 PointAt(double distance)
        {
            return Origin + distance * Direction;
        }
    }
}
