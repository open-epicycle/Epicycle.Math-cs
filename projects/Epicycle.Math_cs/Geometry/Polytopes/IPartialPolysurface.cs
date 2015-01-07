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

namespace Epicycle.Math.Geometry.Polytopes
{
    // consists of spatial vertices, edges and faces like a polyhedral surface but doesn't have to be complete, e.g. can contain disconnected vertices
    public interface IPartialPolysurface
    {
        IReadOnlyCollection<IPolysurfaceVertex> Vertices { get; }

        IReadOnlyCollection<IPolysurfaceEdge> Edges { get; }

        IReadOnlyCollection<IPolysurfaceFace> Faces { get; }

        // returns null if vertices coincide or are not connected
        // throws if vertices don't belong to given surface
        IPolysurfaceEdge GetEdge(IPolysurfaceVertex source, IPolysurfaceVertex target);

        IEqualityComparer<IPolysurfaceEdge> UndirectedEdgeComparer { get; }
    }

    // Morally, these 3 interfaces should be nested within IPartialPolysurface. Unfortunatelly C# doesn't allow it (as opposed to VB.NET)

    public interface IPolysurfaceVertex : IEquatable<IPolysurfaceVertex>
    {
        Vector3 Point { get; }

        // number of attached edges
        int Order { get; }

        // edges are returned as outgoing from vertex, in counterclockwise order w.r.t. surface orientation
        // for a boundary vertex belonging to an IPolysurface, the first and last edges are boundary
        // each vertex is attached to at least 3 edges
        IEnumerable<IPolysurfaceEdge> Edges { get; }
    }

    public interface IPolysurfaceEdge : IEquatable<IPolysurfaceEdge>
    {
        IPolysurfaceVertex Source { get; }
        IPolysurfaceVertex Target { get; }

        // one of the faces can be null but not both
        // right/left is when standing on positive side of surface and looking at edge direction
        IPolysurfaceFace RightFace { get; }
        IPolysurfaceFace LeftFace { get; }

        IPolysurfaceEdge Reversed { get; }
    }

    public interface IPolysurfaceFace : IEquatable<IPolysurfaceFace>
    {
        // polygon is guaranteed to be simple
        // polygon orientation is the same as face orientation
        // vertex indices in polygon are *not* guaranteed to correspond to order of Edges
        IPolygon3 Polygon { get; }

        Vector3 Normal { get; }
        
        int Order { get; }

        // edges are returned oriented counterclockwise, in counterclockwise order w.r.t. surface orientation
        // each face has at least 3 edges
        IEnumerable<IPolysurfaceEdge> Edges { get; }
    }
}
