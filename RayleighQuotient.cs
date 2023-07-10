using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class RayleighQuotient
{
    public double Solve(RectangularMatrix matrix, Vector startVector)
    {
        while (true)
        {
            var shift = (startVector * matrix * startVector) / (startVector * startVector);
        }
    }
}
