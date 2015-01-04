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
    public abstract class BaseSquareMatrix : BaseMatrix, ISquareMatrix
    {
        // creates zero matrix
        public BaseSquareMatrix(int dimension) : base(dimension, dimension) { }

        internal BaseSquareMatrix(Matrix<double> data)
            : base(data)
        {
            ArgAssert.Equal(data.RowCount, "data.RowsCount", data.ColumnCount, "data.ColumnCount");
        }

        public static implicit operator OSquareMatrix(BaseSquareMatrix matrix)
        {
            return new OSquareMatrix(matrix);
        }

        public ISquareMatrix Id()
        {
            return new SquareMatrix(DenseMatrix.Identity(this.RowCount));
        }

        public ISquareMatrix Add(ISquareMatrix matrix)
        {
            return base.Add(matrix).AsSquare();
        }

        public ISquareMatrix Subtract(ISquareMatrix matrix)
        {
            return base.Subtract(matrix).AsSquare();
        }

        public ISquareMatrix Multiply(ISquareMatrix matrix)
        {
            return base.Multiply(matrix).AsSquare();
        }

        public new ISquareMatrix Multiply(double scalar)
        {
            return base.Multiply(scalar).AsSquare();
        }

        public new ISquareMatrix Divide(double scalar)
        {
            return base.Divide(scalar).AsSquare();
        }

        public sealed override ISquareMatrix AsSquare()
        {
            return this;
        }

        ISquareMatrix ISquareMatrix.Transposed()
        {
            return SquareTransposed();
        }

        public ISquareMatrix Inv()
        {
            return new SquareMatrix(_data.Inverse());
        }

        public abstract ISquareMatrix SquareTransposed();

        public abstract ISymmetricMatrix AsSymmetric();

        public static OSquareMatrix operator +(BaseSquareMatrix m1, BaseSquareMatrix m2)
        {
            return new OSquareMatrix(m1.Add(m2));
        }

        public static OSquareMatrix operator -(BaseSquareMatrix m1, BaseSquareMatrix m2)
        {
            return new OSquareMatrix(m1.Subtract(m2));
        }

        public static OSquareMatrix operator *(BaseSquareMatrix m1, BaseSquareMatrix m2)
        {
            return new OSquareMatrix(m1.Multiply(m2));
        }

        public static OSquareMatrix operator *(double x, BaseSquareMatrix m)
        {
            return new OSquareMatrix(m.Multiply(x));
        }

        public static OSquareMatrix operator *(BaseSquareMatrix m, double x)
        {
            return new OSquareMatrix(m.Multiply(x));
        }

        public static OSquareMatrix operator /(BaseSquareMatrix m, double x)
        {
            return new OSquareMatrix(m.Divide(x));
        }
    }
}
