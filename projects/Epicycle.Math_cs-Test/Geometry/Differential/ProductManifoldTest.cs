using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using Moq;

using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Geometry.Differential
{
    [TestFixture]
    public sealed class ProductManifoldTest : AssertionHelper
    {
        private IManifold MockManifold(int dimension)
        {
            return Mock.Of<IManifold>(man => man.Dimension == dimension);
        }

        [Test]
        public void Dimension_is_sum_of_factor_dimensions()
        {
            var dim1 = 2;
            var dim2 = 3;
            var dim3 = 5;

            var man1 = MockManifold(dim1);
            var man2 = MockManifold(dim2);
            var man3 = MockManifold(dim3);

            var product = new ProductManifold(man1, man2, man3);

            Expect(product.Dimension, Is.EqualTo(dim1 + dim2 + dim3));
        }

        [Test]
        public void Coordinate_indices_are_accumulated_sum_of_factor_dimensions()
        {
            var dim1 = 2;
            var dim2 = 3;
            var dim3 = 5;

            var man1 = MockManifold(dim1);
            var man2 = MockManifold(dim2);
            var man3 = MockManifold(dim3);

            var product = new ProductManifold(man1, man2, man3);

            Expect(product.CoordinateIndex(0), Is.EqualTo(0));
            Expect(product.CoordinateIndex(1), Is.EqualTo(dim1));
            Expect(product.CoordinateIndex(2), Is.EqualTo(dim1 + dim2));
            Expect(product.CoordinateIndex(3), Is.EqualTo(dim1 + dim2 + dim3));
        }

        private IManifoldPoint MockManifoldPoint()
        {
            return Mock.Of<IManifoldPoint>();
        }

        private IManifold MockManifold(int dimension, IManifoldPoint untranslatedPoint, OVector translationVector, IManifoldPoint translatedPoint)
        {
            var answer = new Mock<IManifold>();

            answer.Setup(man => man.Dimension).Returns(dimension);
            answer.Setup(man => man.Translate(untranslatedPoint, translationVector)).Returns(translatedPoint);
            answer.Setup(man => man.GetTranslation(translatedPoint, untranslatedPoint)).Returns(translationVector);

            return answer.Object;
        }        

        [Test]
        public void Translating_product_point_is_equivalent_to_translating_each_factor_by_corresponding_subvector()
        {
            var untranslatedPoint1 = MockManifoldPoint();
            var untranslatedPoint2 = MockManifoldPoint();
            var untranslatedPoint3 = MockManifoldPoint();

            var translatedPoint1 = MockManifoldPoint();
            var translatedPoint2 = MockManifoldPoint();
            var translatedPoint3 = MockManifoldPoint();

            var man1 = MockManifold(2, untranslatedPoint1, new Vector(1, 2), translatedPoint1);
            var man2 = MockManifold(3, untranslatedPoint2, new Vector(3, 4, 5), translatedPoint2);
            var man3 = MockManifold(2, untranslatedPoint3, new Vector(6, 7), translatedPoint3);

            var productMan = new ProductManifold(man1, man2, man3);
            var productPnt = new ProductManifold.Point(untranslatedPoint1, untranslatedPoint2, untranslatedPoint3);

            var expected = new ProductManifold.Point(translatedPoint1, translatedPoint2, translatedPoint3);
            var actual = productMan.Translate(productPnt, new Vector(1, 2, 3, 4, 5, 6, 7));

            Expect(actual, Is.InstanceOf<ProductManifold.Point>());

            Expect(((ProductManifold.Point)actual).Factors.Count, Is.EqualTo(expected.Factors.Count));

            Expect(((ProductManifold.Point)actual).Factors[0], Is.EqualTo(expected.Factors[0]));
            Expect(((ProductManifold.Point)actual).Factors[1], Is.EqualTo(expected.Factors[1]));
            Expect(((ProductManifold.Point)actual).Factors[2], Is.EqualTo(expected.Factors[2]));
        }

        private const double _tolerance = 1e-9;

        [Test]
        public void GetTranslation_between_two_product_points_returns_concatenation_of_GetTranslation_on_invidividual_factors()
        {
            var untranslatedPoint1 = MockManifoldPoint();
            var untranslatedPoint2 = MockManifoldPoint();
            var untranslatedPoint3 = MockManifoldPoint();

            var translatedPoint1 = MockManifoldPoint();
            var translatedPoint2 = MockManifoldPoint();
            var translatedPoint3 = MockManifoldPoint();

            var man1 = MockManifold(2, untranslatedPoint1, new Vector(1, 2), translatedPoint1);
            var man2 = MockManifold(3, untranslatedPoint2, new Vector(3, 4, 5), translatedPoint2);
            var man3 = MockManifold(2, untranslatedPoint3, new Vector(6, 7), translatedPoint3);

            var productMan = new ProductManifold(man1, man2, man3);

            var from = new ProductManifold.Point(untranslatedPoint1, untranslatedPoint2, untranslatedPoint3);
            var to = new ProductManifold.Point(translatedPoint1, translatedPoint2, translatedPoint3);

            var expected = new Vector(1, 2, 3, 4, 5, 6, 7);
            var actual = productMan.GetTranslation(to, from);

            Expect(actual.Dimension, Is.EqualTo(expected.Dimension));
            Expect((actual - expected).Norm(), Is.LessThan(_tolerance));
        }
    }
}
