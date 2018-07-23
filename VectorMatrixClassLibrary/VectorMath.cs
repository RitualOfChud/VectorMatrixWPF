using System;

namespace VectorMatrixClassLibrary
{
    public abstract class VectorMath
    {

        public static DWVector GetVectorLocation(double iHat, double jHat, double x, double y)
        {
            double X = iHat * x;
            double Y = jHat * y;

            return new DWVector(X, Y);
        }

        public static DWMatrix Rotate90Clockwise (DWMatrix matrix)
        {
            double tempix = matrix.IX;
            double tempiy = matrix.IY;
            double ix = -matrix.JX;
            double iy = -matrix.JY;
            double jx = tempix;
            double jy = tempiy;

            return new DWMatrix(ix, iy, jx, jy);
        }

    }
}
