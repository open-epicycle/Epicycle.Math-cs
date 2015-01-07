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
    public static class QuaternionUtils
    {
        public static Quaternion TimesI(this Quaternion @this)
        {
            return new Quaternion(-@this.B, @this.A, @this.D, -@this.C);
        }

        public static Quaternion ITimes(this Quaternion @this)
        {
            return new Quaternion(-@this.B, @this.A, -@this.D, @this.C);
        }

        public static Quaternion TimesJ(this Quaternion @this)
        {
            return new Quaternion(-@this.C, -@this.D, @this.A, @this.B);
        }

        public static Quaternion JTimes(this Quaternion @this)
        {
            return new Quaternion(-@this.C, @this.D, @this.A, -@this.B);
        }

        public static Quaternion TimesK(this Quaternion @this)
        {
            return new Quaternion(-@this.D, @this.C, -@this.B, @this.A);
        }

        public static Quaternion KTimes(this Quaternion @this)
        {
            return new Quaternion(-@this.D, -@this.C, @this.B, @this.A);
        }

        public static double ProductA(Quaternion q1, Quaternion q2)
        {
            return q1.A * q2.A - q1.B * q2.B - q1.C * q2.C - q1.D * q2.D;
        }

        public static double ProductB(Quaternion q1, Quaternion q2)
        {
            return q1.A * q2.B + q1.B * q2.A + q1.C * q2.D - q1.D * q2.C;
        }

        public static double ProductC(Quaternion q1, Quaternion q2)
        {
            return q1.A * q2.C - q1.B * q2.D + q1.C * q2.A + q1.D * q2.B;
        }

        public static double ProductD(Quaternion q1, Quaternion q2)
        {
            return q1.A * q2.D + q1.B * q2.C - q1.C * q2.B + q1.D * q2.A;
        }

        public static double ProductRe(Quaternion q1, Quaternion q2)
        {
            return ProductA(q1, q2);
        }

        public static Vector3 ProductIm(Quaternion q1, Quaternion q2)
        {
            return new Vector3(ProductB(q1, q2), ProductC(q1, q2), ProductD(q1, q2));
        }
    }
}
