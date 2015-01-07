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

using NUnit.Framework;

namespace Epicycle.Math.Geometry
{
    public sealed class Vector2Test : AssertionHelper
    {
        [Test]
        public void Ccw_returns_true_on_ccw_Vector2_triples()
        {
            var u = new Vector2(1, 0);
            var v = new Vector2(0, 1);
            var w = new Vector2(-1, -1);

            Expect(Vector2Utils.AreCcw(u, v, w));

            u = new Vector2(0.1, 1);
            v = new Vector2(0, 1);
            w = new Vector2(-0.1, 1);

            Expect(Vector2Utils.AreCcw(u, v, w));
        }

        [Test]
        public void Ccw_returns_false_on_cw_Vector2_triples()
        {
            var u = new Vector2(1, 0);
            var v = new Vector2(0, 1);
            var w = new Vector2(-1, -1);

            Expect(!Vector2Utils.AreCcw(v, u, w));

            u = new Vector2(0.1, 1);
            v = new Vector2(0, 1);
            w = new Vector2(-0.1, 1);

            Expect(!Vector2Utils.AreCcw(w, v, u));
        }
    }
}
