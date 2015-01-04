using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
