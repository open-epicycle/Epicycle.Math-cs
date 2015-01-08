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

namespace Epicycle.Math.Geometry.Differential
{
    public interface IManifold
    {
        int Dimension { get; }

        // translates point along tangent vector
        // throws if point doesn't belong to manifold
        // throws if tangentVector dimension is different from manifold dimension
        IManifoldPoint Translate(IManifoldPoint point, OVector tangentVector);

        // find translation vector from one point to another
        // throws if either point doesn't belong to manifold
        OVector GetTranslation(IManifoldPoint to, IManifoldPoint from);
    }

    public interface IManifoldPoint
    {
        int Dimension { get; }
    }
}
