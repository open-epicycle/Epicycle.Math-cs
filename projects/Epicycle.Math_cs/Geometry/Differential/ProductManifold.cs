using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Geometry.Differential
{
    public sealed class ProductManifold : IManifold
    {
        public ProductManifold(params IManifold[] factors)
        {
            _factors = factors;

            var indices = new List<int>(_factors.Count + 1);

            indices.Add(0);

            foreach (var factor in _factors)
            {
                indices.Add(indices.Last() + factor.Dimension);
            }

            _indices = indices;
        }

        private readonly IReadOnlyList<IManifold> _factors;
        private readonly IReadOnlyList<int> _indices;

        public int Dimension
        {
            get { return _indices.Last(); }
        }

        public IReadOnlyList<IManifold> Factors
        {
            get { return _factors; }
        }

        public int CoordinateIndex(int factorIndex)
        {
            return _indices[factorIndex];
        }

        public IManifoldPoint Translate(IManifoldPoint point, OVector tangentVector)
        {
            var nativePoint = (Point)point;

            var answerFactors = new List<IManifoldPoint>(_factors.Count);

            for (int i = 0; i < _factors.Count; i++)
            {
                answerFactors.Add(_factors[i].Translate(nativePoint.Factors[i], tangentVector.Subvector(_indices[i], _factors[i].Dimension)));
            }

            return new Point(answerFactors);
        }

        public OVector GetTranslation(IManifoldPoint to, IManifoldPoint from)
        {
            var nativeTo = (Point)to;
            var nativeFrom = (Point)from;

            var answer = new Vector(Dimension);

            for (int i = 0; i < _factors.Count; i++)
            {
                answer.SetSubvector(_indices[i], _factors[i].GetTranslation(nativeTo.Factors[i], nativeFrom.Factors[i]));
            }

            return answer;
        }

        public sealed class Point : IManifoldPoint
        {
            public Point(params IManifoldPoint[] factors) : this((IReadOnlyList<IManifoldPoint>)factors)
            {
                
            }

            public Point(IReadOnlyList<IManifoldPoint> factors)
            {
                _dimension = factors.Sum(man => man.Dimension);
                _factors = factors;
            }

            private readonly int _dimension;
            private readonly IReadOnlyList<IManifoldPoint> _factors;

            public int Dimension
            {
                get { return _dimension; }
            }

            public IReadOnlyList<IManifoldPoint> Factors
            {
                get { return _factors; }
            }
        }
    }    
}
