using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.LinearAlgebra
{
    public interface IMatrix
    {
        int RowCount { get; }
        int ColumnCount { get; }

        double this[int row, int col] { get; }

        // throws unless matrices are of equal dimensions
        IMatrix Add(IMatrix matrix);
        // throws unless matrices are of equal dimensions
        IMatrix Subtract(IMatrix matrix);

        // throws unless this.ColsCount == matrix.RowsCount
        IMatrix Multiply(IMatrix matrix);

        // throws unless this.ColsCount == vector.Dimension
        IVector Multiply(IVector vector);
        IVector ApplyTransposed(IVector vector);

        IMatrix Multiply(double scalar);
        IMatrix Divide(double scalar);

        IMatrix Transposed();

        IVector GetRow(int i);
        IVector GetColumn(int j);

        double FrobeniusNorm();

        // throws unless this.RowsCount == this.ColsCount
        ISquareMatrix AsSquare();
    }

    // wrapper for IMatrix used for operator overloading
    public struct OMatrix
    {
        public OMatrix(IMatrix matrix)
        {
            _matrix = matrix;
        }

        private readonly IMatrix _matrix;

        internal IMatrix Value
        {
            get { return _matrix; }
        }

        public int RowCount 
        {
            get { return Value.RowCount; }
        }

        public int ColumnCount
        {
            get { return Value.ColumnCount; }
        }

        public double this[int row, int col] 
        {
            get { return Value[row, col]; }
        }

        public static OMatrix operator +(OMatrix m1, OMatrix m2)
        {
            return new OMatrix(m1.Value.Add(m2.Value));
        }

        public static OMatrix operator -(OMatrix m1, OMatrix m2)
        {
            return new OMatrix(m1.Value.Subtract(m2.Value));
        }

        public static OMatrix operator *(OMatrix m1, OMatrix m2)
        {
            return new OMatrix(m1.Value.Multiply(m2.Value));
        }

        public static OVector operator *(OMatrix m, OVector v)
        {
            return new OVector(m.Value.Multiply(v.Value));
        }

        public static OMatrix operator *(double x, OMatrix m)
        {
            return new OMatrix(m.Value.Multiply(x));
        }

        public static OMatrix operator *(OMatrix m, double x)
        {
            return new OMatrix(m.Value.Multiply(x));
        }

        public static OMatrix operator /(OMatrix m, double x)
        {
            return new OMatrix(m.Value.Divide(x));
        }

        public OMatrix Transposed()
        {
            return new OMatrix(Value.Transposed());
        }

        public OVector ApplyTransposed(OVector vector)
        {
            return new OVector(Value.ApplyTransposed(vector.Value));
        }

        public OVector GetRow(int i)
        {
            return new OVector(Value.GetRow(i));
        }

        public OVector GetColumn(int j)
        {
            return new OVector(Value.GetColumn(j));
        }

        public double FrobeniusNorm()
        {
            return Value.FrobeniusNorm();
        }

        public OSquareMatrix AsSquare()
        {
            return new OSquareMatrix(Value.AsSquare());
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
