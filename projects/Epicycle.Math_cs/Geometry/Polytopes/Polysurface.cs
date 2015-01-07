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

namespace Epicycle.Math.Geometry.Polytopes
{
    // UNDER CONSTRUCTION, DO NOT USE
    public sealed class Polysurface : IPolysurface
    {
        internal Polysurface(IPartialPolysurface surface)
        {
            // must be copied to protect state integrity
            _vertices = surface.Vertices.ToList();
            _edges = surface.Edges.ToList();
            _faces = surface.Faces.ToList();

            // TODO: Combinatorical integrity test!!!
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<IPolysurfaceVertex> Vertices
        {
            get { return _vertices; }
        }

        public IReadOnlyCollection<IPolysurfaceEdge> Edges
        {
            get { return _edges; }
        }

        public IReadOnlyCollection<IPolysurfaceFace> Faces
        {
            get { return _faces; }
        }

        private readonly IReadOnlyCollection<IPolysurfaceVertex> _vertices;
        private readonly IReadOnlyCollection<IPolysurfaceEdge> _edges;
        private readonly IReadOnlyCollection<IPolysurfaceFace> _faces;

        public IPolysurfaceEdge GetEdge(IPolysurfaceVertex source, IPolysurfaceVertex target)
        {
            throw new NotImplementedException();
        }


        public IEqualityComparer<IPolysurfaceEdge> UndirectedEdgeComparer
        {
            get { throw new NotImplementedException(); }
        }
    }
}
