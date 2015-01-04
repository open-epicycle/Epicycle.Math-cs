using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry.Polytopes
{
    public interface IOpenPolylineBuilder : IOpenPolyline
    {
        void Add(Vector2 vertex);

        void Join(IOpenPolyline line);
    }
}
