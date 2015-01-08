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
    public interface ISymmetricMatrix : ISquareMatrix
    {
        // returns matrix * this * matrix.Transposed()
        // throws unless matrix.ColsCount == this.RowsCount
        ISymmetricMatrix Conjugate(IMatrix matrix);

        // throws unless matrix.ColsCount == this.ColsCount
        ISymmetricMatrix Add(ISymmetricMatrix matrix);
        // throws unless matrix.ColsCount == this.ColsCount
        ISymmetricMatrix Subtract(ISymmetricMatrix matrix);

        new ISymmetricMatrix Multiply(double scalar);
        new ISymmetricMatrix Divide(double scalar);

        new ISymmetricMatrix Inv();

        // identity matrix of same dimension
        new ISymmetricMatrix Id();
    }

    // wrapper for ISymmetricMatrix used for operator overloading
    public struct OSymmetricMatrix
    {
        public OSymmetricMatrix(ISymmetricMatrix matrix)
        {
            _matrix = matrix;
        }

        private readonly ISymmetricMatrix _matrix;

        public static implicit operator OSquareMatrix(OSymmetricMatrix matrix)
        {
            return new OSquareMatrix(matrix.Value);
        }

        public static implicit operator OMatrix(OSymmetricMatrix matrix)
        {
            return new OMatrix(matrix.Value);
        }

        internal ISymmetricMatrix Value
        {
            get { return _matrix; }
        }

        public int Dimension
        {
            get { return Value.RowCount; }
        }

        public double this[int row, int col]
        {
            get { return Value[row, col]; }
        }

        public OSymmetricMatrix Conjugate(OMatrix matrix)
        {
            return new OSymmetricMatrix(Value.Conjugate(matrix.Value));
        }

        public static OSymmetricMatrix operator +(OSymmetricMatrix m1, OSymmetricMatrix m2)
        {
            return new OSymmetricMatrix(m1.Value.Add(m2.Value));
        }

        public static OSymmetricMatrix operator -(OSymmetricMatrix m1, OSymmetricMatrix m2)
        {
            return new OSymmetricMatrix(m1.Value.Subtract(m2.Value));
        }

        public static OSquareMatrix operator *(OSymmetricMatrix m1, OSymmetricMatrix m2)
        {
            return new OSquareMatrix(m1.Value.Multiply(m2.Value));
        }

        public static OSquareMatrix operator *(OSymmetricMatrix m1, OSquareMatrix m2)
        {
            return new OSquareMatrix(m1.Value.Multiply(m2.Value));
        }

        public static OSquareMatrix operator *(OSquareMatrix m1, OSymmetricMatrix m2)
        {
            return new OSquareMatrix(m1.Value.Multiply(m2.Value));
        }

        public static OMatrix operator *(OSymmetricMatrix m1, OMatrix m2)
        {
            return new OMatrix(m1.Value.Multiply(m2.Value));
        }

        public static OMatrix operator *(OMatrix m1, OSymmetricMatrix m2)
        {
            return new OMatrix(m1.Value.Multiply(m2.Value));
        }

        public static OVector operator *(OSymmetricMatrix m, OVector v)
        {
            return new OVector(m.Value.Multiply(v.Value));
        }

        public static OSymmetricMatrix operator *(double x, OSymmetricMatrix m)
        {
            return new OSymmetricMatrix(m.Value.Multiply(x));
        }

        public static OSymmetricMatrix operator *(OSymmetricMatrix m, double x)
        {
            return new OSymmetricMatrix(m.Value.Multiply(x));
        }

        public static OSymmetricMatrix operator /(OSymmetricMatrix m, double x)
        {
            return new OSymmetricMatrix(m.Value.Divide(x));
        }

        public OSymmetricMatrix Inv()
        {
            return new OSymmetricMatrix(Value.Inv());
        }

        OSymmetricMatrix Id()
        {
            return new OSymmetricMatrix(Value.Id());
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

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
