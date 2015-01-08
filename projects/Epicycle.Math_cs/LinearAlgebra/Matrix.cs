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

using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System.Linq;

namespace Epicycle.Math.LinearAlgebra
{
    public sealed class Matrix : BaseMatrix
    {
        // creates zero matrix
        public Matrix(int rowCount, int colCount)
            : base(rowCount, colCount) { }

        public Matrix(double[,] values)
            : base(DenseMatrix.OfArray(values)) { }

        public Matrix(params OVector[] columns)
            : base(DenseMatrix.OfColumns(columns.First().Dimension, columns.Length, columns.Select<OVector, IEnumerable<double>>(col => col))) { }

        internal Matrix(Matrix<double> data) : base(data) { }

        public new double this[int row, int col]
        {
            get { return _data[row, col]; }
            set { _data[row, col] = value; }
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

        public override IMatrix Transposed()
        {
            return new Matrix(this._data.Transpose());
        }

        public override IVector ApplyTransposed(IVector vector)
        {
            return new Vector(_data.TransposeThisAndMultiply(((Vector)vector).Data));
        }

        public override ISquareMatrix AsSquare()
        {
            return new SquareMatrix(_data);
        }

        public static OMatrix Zero(int rowCount, int colCount)
        {
            return new Matrix(rowCount, colCount);
        }

        public static OMatrix Id(int dimension)
        {
            return SymmetricMatrix.Id(dimension);
        }

        public static OMatrix Scalar(int dimension, double value)
        {
            return SymmetricMatrix.Scalar(dimension, value);
        }
    }
}
