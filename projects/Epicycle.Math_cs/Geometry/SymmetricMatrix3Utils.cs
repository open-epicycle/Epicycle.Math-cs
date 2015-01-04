using System.Collections.Generic;
using System.Linq;

using Epicycle.Commons;

namespace Epicycle.Math.Geometry
{
    using System;

    public static class SymmetricMatrix3Utils
    {
        public static Vector3 RowX(this SymmetricMatrix3 @this)
        {
            return new Vector3(@this.XX, @this.XY, @this.ZX);
        }

        public static Vector3 RowY(this SymmetricMatrix3 @this)
        {
            return new Vector3(@this.XY, @this.YY, @this.YZ);
        }

        public static Vector3 RowZ(this SymmetricMatrix3 @this)
        {
            return new Vector3(@this.ZX, @this.YZ, @this.ZZ);
        }

        public static double Trace(this SymmetricMatrix3 @this)
        {
            return @this.XX + @this.YY + @this.ZZ;
        }

        public static double Det(this SymmetricMatrix3 @this)
        {
            var mx = @this.YY * @this.ZZ - @this.YZ * @this.YZ;
            var my = @this.XY * @this.ZZ - @this.YZ * @this.ZX;
            var mz = @this.XY * @this.YZ - @this.YY * @this.ZX;

            return @this.XX * mx - @this.XY * my + @this.ZX * mz;
        }

        public static double LowestEigenvalue(this SymmetricMatrix3 @this)
        {
            var b = @this.Trace();

            var c =
                -@this.YY * @this.ZZ + @this.YZ * @this.YZ
                -@this.XX * @this.ZZ + @this.ZX * @this.ZX
                -@this.XX * @this.YY + @this.XY * @this.XY;

            var d = @this.Det();

            return CubicEquation.Solve(-1, b, c, d).Min();
        }

        private static Vector3 Eigenvector(this SymmetricMatrix3 @this, double eigenvalue)
        {
            var rowX = new Vector3(@this.XX - eigenvalue, @this.XY, @this.ZX);
            var rowY = new Vector3(@this.XY, @this.YY - eigenvalue, @this.YZ);
            var rowZ = new Vector3(@this.ZX, @this.YZ, @this.ZZ - eigenvalue);

            var normX2 = rowX.Norm2;
            var normY2 = rowY.Norm2;
            var normZ2 = rowZ.Norm2;

            Vector3 row1, row2, row3;

            if (normX2 > normY2)
            {
                if (normX2 > normZ2)
                {
                    row1 = rowX / Math.Sqrt(normX2);
                    row2 = rowY;
                    row3 = rowZ;
                }
                else
                {
                    row1 = rowZ / Math.Sqrt(normZ2);
                    row2 = rowX;
                    row3 = rowY;
                }
            }
            else
            {
                if (normY2 > normZ2)
                {
                    row1 = rowY / Math.Sqrt(normY2);
                    row2 = rowX;
                    row3 = rowZ;
                }
                else
                {
                    row1 = rowZ / Math.Sqrt(normZ2);
                    row2 = rowY;
                    row3 = rowX;
                }
            }

            var cross2 = row1.Cross(row2);
            var cross3 = row1.Cross(row3);

            var norm22 = cross2.Norm2;
            var norm32 = cross3.Norm2;

            if (norm22 > norm32)
            {
                if (norm22 > BasicMath.Epsilon)
                {
                    return cross2 / Math.Sqrt(norm22);
                }
                else
                {
                    return Vector3Utils.VectorOrthogonalTo(row1).Normalized;
                }
            }
            else
            {
                if (norm32 > BasicMath.Epsilon)
                {
                    return cross3 / Math.Sqrt(norm32);
                }
                else
                {
                    return Vector3Utils.VectorOrthogonalTo(row1).Normalized;
                }
            }
        }

        public static Vector3 LowestEigenvector(this SymmetricMatrix3 @this)
        {
            return @this.Eigenvector(@this.LowestEigenvalue());
        }

        public static void ComputeLowestEigendata(this SymmetricMatrix3 @this, out double eigenvalue, out Vector3 eigenvector)
        {
            eigenvalue = @this.LowestEigenvalue();
            eigenvector = @this.Eigenvector(eigenvalue);
        }
    }
}
