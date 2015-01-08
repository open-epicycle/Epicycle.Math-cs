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

using System.Drawing;

namespace Epicycle.Math.Geometry.Windows
{
    using System;

    public static class Vector2iUtils
    {
        public static Vector2i ToVector2i(this Point p)
        {
            return new Vector2i(p.X, p.Y);
        }

        public static Vector2i ToVector2i(this PointF p)
        {
            return new Vector2i((int)Math.Round(p.X), (int)Math.Round(p.Y));
        }

        public static Point ToDrawingPoint(this Vector2i v)
        {
            return new Point(v.X, v.Y);
        }

        public static PointF ToDrawingPointF(this Vector2i v)
        {
            return new PointF(v.X, v.Y);
        }
    }
}
