using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Epicycle.Math.Geometry
{
    using System;

    [TestFixture]
    public sealed class Rotation3Test : AssertionHelper
    {
        private const double _tolerance = 1e-10;

        private void ExpectAxisAngle(Rotation3 rot, Vector3 axis, double angle)
        {
            Expect(rot.Angle, Is.EqualTo(angle).Within(_tolerance));
            Expect(Vector3.Distance(rot.Axis, axis), Is.LessThan(_tolerance));
        }

        [Test]
        public void Axis_and_Angle_are_correct_for_basis_quaternions()
        {
            var one = new Rotation3(Quaternion.One);
            Expect(one.Angle, Is.EqualTo(0).Within(_tolerance));

            var i = new Rotation3(Quaternion.I);
            ExpectAxisAngle(i, axis: Vector3.UnitX, angle: Math.PI);

            var j = new Rotation3(Quaternion.J);
            ExpectAxisAngle(j, axis: Vector3.UnitY, angle: Math.PI);

            var k = new Rotation3(Quaternion.K);
            ExpectAxisAngle(k, axis: Vector3.UnitZ, angle: Math.PI);
        }

        [Test]
        public void Basis_quaternion_rotations_are_computed_from_their_axis_and_angle()
        {
            var one = new Rotation3(angle: 0, axis: new Vector3(0.1, 0.2, 0.3));
            Expect(Quaternion.Distance(one.Quaternion, Quaternion.One), Is.LessThan(_tolerance));

            var i = new Rotation3(angle: Math.PI, axis: Vector3.UnitX);
            Expect(Quaternion.Distance(i.Quaternion, Quaternion.I), Is.LessThan(_tolerance));

            var j = new Rotation3(angle: Math.PI, axis: Vector3.UnitY);
            Expect(Quaternion.Distance(j.Quaternion, Quaternion.J), Is.LessThan(_tolerance));

            var k = new Rotation3(angle: Math.PI, axis: Vector3.UnitZ);
            Expect(Quaternion.Distance(k.Quaternion, Quaternion.K), Is.LessThan(_tolerance));
        }

        [Test]
        public void Axis_and_Angle_are_consistent_with_given_in_ctor()
        {
            var angle = 2.56;
            var axis = new Vector3(4.1, -0.9, 1.3);

            var rot = new Rotation3(angle, axis);

            Expect(rot.Angle, Is.EqualTo(angle).Within(_tolerance));
            Expect(Vector3.Distance(rot.Axis, axis.Normalized), Is.LessThan(_tolerance));
        }

        [Test]
        public void Rotation3_constructed_from_axis_and_angle_applies_according_to_Rodrigues_formula()
        {
            var angle = 2.56;
            var axis = new Vector3(4.1, -0.9, 1.3).Normalized;

            var rot = new Rotation3(angle, axis);

            var vector = new Vector3(-1.23, 4.56, -7.89);

            var rotatedVector = rot.Apply(vector);

            var rodrigues = Math.Cos(angle) * vector + Math.Sin(angle) * axis.Cross(vector) + (1 - Math.Cos(angle)) * (axis * vector) * axis;

            Expect(Vector3.Distance(rotatedVector, rodrigues), Is.LessThan(_tolerance));
        }

        private void ExpectEqualRotations(Rotation3 rot1, Rotation3 rot2)
        {
            Expect(Quaternion.Distance(rot1, rot2) < _tolerance || Quaternion.Distance(rot1, -rot2.Quaternion) < _tolerance);
        }

        IEnumerable<Rotation3> GramSchmidt_TestCases()
        {
            yield return new Rotation3(angle: 1.0, axis: new Vector3(3.141, 2.718, 0.577));

            yield return (Rotation3)new Quaternion(0, 2.718, 0.577, 2.665);

            yield return Rotation3Utils.RotationX180;
            yield return Rotation3Utils.RotationY180;
            yield return Rotation3Utils.RotationZ180;
        }

        [Test]
        public void GramSchmidt_reconstructs_rotation_from_its_x_column_and_a_linear_combination_of_the_x_and_y_columns([ValueSource("GramSchmidt_TestCases")] Rotation3 rot)
        {
            var x = 4.3 * rot.ColX();
            var y = 3.4 * rot.ColY() + 0.123 * x;

            var result = Rotation3Utils.GramSchmidt(x, y);

            ExpectEqualRotations(result, rot);
        }
    }
}
