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
using System.Linq;

namespace Epicycle.Math.LinearAlgebra
{
    public sealed class Vector : IVector
    {
        // creates zero-vector of given dimension
        public Vector(int dimension)
        {
            _data = new DenseVector(dimension);
        }

        public Vector(params double[] values)
        {
            _data = new DenseVector(values);
        }

        internal Vector(Vector<double> data)
        {
            _data = data;
        }

        private Vector<double> _data;

        public static implicit operator OVector(Vector vector)
        {
            return new OVector(vector);
        }

        internal Vector<double> Data
        {
            get { return _data; }
        }

        public int Dimension
        {
            get { return _data.Count; }
        }

        public double this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        public IVector Add(IVector vector)
        {
            var that = (Vector)vector; // Liskov violated because Add is morally a multi-method

            return new Vector(this._data + that._data);
        }

        public IVector Subtract(IVector vector)
        {
            var that = (Vector)vector; // Liskov violated because Subtract is morally a multi-method

            return new Vector(this._data - that._data);
        }

        public IVector Minus()
        {
            return new Vector(-_data);
        }

        public IVector Multiply(double x)
        {
            return new Vector(x * _data);
        }

        public IVector Divide(double x)
        {
            return new Vector(_data / x);
        }

        public double Norm2()
        {
            double answer = 0;

            foreach (var value in _data)
            {
                answer += value * value;
            }

            return answer;
        }

        public IMatrix AsColumn()
        {
            return new Matrix(_data.ToColumnMatrix());
        }

        public IMatrix AsRow()
        {
            return new Matrix(_data.ToRowMatrix());
        }

        public IVector Subvector(int index, int count)
        {
            return new Vector(_data.SubVector(index, count));
        }

        public void SetSubvector(int index, OVector vector)
        {
            _data.SetSubVector(index, vector.Dimension, ((Vector)vector.Value)._data);
        }

        public static OVector operator +(Vector v1, Vector v2)
        {
            return new OVector(v1.Add(v2));
        }

        public static OVector operator -(Vector v1, Vector v2)
        {
            return new OVector(v1.Subtract(v2));
        }

        public static OVector operator +(Vector v)
        {
            return v;
        }

        public static OVector operator -(Vector v)
        {
            return new OVector(v.Minus());
        }

        public static OVector operator *(double x, Vector v)
        {
            return new OVector(v.Multiply(x));
        }

        public static OVector operator *(Vector v, double x)
        {
            return new OVector(v.Multiply(x));
        }

        public static OVector operator /(Vector v, double x)
        {
            return new OVector(v.Divide(x));
        }

        public static Vector Zero(int dimension)
        {
            return new Vector(dimension);
        }

        public static Vector Basis(int coordinate, int dimension)
        {
            var answer = new Vector(dimension);

            answer[coordinate] = 1;

            return answer;
        }

        public override string ToString()
        {
            if (Dimension > 4)
            {
                return base.ToString();
            }
            else
            {
                return "(" + string.Join(", ", _data.Select(x => x.ToString())) + ")";
            }
        }
    }
}
