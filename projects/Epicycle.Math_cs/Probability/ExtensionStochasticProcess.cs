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

using Epicycle.Math.Geometry.Differential;
using Epicycle.Math.LinearAlgebra;

namespace Epicycle.Math.Probability
{
    public abstract class ExtensionStochasticProcess : IStochasticProcess
    {
        public ExtensionStochasticProcess(IStochasticProcess baseProcess, IManifold fiber)
        {
            _baseProcess = baseProcess;

            _stateSpace = new ProductManifold(_baseProcess.StateSpace, fiber);
        }

        private readonly IStochasticProcess _baseProcess;
        private readonly ProductManifold _stateSpace;

        public enum ManifoldProductOrder
        {
            Base = 0,
            Fiber = 1
        }

        IManifold IStochasticProcess.StateSpace
        {
            get { return _stateSpace; }
        }

        public ProductManifold StateSpace
        {
            get { return _stateSpace; }
        }

        public IManifold Base
        {
            get { return _stateSpace.Factors[(int)ManifoldProductOrder.Base]; }
        }

        public IManifold Fiber
        {
            get { return _stateSpace.Factors[(int)ManifoldProductOrder.Fiber]; }
        }

        public IStochasticProcess BaseProcess
        {
            get { return _baseProcess; }
        }

        public int BaseCoordinateIndex()
        {
            return BaseCoordinateIndex(_stateSpace);
        }

        public int FiberCoordinateIndex()
        {
            return FiberCoordinateIndex(_stateSpace);
        }

        public static int BaseCoordinateIndex(ProductManifold stateSpace)
        {
            return stateSpace.CoordinateIndex((int)ManifoldProductOrder.Base);
        }

        public static int FiberCoordinateIndex(ProductManifold stateSpace)
        {
            return stateSpace.CoordinateIndex((int)ManifoldProductOrder.Fiber);
        }

        public static IManifoldPoint CreateState(IManifoldPoint baseState, IManifoldPoint fiberState)
        {
            return new ProductManifold.Point(baseState, fiberState);
        }

        protected struct StochasticFiberPoint
        {
            public StochasticFiberPoint(StochasticManifoldPoint point, OMatrix mixedCovariance)
            {
                _point = point;
                _mixedCovariance = mixedCovariance;
            }

            private readonly StochasticManifoldPoint _point;
            private readonly OMatrix _mixedCovariance;

            public StochasticManifoldPoint Point
            {
                get { return _point; }
            }

            // covariance of fiber with base
            public OMatrix MixedCovariance
            {
                get { return _mixedCovariance; }
            }
        }

        protected struct State
        {
            public State(IManifoldPoint point)
            {
                var productPoint = (ProductManifold.Point)point;

                _basePoint = productPoint.Factors[(int)ManifoldProductOrder.Base];
                _fiberPoint = productPoint.Factors[(int)ManifoldProductOrder.Fiber];
            }

            private readonly IManifoldPoint _basePoint;
            private readonly IManifoldPoint _fiberPoint;

            public IManifoldPoint BasePoint
            {
                get { return _basePoint; }
            }

            public IManifoldPoint FiberPoint
            {
                get { return _fiberPoint; }
            }
        }

        public static IManifoldPoint GetBaseState(IManifoldPoint point)
        {
            return new State(point).BasePoint;
        }

        public static IManifoldPoint GetFiberState(IManifoldPoint point)
        {
            return new State(point).FiberPoint;
        }

        protected abstract StochasticFiberPoint FiberEvolution(State state, StochasticManifoldPoint basePoint2, double time);

        public StochasticManifoldPoint Apply(IManifoldPoint point, double time)
        {
            var state = new State(point);

            var basePoint = _baseProcess.Apply(state.BasePoint, time);
            var fiberPoint = FiberEvolution(state, basePoint, time);

            var answerExpectation = new ProductManifold.Point(basePoint.Expectation, fiberPoint.Point.Expectation);

            var answerCovariance = new SymmetricMatrix(_stateSpace.Dimension);

            answerCovariance.SetSubmatrix(0, basePoint.Covariance);
            answerCovariance.SetSubmatrix(_baseProcess.StateSpace.Dimension, fiberPoint.Point.Covariance);
            answerCovariance.SetSubmatrix(_baseProcess.StateSpace.Dimension, 0, fiberPoint.MixedCovariance);

            return new StochasticManifoldPoint(answerExpectation, answerCovariance);
        }

        protected abstract OSquareMatrix FiberDifferential(State state, IManifoldPoint basePoint2, double time);
        protected abstract OMatrix MixedDifferential(State state, IManifoldPoint basePoint2, OSquareMatrix baseDifferential, double time);

        public OSquareMatrix ExpectationDifferential(IManifoldPoint point, double time)
        {
            var state = new State(point);

            var basePoint = _baseProcess.Apply(state.BasePoint, time);

            var baseDifferential = _baseProcess.ExpectationDifferential(state.BasePoint, time);
            var mixedDifferential = MixedDifferential(state, basePoint.Expectation, baseDifferential, time);
            var fiberDifferential = FiberDifferential(state, basePoint.Expectation, time);

            var answer = new SquareMatrix(_stateSpace.Dimension);

            answer.SetSubmatrix(0, 0, baseDifferential);
            answer.SetSubmatrix(_baseProcess.StateSpace.Dimension, 0, mixedDifferential);
            answer.SetSubmatrix(_baseProcess.StateSpace.Dimension, _baseProcess.StateSpace.Dimension, fiberDifferential);

            return answer;
        }
    }
}