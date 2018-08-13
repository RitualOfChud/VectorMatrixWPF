using System;

namespace VectorMatrixClassLibrary
{
    public abstract class VectorMath
    {

        public static DWVector GetVectorLocation(DWVector iHat, DWVector jHat, double x, double y)
        {
            double X = x * iHat.X + y * jHat.X;
            double Y = x * iHat.Y + y * jHat.Y;

            return new DWVector(X, Y);
        }

        public static DWMatrix LinearTransformation(DWMatrix transformMatrix, DWMatrix origMatrix)
        {

            double ix = origMatrix.IX * transformMatrix.IX + origMatrix.IY * transformMatrix.JX;
            double iy = origMatrix.IX * transformMatrix.IY + origMatrix.IY * transformMatrix.JY;

            double jx = origMatrix.JX * transformMatrix.IX + origMatrix.JY * transformMatrix.JX;
            double jy = origMatrix.JX * transformMatrix.IY + origMatrix.JY * transformMatrix.JY;

            return new DWMatrix(ix, iy, jx, jy);
        }
         
        public static DWMatrix RotateNDegreesAntiClockwise(DWMatrix matrix, double angle)
        {

            double ix = matrix.IX * Math.Cos(angle) - matrix.IY * Math.Sin(angle);
            double iy = matrix.IX * Math.Sin(angle) + matrix.IY * Math.Cos(angle);
            double jx = matrix.JX * Math.Cos(angle) - matrix.JY * Math.Sin(angle);
            double jy = matrix.JX * Math.Sin(angle) + matrix.JY * Math.Cos(angle);

            return new DWMatrix(ix, iy, jx, jy);
        }

        public static DWMatrix Rotate90AntiClockwise(DWMatrix matrix)
        {
            double tempjx = matrix.JX;
            double tempjy = matrix.JY;
            double jx = -matrix.IX;
            double jy = -matrix.IY;
            double ix = tempjx;
            double iy = tempjy;

            return new DWMatrix(ix, iy, jx, jy);
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
