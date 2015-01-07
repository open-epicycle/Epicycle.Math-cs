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

using Epicycle.Commons;

namespace Epicycle.Math.Geometry
{
    using System;

    public static class Rotation2Order4Utils
    {
        public static Rotation2Order4 Inv(this Rotation2Order4 @this)
        {
            switch (@this)
            {
            case Rotation2Order4.Rotation0:
                return Rotation2Order4.Rotation0;

            case Rotation2Order4.Rotation90:
                return Rotation2Order4.Rotation270;

            case Rotation2Order4.Rotation180:
                return Rotation2Order4.Rotation180;

            case Rotation2Order4.Rotation270:
                return Rotation2Order4.Rotation90;

            default:
                throw new InternalException("Illegal rotation!");
            }
        }

        public static Rotation2Order4 Compose(Rotation2Order4 x, Rotation2Order4 y)
        {
            return (Rotation2Order4)(((int)x + (int)y) % 4);
        }

        public static Vector2 Apply(this Rotation2Order4 @this, Vector2 v)
        {
            switch (@this)
            {
                case Rotation2Order4.Rotation0:
                    return v;

                case Rotation2Order4.Rotation90:
                    return new Vector2(-v.Y, v.X);

                case Rotation2Order4.Rotation180:
                    return -v;

                case Rotation2Order4.Rotation270:
                    return new Vector2(v.Y, -v.X);

                default:
                    throw new InternalException("Illegal rotation!");
            }
        }

        public static Vector2 ApplyInv(this Rotation2Order4 @this, Vector2 v)
        {
            switch (@this)
            {
                case Rotation2Order4.Rotation0:
                    return v;

                case Rotation2Order4.Rotation90:
                    return new Vector2(v.Y, -v.X);

                case Rotation2Order4.Rotation180:
                    return -v;

                case Rotation2Order4.Rotation270:
                    return new Vector2(-v.Y, v.X);

                default:
                    throw new InternalException("Illegal rotation!");
            }
        }

        public static double Angle(this Rotation2Order4 @this)
        {
            switch (@this)
            {
                case Rotation2Order4.Rotation0:
                    return 0;

                case Rotation2Order4.Rotation90:
                    return Math.PI / 2;

                case Rotation2Order4.Rotation180:
                    return Math.PI;

                case Rotation2Order4.Rotation270:
                    return -Math.PI / 2;

                default:
                    throw new InternalException("Illegal rotation!");
            }
        }
    }
}
