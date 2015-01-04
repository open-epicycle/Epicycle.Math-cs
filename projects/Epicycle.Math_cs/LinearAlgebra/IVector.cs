using System.Collections.Generic;
using System.Linq;

using Epicycle.Math.Geometry.Differential;

namespace Epicycle.Math.LinearAlgebra
{
    using System;

    public interface IVector
    {
        int Dimension { get; }

        double this[int index] { get; }

        // throws unless vector.Dimension == this.Dimension
        IVector Add(IVector vector);
        // throws unless vector.Dimension == this.Dimension
        IVector Subtract(IVector vector);

        IVector Minus();

        IVector Multiply(double scalar);
        IVector Divide(double scalar);

        IVector Subvector(int index, int count);

        double Norm2();

        IMatrix AsColumn();
        IMatrix AsRow();
    }

    // wrapper for IVector used for operator overloading
    public struct OVector : IManifoldPoint, IEnumerable<double>
    {
        public OVector(IVector vector)
        {
            _vector = vector;
        }

        private readonly IVector _vector;

        internal IVector Value
        {
            get { return _vector; }
        }

        public int Dimension
        {
            get { return Value.Dimension; }
        }

        public double this[int index]
        {
            get { return Value[index]; }
        }

        public static OVector operator +(OVector m1, OVector m2)
        {
            return new OVector(m1.Value.Add(m2.Value));
        }

        public static OVector operator -(OVector m1, OVector m2)
        {
            return new OVector(m1.Value.Subtract(m2.Value));
        }

        public static OVector operator +(OVector v)
        {
            return v;
        }

        public static OVector operator -(OVector v)
        {
            return new OVector(v.Value.Minus());
        }

        public static OVector operator *(double x, OVector v)
        {
            return new OVector(v.Value.Multiply(x));
        }

        public static OVector operator *(OVector v, double x)
        {
            return new OVector(v.Value.Multiply(x));
        }

        public static OVector operator /(OVector v, double x)
        {
            return new OVector(v.Value.Divide(x));
        }

        public OVector Subvector(int index, int count)
        {
            return new OVector(Value.Subvector(index, count));
        }

        public double Norm2()
        {
            return Value.Norm2();
        }

        public double Norm()
        {
            return Math.Sqrt(Value.Norm2());
        }

        public OMatrix AsColumn()
        {
            return new OMatrix(Value.AsColumn());
        }

        public OMatrix AsRow()
        {
            return new OMatrix(Value.AsRow());
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < Dimension; i++)
            {
                yield return Value[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
