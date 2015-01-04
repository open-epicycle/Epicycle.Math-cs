using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Epicycle.Math.Geometry
{
    using System;

    [TestFixture]
    public sealed class Vector3Test : AssertionHelper
    {
        private const double _tolerance = 1e-10;

        [Test]
        public void Two_Vector3_with_equal_components_are_equal_as_objects()
        {
            var x = 1.1;
            var y = 2.2;
            var z = 3.3;

            object v = new Vector3(x, y, z);
            object w = new Vector3(x, y, z);

            Expect(v.Equals(w));
        }

        [Test]
        public void Two_Vector3_with_a_different_component_are_not_equal_as_objects()
        {
            var x = 1.1;
            var y = 2.2;
            var z = 3.3;

            object v = new Vector3(x, y, z);
            object w = new Vector3(x, y, z + 0.1);

            Expect(!v.Equals(w));
        }

        [Test]
        public void A_Vector3_is_not_equal_to_an_object_which_is_not_a_Vector3()
        {
            var x = 1.1;
            var y = 2.2;
            var z = 3.3;

            object v = new Vector3(x, y, z);
            object w = "Hello world";

            Expect(!v.Equals(w));
        }

        private IEnumerable<Vector3> VectorOrthogonalTo_TestCases()
        {
            yield return new Vector3(1, 2, 3);

            yield return new Vector3(0, 1, 2);
            yield return new Vector3(-1, 0, 1);
            yield return new Vector3(1.5, -0.8, 0);

            yield return new Vector3(3.141, 0, 0);
            yield return new Vector3(0, 2.718, 0);
            yield return new Vector3(0, 0, -0.577);
        }

        [Test]
        public void VectorOrthogonalTo_returns_Vector3_orthogonal_to_input_Vector3_which_is_not_zero_when_input_Vector3_is_not_zero
            ([ValueSource("VectorOrthogonalTo_TestCases")] Vector3 v)
        {
            var result = Vector3Utils.VectorOrthogonalTo(v);

            Expect(v * result, Is.EqualTo(0).Within(_tolerance));
            Expect(result.Norm, Is.GreaterThan(0.8 * v.Norm));
        }
    }
}
