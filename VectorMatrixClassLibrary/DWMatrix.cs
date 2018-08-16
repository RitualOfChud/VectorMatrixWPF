using System;
using System.Collections.Generic;
using System.Text;

namespace VectorMatrixClassLibrary
{
    public class DWMatrix
    {

        public double IX { get; set; }
        public double IY { get; set; }
        public double JX { get; set; }
        public double JY { get; set; }

        public DWMatrix() {}

        public DWMatrix(double iX, double iY, double jX, double jY)
        {
            IX = iX;
            IY = iY;
            JX = jX;
            JY = jY;
        }

        public DWMatrix(DWVector ihat, DWVector jhat)
        {
            IX = ihat.X;
            IY = ihat.Y;
            JX = jhat.X;
            JY = jhat.Y;
        }

        //////////////////////////
        // OPERATOR OVERLOADING //
        //////////////////////////

        /// <summary>
        /// Overload the + operator
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static DWMatrix operator +(DWMatrix m1, DWMatrix m2)
        {
            double ix = m1.IX + m2.IX;
            double iy = m1.IY + m2.IY;
            double jx = m1.JX + m2.JX;
            double jy = m1.JY + m2.JY;
            return new DWMatrix(ix, iy, jx, jy);
        }

        public static DWMatrix operator -(DWMatrix m1, DWMatrix m2)
        {
            double ix = m1.IX - m2.IX;
            double iy = m1.IY - m2.IY;
            double jx = m1.JX - m2.JX;
            double jy = m1.JY - m2.JY;
            return new DWMatrix(ix, iy, jx, jy);
        }

        public static DWMatrix operator *(DWMatrix m1, DWMatrix m2)
        {
            double ix = m1.IX * m2.IX + m1.JX * m2.IY;
            double jx = m1.IX * m2.JX + m1.JX * m2.JY;
            double iy = m1.IY * m2.IX + m1.JY * m2.IY;
            double jy = m1.IY * m2.JX + m1.JY * m2.JY;
            return new DWMatrix(ix, iy, jx, jy);
        }

        public static DWMatrix operator *(DWMatrix matrix, double scalar)
        {
            double ix = matrix.IX * scalar;
            double iy = matrix.IY * scalar;
            double jx = matrix.JX * scalar;
            double jy = matrix.JY * scalar;
            return new DWMatrix(ix, iy, jx, jy);
        }

        public static DWMatrix operator /(DWMatrix m1, double n)
        {
            double ix = m1.IX / n;
            double iy = m1.IY / n;
            double jx = m1.JX / n;
            double jy = m1.JY / n;
            return new DWMatrix(ix, iy, jx, jy);
        }
    }
}
