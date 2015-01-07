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
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.LinearAlgebra
{
    public sealed class SquareMatrix : BaseSquareMatrix
    {
        // creates zero matrix
        public SquareMatrix(int dimension) : base(dimension) { }

        public SquareMatrix(double[,] values)
            : base(DenseMatrix.OfArray(values)) 
        {
            ArgAssert.Equal(values.GetLength(0), "values.GetLength(0)", values.GetLength(1), "values.GetLength(1)");
        }

        public SquareMatrix(params OVector[] columns)
            : base(DenseMatrix.OfColumns(columns.Length, columns.Length, columns.Select<OVector, IEnumerable<double>>(col => col))) { }

        internal SquareMatrix(Matrix<double> data) : base(data) { }

        public new double this[int row, int col]
        {
            get { return _data[row, col]; }
            set { _data[row, col] = value; }
        }

        public override IMatrix Transposed()
        {
            return SquareTransposed();
        }

        public override ISquareMatrix SquareTransposed()
        {
            return new SquareMatrix(this._data.Transpose());
        }

        public override IVector ApplyTransposed(IVector vector)
        {
            return new Vector(_data.TransposeThisAndMultiply(((Vector)vector).Data));
        }

        public override ISymmetricMatrix AsSymmetric()
        {
            var answer = new SymmetricMatrix(this.RowCount);

            for (var i = 0; i < this.RowCount; i++)
            {
                for (var j = 0; j <= i; j++)
                {
                    answer[i, j] = this[i, j];
                }
            }

            return answer;
        }

        public void SetSubmatrix(int row, int col, OMatrix submatrix)
        {
            _data.SetSubMatrix(row, submatrix.RowCount, col, submatrix.ColumnCount, ((BaseMatrix)submatrix.Value).Data);
        }

        public void SetRow(int i, OVector vector)
        {
            _data.SetRow(i, ((Vector)vector.Value).Data);
        }

        public void SetColumn(int i, OVector vector)
        {
            _data.SetColumn(i, ((Vector)vector.Value).Data);
        }

        public static OSquareMatrix Zero(int dimension)
        {
            return SymmetricMatrix.Zero(dimension);
        }

        public static OSquareMatrix Id(int dimension)
        {
            return SymmetricMatrix.Id(dimension);
        }

        public static OSquareMatrix Scalar(int dimension, double value)
        {
            return SymmetricMatrix.Scalar(dimension, value);
        }
    }
}
