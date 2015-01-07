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

namespace Epicycle.Math.Geometry
{
    public struct RotoTranslation3
    {
        public RotoTranslation3(Rotation3 rotation, Vector3 translation)
        {
            _rotation = rotation;
            _translation = translation;
        }

        public RotoTranslation3(Rotation3 rotation) : this(rotation, Vector3.Zero) { }

        public RotoTranslation3(Vector3 translation) : this(Rotation3.Id, translation) { }

        public static implicit operator RotoTranslation3(Rotation3 rotation)
        {
            return new RotoTranslation3(rotation);
        }

        // explicit since although conversion doesn't loose information it is not perfectly intuitive
        public static explicit operator RotoTranslation3(Vector3 translation)
        {
            return new RotoTranslation3(translation);
        }

        private readonly Rotation3 _rotation;
        private readonly Vector3 _translation;

        public static readonly RotoTranslation3 Id = new RotoTranslation3(Rotation3.Id, Vector3.Zero);

        public Rotation3 Rotation
        {
            get { return _rotation; }
        }

        public Vector3 Translation
        {
            get { return _translation; }
        }

        public RotoTranslation3 Inv
        {
            get { return new RotoTranslation3(Rotation.Inv, -Rotation.ApplyInv(Translation)); }
        }

        public Vector3 Apply(Vector3 point)
        {
            return Rotation.Apply(point) + Translation;
        }

        public Vector3 ApplyInv(Vector3 point)
        {
            return Rotation.ApplyInv(point - Translation);
        }

        public static RotoTranslation3 operator *(RotoTranslation3 x, RotoTranslation3 y)
        {
            return new RotoTranslation3(x.Rotation * y.Rotation, x.Apply(y.Translation));
        }
    }
}
