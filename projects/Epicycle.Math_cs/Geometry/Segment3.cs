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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Epicycle.Commons;

namespace Epicycle.Math.Geometry
{
    public struct Segment3
    {
        public Segment3(Vector3 start, Vector3 end)
        {
            ArgAssert.NotNull(start, "start");
            ArgAssert.NotNull(end, "end");

            _start = start;
            _end = end;
        }

        private readonly Vector3 _start;
        private readonly Vector3 _end;

        public Vector3 Start
        {
            get { return _start; }
        }

        public Vector3 End
        {
            get { return _end; }
        }

        public double Length2
        {
            get { return Vector3.Distance2(Start, End); }
        }

        public double Length
        {
            get { return Vector3.Distance(Start, End); }
        }

        public Vector3 Vector
        {
            get { return End - Start; }
        }

        public Vector3 PointAt(double t)
        {
            return t * End + (1 - t) * Start;
        }
    }
}
