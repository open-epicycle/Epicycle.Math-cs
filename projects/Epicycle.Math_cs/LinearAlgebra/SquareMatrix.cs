using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics.LinearAlgebra.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

using Epicycle.Commons;

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
