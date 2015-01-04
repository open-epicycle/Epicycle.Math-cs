namespace Epicycle.Math.Geometry
{
    using System;

    public struct Ray3
    {
        public Ray3(Vector3 origin, Vector3 direction)
        {
            _origin = origin;
            _direction = direction.Normalized;
        }

        private readonly Vector3 _origin;
        private readonly Vector3 _direction;

        public Vector3 Origin
        {
            get { return _origin; }
        }

        public Vector3 Direction
        {
            get { return _direction; }
        }

        public Vector3 PointAt(double distance)
        {
            return Origin + distance * Direction;
        }
    }
}
