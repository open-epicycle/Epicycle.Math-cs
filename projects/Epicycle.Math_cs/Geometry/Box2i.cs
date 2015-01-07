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

namespace Epicycle.Math.Geometry
{
    using System;

    // a Box2i is a rectangular collection of square tiles with unit side centered on integer coordinate points
    public struct Box2i
    {
        #region creation and conversion

        public Box2i(Vector2i minCorner, Vector2i dimensions) 
            : this(minCorner.X, minCorner.Y, dimensions.X, dimensions.Y) { }

        public Box2i(int minX, int minY, int width, int height)
        {
            _minCorner = new Vector2i(minX, minY);
            _maxCorner = new Vector2i(minX + width - 1, minY + height - 1);
        }

        private readonly Vector2i _minCorner;
        private readonly Vector2i _maxCorner;        

        public static Box2i Hull(Vector2i v, Vector2i w)
        {
            var minCorner = new Vector2i(Math.Min(v.X, w.X), Math.Min(v.Y, w.Y));
            var maxCorner = new Vector2i(Math.Max(v.X, w.X), Math.Max(v.Y, w.Y));

            return new Box2i(minCorner, maxCorner, true);
        }

        // dummy argument is needed to distinguish from a ctor with the same signature
        private Box2i(Vector2i minCorner, Vector2i maxCorner, bool dummy)
        {
            _minCorner = minCorner;
            _maxCorner = maxCorner;
        }

        public static implicit operator Box2(Box2i box)
        {
            return new Box2(box.MinX - 0.5, box.MinY - 0.5, box.Width, box.Height);
        }

        #endregion

        #region properties

        // center of tile with minimal coordinates
        public Vector2i MinCorner 
        {
            get { return _minCorner; }
        }

        // center of tile with maximal coordinates
        public Vector2i MaxCorner 
        {
            get { return _maxCorner; }
        }

        public int MinX
        {
            get { return _minCorner.X; }
        }

        public int MaxX
        {
            get { return _maxCorner.X; }
        }

        public int MinY
        {
            get { return _minCorner.Y; }
        }

        public int MaxY
        {
            get { return _maxCorner.Y; }
        }

        public Vector2 Center
        {
            get { return new Vector2((_maxCorner.X - _minCorner.X) / 2d, (_maxCorner.Y - _minCorner.Y) / 2d); }
        }

        public int Width 
        {
            get { return MaxCorner.X - MinCorner.X + 1; }
        }

        public int Height
        {
            get { return MaxCorner.Y - MinCorner.Y + 1; }
        }

        public Vector2i Dimensions
        {
            get { return new Vector2i(Width, Height); }
        }

        public int Area
        {
            get { return Width * Height; }
        }

        #endregion

        #region emptiness

        public bool IsEmpty
        {
            get { return MaxCorner.X < MinCorner.X || MaxCorner.Y < MinCorner.Y; }
        }

        public static Box2i Empty
        {
            get { return _emptyBox; }
        }

        private static readonly Box2i _emptyBox;

        static Box2i()
        {
            _emptyBox = new Box2i(minX: 0, minY: 0, width: -1, height: -1);
        }

        #endregion

        #region methods

        public bool Contains(Box2i box)
        {
            if(box.IsEmpty)
            {
                return true;
            }

            return
                MinCorner.X <= box.MinCorner.X &&
                MinCorner.Y <= box.MinCorner.Y &&
                MaxCorner.X >= box.MaxCorner.X &&
                MaxCorner.Y >= box.MaxCorner.Y;
        }

        public static Box2i Intersection(Box2i a, Box2i b)
        {
            var minCorner = new Vector2i(Math.Max(a.MinCorner.X, b.MinCorner.X), Math.Max(a.MinCorner.Y, b.MinCorner.Y));
            var maxCorner = new Vector2i(Math.Min(a.MaxCorner.X, b.MaxCorner.X), Math.Min(a.MaxCorner.Y, b.MaxCorner.Y));

            return new Box2i(minCorner, maxCorner, dummy: true);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", MinCorner.X, MinCorner.Y, Width, Height);
        }

        #endregion
    }
}
