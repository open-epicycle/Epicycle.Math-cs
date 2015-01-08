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
using Epicycle.Math.Geometry;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;
using System;

namespace Epicycle.Math.LinearAlgebra
{
    // this implementation stores data redundantly as n^2 doubles; a more economic implementation is possible
    public sealed class SymmetricMatrix : BaseSquareMatrix, ISymmetricMatrix
    {
        // creates zero matrix
        public SymmetricMatrix(int dimension) : base(dimension) { }

        public SymmetricMatrix(double[][] values) : base(values.Length) 
        {
            for (int i = 0; i < this.RowCount; i++)
            {
                ArgAssert.Equal(values[i].Length, "values[i].Length", i + 1, "i + 1");

                _data[i, i] = values[i][i];

                for (int j = 0; j < i; j++)
                {
                    _data[i, j] = values[i][j];
                    _data[j, i] = values[i][j];
                }
            }
        }

        // this private ctor is only used in contexts in which data is known to be symmetric
        private SymmetricMatrix(Matrix<double> data) : base(data) { }

        public static implicit operator OSymmetricMatrix(SymmetricMatrix matrix)
        {
            return new OSymmetricMatrix(matrix);
        }

        public new double this[int row, int col]
        {
            get { return _data[row, col]; }

            set 
            { 
                _data[row, col] = value;
                _data[col, row] = value;
            }
        }

        public new ISymmetricMatrix Id()
        {
            return new SymmetricMatrix(DenseMatrix.Identity(this.RowCount));
        }

        public ISymmetricMatrix Conjugate(IMatrix matrix)
        {
            var that = (BaseMatrix)matrix; // Liskov, I'm sorry

            return new SymmetricMatrix(that.Data * this.Data * that.Data.Transpose());
        }

        public ISymmetricMatrix Add(ISymmetricMatrix matrix)
        {
            var that = (SymmetricMatrix)matrix; // Liskov violated because Add is morally a multi-method

            return new SymmetricMatrix(this._data + that._data);
        }

        public ISymmetricMatrix Subtract(ISymmetricMatrix matrix)
        {
            var that = (SymmetricMatrix)matrix; // Liskov violated because Add is morally a multi-method

            return new SymmetricMatrix(this._data - that._data);
        }

        public new ISymmetricMatrix Multiply(double scalar)
        {
            return new SymmetricMatrix(scalar * this._data);
        }

        public new ISymmetricMatrix Divide(double scalar)
        {
            return new SymmetricMatrix((1 / scalar) * this._data);
        }
        
        public new ISymmetricMatrix Inv()
        {
            return new SymmetricMatrix(_data.Inverse());
        }
        
        public override IMatrix Transposed()
        {
            return this;
        }

        public override IVector ApplyTransposed(IVector vector)
        {
            return vector;
        }

        public override ISquareMatrix SquareTransposed()
        {
            return this;
        }

        public override ISymmetricMatrix AsSymmetric()
        {
            return this;
        }

        public static OSymmetricMatrix operator +(SymmetricMatrix m1, SymmetricMatrix m2)
        {
            return new OSymmetricMatrix(m1.Add(m2));
        }

        public static OSymmetricMatrix operator -(SymmetricMatrix m1, SymmetricMatrix m2)
        {
            return new OSymmetricMatrix(m1.Subtract(m2));
        }

        public static OSymmetricMatrix operator *(double x, SymmetricMatrix m)
        {
            return new OSymmetricMatrix(m.Multiply(x));
        }

        public static OSymmetricMatrix operator *(SymmetricMatrix m, double x)
        {
            return new OSymmetricMatrix(m.Multiply(x));
        }

        public static OSymmetricMatrix operator /(SymmetricMatrix m, double x)
        {
            return new OSymmetricMatrix(m.Divide(x));
        }

        public void SetSubmatrix(int index, OSymmetricMatrix matrix)
        {
            _data.SetSubMatrix(index, matrix.Dimension, index, matrix.Dimension, ((SymmetricMatrix)matrix.Value)._data);
        }

        public void SetSubmatrix(int row, int col, OMatrix matrix)
        {
            var box1 = new Box2i(col, row, matrix.ColumnCount, matrix.RowCount);
            var box2 = new Box2i(row, col, matrix.RowCount, matrix.ColumnCount);

            if (!Box2i.Intersection(box1, box2).IsEmpty)
            {
                throw new ArgumentException("To preserve symmetry, submatrix should not intersect its own transpose");
            }

            _data.SetSubMatrix(row, matrix.RowCount, col, matrix.ColumnCount, ((BaseMatrix)matrix.Value).Data);
            _data.SetSubMatrix(col, matrix.ColumnCount, row, matrix.RowCount, ((BaseMatrix)matrix.Value).Data.Transpose());
        }

        public static OSymmetricMatrix Zero(int dimension)
        {
            return new SymmetricMatrix(dimension);
        }

        public static OSymmetricMatrix Id(int dimension)
        {
            return new SymmetricMatrix(DenseMatrix.Identity(dimension));
        }

        public static OSymmetricMatrix Scalar(int dimension, double value)
        {
            var answer = new SymmetricMatrix(dimension);

            for (int i = 0; i < dimension; i++)
            {
                answer._data[i, i] = value;
            }

            return answer;
        }
    }
}
