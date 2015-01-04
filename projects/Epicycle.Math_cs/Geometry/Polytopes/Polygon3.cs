using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry.Polytopes
{
    public sealed class Polygon3 : IPolygon3
    {
        public Polygon3(RotoTranslation3 coordinateSystem, IPolygon polygon)
        {
            _coordinateSystem = coordinateSystem;
            _polygon = polygon;
        }

        private readonly RotoTranslation3 _coordinateSystem;
        private readonly IPolygon _polygon;

        public RotoTranslation3 CoordinateSystem
        {
            get { return _coordinateSystem; }
        }

        public IPolygon InPlane
        {
            get { return _polygon; }
        }

        public bool IsEmpty 
        {
            get { return _polygon.IsEmpty; }
        }

        public static Polygon3 Empty
        {
            get { return new Polygon3(RotoTranslation3.Id, Polygon.Empty); }
        }
    }
}
