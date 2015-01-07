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

using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Geometry.Differential
{
    public sealed class Rotation3Manifold : IManifold
    {
        public int Dimension
        {
            get { return 3; }
        }

        public IManifoldPoint Translate(IManifoldPoint point, OVector tangentVector)
        {
            return (Rotation3)point * new Rotation3((Vector3)tangentVector);
        }

        public OVector GetTranslation(IManifoldPoint to, IManifoldPoint from)
        {
            return (((Rotation3)from).Inv * (Rotation3)to).Vector;
        }
    }
}
