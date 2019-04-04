using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorMatrixClassLibrary;

namespace VectorMatrixWPF.Models
{
    public class Transformation
    {

        public DWMatrix InitialPlane { get; set; }
        public DWMatrix NewPlane { get; set; }
        public DWMatrix InverseMatrix { get; set; }
        public double Degrees { get; set; }

        public Transformation(DWMatrix initialPlane, DWMatrix newPlane, DWMatrix inverseMatrix, double degrees)
        {
            InitialPlane = initialPlane;
            NewPlane = newPlane;
            InverseMatrix = inverseMatrix;
            Degrees = degrees;
        }
    }
}
