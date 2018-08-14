using System;

namespace VectorMatrixClassLibrary
{
    public abstract class VectorMath
    {
        /// <summary>
        /// Takes an old X/Y coord and returns a new vector based on the current i-hat & j-hat
        /// </summary>
        /// <param name="iHat"></param>
        /// <param name="jHat"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Returns a DWVector with new coords for the current i-hat/j-hat</returns>
        public static DWVector GetNewVectorLocation(DWVector iHat, DWVector jHat, double x, double y)
        {
            double X = x * iHat.X + y * jHat.X;
            double Y = x * iHat.Y + y * jHat.Y;

            return new DWVector(X, Y);
        }

        /// <summary>
        /// Uses matrix multiplication on a transformation matrix and the current plane
        /// </summary>
        /// <param name="transformMatrix">The wanted transformation</param>
        /// <param name="basisMatrix">The basis vectors in a matrix (i-hat & j-hat)</param>
        /// <returns>A new matrix giving the new basis vectors</returns>
        public static DWMatrix LinearTransformation(DWMatrix transformMatrix, DWMatrix basisMatrix)
        {
            double ix = basisMatrix.IX * transformMatrix.IX + basisMatrix.IY * transformMatrix.JX;
            double iy = basisMatrix.IX * transformMatrix.IY + basisMatrix.IY * transformMatrix.JY;

            double jx = basisMatrix.JX * transformMatrix.IX + basisMatrix.JY * transformMatrix.JX;
            double jy = basisMatrix.JX * transformMatrix.IY + basisMatrix.JY * transformMatrix.JY;

            return new DWMatrix(ix, iy, jx, jy);
        }
        
        /// <summary>
        /// Takes the current plane as a matrix and returns a rotated version
        /// </summary>
        /// <param name="basisMatrix"></param>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static DWMatrix RotateNRadiansAntiClockwise(DWMatrix basisMatrix, double radians)
        {
            double ix = basisMatrix.IX * Math.Cos(radians) - basisMatrix.IY * Math.Sin(radians);
            double iy = basisMatrix.IX * Math.Sin(radians) + basisMatrix.IY * Math.Cos(radians);
            double jx = basisMatrix.JX * Math.Cos(radians) - basisMatrix.JY * Math.Sin(radians);
            double jy = basisMatrix.JX * Math.Sin(radians) + basisMatrix.JY * Math.Cos(radians);

            return new DWMatrix(ix, iy, jx, jy);
        }
    }
}
