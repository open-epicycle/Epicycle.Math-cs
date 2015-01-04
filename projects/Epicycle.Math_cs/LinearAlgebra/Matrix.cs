using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics.LinearAlgebra.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

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
