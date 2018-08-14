using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using VectorMatrixClassLibrary;

namespace VectorMatrixWPF.Models
{
    public class DWGridLine
    {
        public DWVector Vector1 { get; set; }
        public DWVector Vector2 { get; set; }
        public DWLine DWLine { get; set; }

        public DWGridLine(DWVector vec1, DWVector vec2, DWLine dwline)
        {
            Vector1 = vec1;
            Vector2 = vec2;
            DWLine = dwline;
        }

    }
}
