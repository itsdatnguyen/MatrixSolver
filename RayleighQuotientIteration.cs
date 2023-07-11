using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class RayleighQuotientIteration
{
    public (double eigenvalue, int iterations) Converge(RectangularMatrix matrix, Vector startingVector, double tolerance = 0.0001)
    {
        var iteration = 1;
        var quotients = new List<double>();
        var xVector = startingVector.Clone();
        var inverseEliminator = new GaussJordanEliminator();
        
        while (true)
        {
            var shift = xVector.DotProduct(matrix * xVector) / xVector.DotProduct(xVector);
            var identityMatrix = MatrixExtensions.CreateIdentityMatrix(matrix.Rows);
            var y = inverseEliminator.Reduce(matrix - shift * identityMatrix) * xVector;
            xVector = y / y.NormalizeInfinity();

            if (quotients.Count > 1 && Math.Abs(quotients[iteration - 2] - shift) < tolerance)
            {
                break;
            }
            quotients.Add(shift);

            iteration++;
        }
        return (quotients[quotients.Count - 1], iteration);
    }
}
