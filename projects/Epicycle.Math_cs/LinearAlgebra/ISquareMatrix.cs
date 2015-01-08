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

namespace Epicycle.Math.LinearAlgebra
{
    public interface ISquareMatrix : IMatrix
    {
        // identity matrix of same dimension
        ISquareMatrix Id();

        // throws unless matrix.RowsCount == this.RowsCount
        ISquareMatrix Add(ISquareMatrix matrix);
        // throws unless matrix.RowsCount == this.RowsCount
        ISquareMatrix Subtract(ISquareMatrix matrix);

        // throws unless this.ColsCount == matrix.RowsCount
        ISquareMatrix Multiply(ISquareMatrix matrix);

        new ISquareMatrix Multiply(double scalar);
        new ISquareMatrix Divide(double scalar);

        new ISquareMatrix Transposed();

        ISquareMatrix Inv();

        // returns symmetric matrix with lower triangle same as this
        // doesn't verify this is actually symmetric
        ISymmetricMatrix AsSymmetric();
    }

    // wrapper for ISquareMatrix used for operator overloading
    public struct OSquareMatrix
    {
        public OSquareMatrix(ISquareMatrix matrix)
        {
            _matrix = matrix;
        }

        private readonly ISquareMatrix _matrix;

        internal ISquareMatrix Value
        {
            get { return _matrix; }
        }

        public static implicit operator OMatrix(OSquareMatrix m)
        {
            return new OMatrix(m.Value);
        }

        public int Dimension
        {
            get { return Value.RowCount; }
        }

        public double this[int row, int col]
        {
            get { return Value[row, col]; }
        }

        public OSquareMatrix Id()
        {
            return new OSquareMatrix(Value.Id());
        }

        public static OSquareMatrix operator +(OSquareMatrix m1, OSquareMatrix m2)
        {
            return new OSquareMatrix(m1.Value.Add(m2.Value));
        }

        public static OSquareMatrix operator -(OSquareMatrix m1, OSquareMatrix m2)
        {
            return new OSquareMatrix(m1.Value.Subtract(m2.Value));
        }

        public static OSquareMatrix operator *(OSquareMatrix m1, OSquareMatrix m2)
        {
            return new OSquareMatrix(m1.Value.Multiply(m2.Value));
        }

        public static OVector operator *(OSquareMatrix m, OVector v)
        {
            return new OVector(m.Value.Multiply(v.Value));
        }

        public static OSquareMatrix operator *(double x, OSquareMatrix m)
        {
            return new OSquareMatrix(m.Value.Multiply(x));
        }

        public static OSquareMatrix operator *(OSquareMatrix m, double x)
        {
            return new OSquareMatrix(m.Value.Multiply(x));
        }

        public static OSquareMatrix operator /(OSquareMatrix m, double x)
        {
            return new OSquareMatrix(m.Value.Divide(x));
        }

        public OSquareMatrix Transposed()
        {
            return new OSquareMatrix(Value.Transposed());
        }

        public OVector ApplyTransposed(OVector vector)
        {
            return new OVector(Value.ApplyTransposed(vector.Value));
        }

        public OSquareMatrix Inv()
        {
            return new OSquareMatrix(Value.Inv());
        }

        OVector GetRow(int i)
        {
            return new OVector(Value.GetRow(i));
        }

        OVector GetColumn(int j)
        {
            return new OVector(Value.GetColumn(j));
        }

        public double FrobeniusNorm()
        {
            return Value.FrobeniusNorm();
        }

        public OSymmetricMatrix AsSymmetric()
        {
            return new OSymmetricMatrix(Value.AsSymmetric());
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
