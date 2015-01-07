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

using NUnit.Framework;

namespace Epicycle.Math.Geometry
{
    [TestFixture]
    public sealed class QuaternionTest : AssertionHelper
    {
        private double _tolerance = 1e-10;

        private void ExpectAlmostEqual(Quaternion x, Quaternion y)
        {
            Expect(x.A, Is.EqualTo(y.A).Within(_tolerance));
            Expect(x.B, Is.EqualTo(y.B).Within(_tolerance));
            Expect(x.C, Is.EqualTo(y.C).Within(_tolerance));
            Expect(x.D, Is.EqualTo(y.D).Within(_tolerance));
        }

        private void ExpectProduct(Quaternion x, Quaternion y, Quaternion expectedProduct)
        {
            ExpectAlmostEqual(x * y, expectedProduct);
        }

        [Test]
        public void Product_of_basis_quaternions_is_correct()
        {
            ExpectProduct(Quaternion.One, Quaternion.One, Quaternion.One);

            ExpectProduct(Quaternion.One, Quaternion.I, Quaternion.I);
            ExpectProduct(Quaternion.I, Quaternion.One, Quaternion.I);
            ExpectProduct(Quaternion.One, Quaternion.J, Quaternion.J);
            ExpectProduct(Quaternion.J, Quaternion.One, Quaternion.J);
            ExpectProduct(Quaternion.One, Quaternion.K, Quaternion.K);
            ExpectProduct(Quaternion.K, Quaternion.One, Quaternion.K);

            ExpectProduct(Quaternion.I, Quaternion.I, -Quaternion.One);
            ExpectProduct(Quaternion.J, Quaternion.J, -Quaternion.One);
            ExpectProduct(Quaternion.K, Quaternion.K, -Quaternion.One);

            ExpectProduct(Quaternion.I, Quaternion.J, Quaternion.K);
            ExpectProduct(Quaternion.J, Quaternion.I, -Quaternion.K);
            ExpectProduct(Quaternion.J, Quaternion.K, Quaternion.I);
            ExpectProduct(Quaternion.K, Quaternion.J, -Quaternion.I);
            ExpectProduct(Quaternion.K, Quaternion.I, Quaternion.J);
            ExpectProduct(Quaternion.I, Quaternion.K, -Quaternion.J);
        }

        [Test]
        public void Product_of_quaternions_is_consistent_with_Vector3_algebra()
        {
            var x = new Quaternion(-0.1, 0.25, 0.52, -0.7);
            var y = new Quaternion(0.89, 1.1, -2.56, 3.14);

            var actualProduct = x * y;

            var expectedProduct = new Quaternion(
                real: x.Re * y.Re - x.Im * y.Im,
                imag: x.Re * y.Im + y.Re * x.Im + x.Im.Cross(y.Im));

            ExpectAlmostEqual(actualProduct, expectedProduct);
        }

        [Test]
        public void Methods_for_multiplication_by_basis_quanternions_are_consistent_with_product_operator()
        {
            var x = new Quaternion(-0.1, 0.25, 0.52, -0.7);

            ExpectAlmostEqual(x.TimesI(), x * Quaternion.I);
            ExpectAlmostEqual(x.TimesJ(), x * Quaternion.J);
            ExpectAlmostEqual(x.TimesK(), x * Quaternion.K);

            ExpectAlmostEqual(x.ITimes(), Quaternion.I * x);
            ExpectAlmostEqual(x.JTimes(), Quaternion.J * x);
            ExpectAlmostEqual(x.KTimes(), Quaternion.K * x);
        }

        [Test]
        public void Methods_for_individual_components_of_quaternion_product_are_consistent_with_product_operator()
        {
            var x = new Quaternion(-0.1, 0.25, 0.52, -0.7);
            var y = new Quaternion(0.89, 1.1, -2.56, 3.14);

            var expected = x * y;
            var actual = new Quaternion(QuaternionUtils.ProductA(x, y), QuaternionUtils.ProductB(x, y), QuaternionUtils.ProductC(x, y), QuaternionUtils.ProductD(x, y));

            ExpectAlmostEqual(actual, expected);
        }

        [Test]
        public void Product_between_Quaternion_and_Vector3_is_consistent_with_product_between_two_quaternions()
        {
            var x = new Quaternion(-0.1, 0.25, 0.52, -0.7);
            var y = new Vector3(1.1, -2.56, 3.14);

            var expected = x * (Quaternion)y;
            var actual = x * y;

            ExpectAlmostEqual(actual, expected);

            expected = (Quaternion)y * x;
            actual = y * x;

            ExpectAlmostEqual(actual, expected);
        }
    }
}
