using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorMatrixWPF.Exceptions
{
    public class OutOfGraphBoundsException : Exception
    {

        public OutOfGraphBoundsException() : base($"Vector out of graph bounds, cannot be displayed") { }
        public OutOfGraphBoundsException(string axis, double val, double max) : base($"{axis}:{val} out of bounds (max: {max}") { }
    }
}
