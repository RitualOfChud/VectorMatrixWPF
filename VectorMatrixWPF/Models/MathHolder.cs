using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorMatrixClassLibrary;

namespace VectorMatrixWPF.Models
{
    public class MathHolder
    {

        public MathHolder() { }

        public DWVector IHat { get; set; } = new DWVector();
        public DWVector JHat { get; set; } = new DWVector();

    }
}
