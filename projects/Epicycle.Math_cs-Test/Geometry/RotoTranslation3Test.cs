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
    public sealed class RotoTranslation3Test : AssertionHelper
    {
        private const double _tolerance = 1e-10;

        [Test]
        public void Applying_the_product_of_two_RotoTranslation3_is_equivalent_to_applying_them_in_succession()
        {
            var x = new RotoTranslation3(new Rotation3(new Quaternion(1.1, -2.2, 3.3, -4.4)), new Vector3(4.1, -2.4, 1.3));
            var y = new RotoTranslation3(new Rotation3(new Quaternion(-0.1, -1.2, -5.3, -3.4)), new Vector3(2.1, -1.4, -0.3));

            var v = new Vector3(3.14, 2.88, -1.79);

            var expected = x.Apply(y.Apply(v));
            var actual = (x * y).Apply(v);

            Expect(Vector3.Distance(actual, expected), Is.LessThan(_tolerance));
        }

        [Test]
        public void Applying_a_RotoTranslation3_followed_by_applying_its_precomputed_Inv_results_in_the_original_vector()
        {
            var x = new RotoTranslation3(new Rotation3(new Quaternion(1.1, -2.2, 3.3, -4.4)), new Vector3(4.1, -2.4, 1.3));

            var v = new Vector3(3.14, 2.88, -1.79);

            var roundTrip = x.Inv.Apply(x.Apply(v));

            Expect(Vector3.Distance(roundTrip, v), Is.LessThan(_tolerance));
        }

        [Test]
        public void ApplyToPlaneXY_yields_a_Plane_passing_through_images_of_points_on_plane_XY_with_correct_orientation()
        {
            var x = new RotoTranslation3(new Rotation3(new Quaternion(1.1, -2.2, 3.3, -4.4)), new Vector3(4.1, -2.4, 1.3));

            var plane = x.ApplyToPlaneXY();

            Expect(x.Apply(Vector3.Zero).DistanceTo(plane), Is.LessThan(_tolerance));
            Expect(x.Apply(Vector3.UnitX).DistanceTo(plane), Is.LessThan(_tolerance));
            Expect(x.Apply(Vector3.UnitY).DistanceTo(plane), Is.LessThan(_tolerance));

            Expect(x.Apply(Vector3.UnitZ).SignedDistanceTo(plane), Is.Positive); // orientation test
        }

        [Test]
        public void ApplyInvToPlaneXY_yields_a_Plane_passing_through_inverse_images_of_points_on_plane_XY_with_correct_orientation()
        {
            var x = new RotoTranslation3(new Rotation3(new Quaternion(1.1, -2.2, 3.3, -4.4)), new Vector3(4.1, -2.4, 1.3));

            var plane = x.ApplyInvToPlaneXY();

            Expect(x.ApplyInv(Vector3.Zero).DistanceTo(plane), Is.LessThan(_tolerance));
            Expect(x.ApplyInv(Vector3.UnitX).DistanceTo(plane), Is.LessThan(_tolerance));
            Expect(x.ApplyInv(Vector3.UnitY).DistanceTo(plane), Is.LessThan(_tolerance));

            Expect(x.ApplyInv(Vector3.UnitZ).SignedDistanceTo(plane), Is.Positive); // orientation test
        }

        [Test]
        public void Apply_to_Plane_given_in_parameteric_form_yields_a_Plane_passing_through_images_of_points_on_original_Plane_with_correct_orientation()
        {
            var x = new RotoTranslation3(new Rotation3(new Quaternion(1.1, -2.2, 3.3, -4.4)), new Vector3(4.1, -2.4, 1.3));

            var p = new Vector3(3.14, 2.88, 0.57);
            var u = new Vector3(-1, -2, 3);
            var v = new Vector3(-0.88, 0.49, 4.05);

            var origPlane = Plane.Parametric(p, u, v);

            var newPlane = x.Apply(origPlane);

            Expect(x.Apply(p).DistanceTo(newPlane), Is.LessThan(_tolerance));
            Expect(x.Apply(p + u).DistanceTo(newPlane), Is.LessThan(_tolerance));
            Expect(x.Apply(p + v).DistanceTo(newPlane), Is.LessThan(_tolerance));

            var q = new Vector3(0.17, -0.46, 0.83);

            Expect(x.Apply(q).SignedDistanceTo(newPlane), Is.EqualTo(q.SignedDistanceTo(origPlane)).Within(_tolerance)); // orientation test
        }

        [Test]
        public void ApplyInv_to_Plane_given_in_parameteric_form_yields_a_Plane_passing_through_inverse_images_of_points_on_original_Plane_with_correct_orientation()
        {
            var x = new RotoTranslation3(new Rotation3(new Quaternion(1.1, -2.2, 3.3, -4.4)), new Vector3(4.1, -2.4, 1.3));

            var p = new Vector3(3.14, 2.88, 0.57);
            var u = new Vector3(-1, -2, 3);
            var v = new Vector3(-0.88, 0.49, 4.05);

            var origPlane = Plane.Parametric(p, u, v);

            var newPlane = x.ApplyInv(origPlane);

            Expect(x.ApplyInv(p).DistanceTo(newPlane), Is.LessThan(_tolerance));
            Expect(x.ApplyInv(p + u).DistanceTo(newPlane), Is.LessThan(_tolerance));
            Expect(x.ApplyInv(p + v).DistanceTo(newPlane), Is.LessThan(_tolerance));

            var q = new Vector3(0.17, -0.46, 0.83);

            Expect(x.ApplyInv(q).SignedDistanceTo(newPlane), Is.EqualTo(q.SignedDistanceTo(origPlane)).Within(_tolerance)); // orientation test
        }

        [Test]
        public void CoordinateSystemWithPlaneXY_returns_coordinate_system_with_origin_on_Plane_and_z_axis_direction_coinciding_with_Plane_Normal()
        {
            var plane = new Plane(-3.141, new Vector3(2.718, -0.577, -1.248));

            var cs = RotoTranslation3Utils.CoordinateSystemWithPlaneXY(plane);

            Expect(cs.Translation.DistanceTo(plane), Is.LessThan(_tolerance));
            Expect(Vector3.Distance(cs.Rotation.ColZ(), plane.Normal), Is.LessThan(_tolerance));
        }

        [Test]
        public void Applying_RotoTranslation3_commutes_with_taking_PointAt_given_coordinate_from_Ray3()
        {
            var ray = new Ray3(origin: new Vector3(3.141, 2.718, 0.577), direction: new Vector3(1.23, 4.56, -3.21));
            var trans = new RotoTranslation3(new Rotation3(new Quaternion(1.1, -2.2, 3.3, -4.4)), new Vector3(4.1, -2.4, 1.3));

            var coord = 5.77;

            var expected = trans.Apply(ray.PointAt(coord));
            var actual = trans.Apply(ray).PointAt(coord);

            Expect(Vector3.Distance(actual, expected), Is.LessThan(_tolerance));
        }

        [Test]
        public void Applying_inverse_RotoTranslation3_commutes_with_taking_PointAt_given_coordinate_from_Ray3()
        {
            var ray = new Ray3(origin: new Vector3(3.141, 2.718, 0.577), direction: new Vector3(1.23, 4.56, -3.21));
            var trans = new RotoTranslation3(new Rotation3(new Quaternion(1.1, -2.2, 3.3, -4.4)), new Vector3(4.1, -2.4, 1.3));

            var coord = 5.77;

            var expected = trans.ApplyInv(ray.PointAt(coord));
            var actual = trans.ApplyInv(ray).PointAt(coord);

            Expect(Vector3.Distance(actual, expected), Is.LessThan(_tolerance));
        }
    }
}
