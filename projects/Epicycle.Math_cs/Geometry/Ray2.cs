using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry
{
    public sealed class Ray2
    {
        public Ray2(Vector2 origin, Vector2 direction)
        {
            _origin = origin;
            _direction = direction.Normalized;
        }

        private readonly Vector2 _origin;
        private readonly Vector2 _direction;

        public Vector2 Origin
        {
            get { return _origin; }
        }

        public Vector2 Direction
        {
            get { return _direction; }
        }

        public Vector2 PointAt(double distance)
        {
            return Origin + distance * Direction;
        }
    }
}
