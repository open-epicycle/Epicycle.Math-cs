using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics.LinearAlgebra.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Epicycle.Math.LinearAlgebra
{
    public abstract class BaseMatrix : IMatrix
    {
        // creates zero matrix
        protected BaseMatrix(int rowCount, int colCount)
        {
            _data = new DenseMatrix(rowCount, colCount);
        }

        protected BaseMatrix(Matrix<double> data)
        {
            _data = data;
        }

        protected readonly Matrix<double> _data;

        public static implicit operator OMatrix(BaseMatrix matrix)
        {
            return new OMatrix(matrix);
        }

        internal Matrix<double> Data
        {
            get { return _data; }
        }

        public int RowCount
        {
            get { return _data.RowCount; }
        }

        public int ColumnCount
        {
            get { return _data.ColumnCount; }
        }

        public double this[int row, int col]
        {
            get { return _data[row, col]; }
        }

        public IMatrix Add(IMatrix matrix)
        {
            var that = (BaseMatrix)matrix; // Liskov violated because Add is morally a multi-method

            return new Matrix(this._data + that._data);
        }

        public IMatrix Subtract(IMatrix matrix)
        {
            var that = (BaseMatrix)matrix; // Liskov violated because Multiply is morally a multi-method

            return new Matrix(this._data - that._data);
        }

        public IMatrix Multiply(IMatrix matrix)
        {
            var that = (BaseMatrix)matrix; // Liskov violated because Multiply is morally a multi-method

            return new Matrix(this._data * that._data);
        }

        public IVector Multiply(IVector vector)
        {
            var that = (Vector)vector; // Liskov violated because... Oops, no good excuse

            return new Vector(this._data * that.Data);
        }

        public IMatrix Multiply(double scalar)
        {
            return new Matrix(scalar * _data);
        }

        public IMatrix Divide(double scalar)
        {
            return new Matrix((1 / scalar) * _data);
        }

        public IVector GetRow(int i)
        {
            return new Vector(_data.Row(i));
        }

        public IVector GetColumn(int j)
        {
            return new Vector(_data.Column(j));
        }

        public double FrobeniusNorm()
        {
            return _data.FrobeniusNorm();
        }

        public abstract ISquareMatrix AsSquare();

        public abstract IMatrix Transposed();
        public abstract IVector ApplyTransposed(IVector vector);

        public static OMatrix operator +(BaseMatrix m1, BaseMatrix m2)
        {
            return new OMatrix(m1.Add(m2));
        }

        public static OMatrix operator -(BaseMatrix m1, BaseMatrix m2)
        {
            return new OMatrix(m1.Subtract(m2));
        }

        public static OMatrix operator *(BaseMatrix m1, BaseMatrix m2)
        {
            return new OMatrix(m1.Multiply(m2));
        }

        public static OVector operator *(BaseMatrix m, Vector v)
        {
            return new OVector(m.Multiply(v));
        }

        public static OMatrix operator *(double x, BaseMatrix m)
        {
            return new OMatrix(m.Multiply(x));
        }

        public static OMatrix operator *(BaseMatrix m, double x)
        {
            return new OMatrix(m.Multiply(x));
        }

        public static OMatrix operator /(BaseMatrix m, double x)
        {
            return new OMatrix(m.Divide(x));
        }

        public override string ToString()
        {
            if (RowCount > 4 || ColumnCount > 4)
            {
                return base.ToString();
            }
            else
            {
                return "(" + string.Join(", ", _data.RowEnumerator().Select(x => new Vector(x.Item2).ToString())) + ")";
            }
        }
    }
}
