using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class HouseholderQRIterator
{
    public (Vector eigenValues, int iterations) SolveEigenvalues(RectangularMatrix sourceMatrix, double tolerance = 0.0001)
    {
        var qrTransformer = new HouseholderTransformer();
        var matrix = sourceMatrix.Clone();
        var iteration = 1;
        var eigenValueHistory = new List<Vector>();
        var eigenValueCount = Math.Min(matrix.Columns, matrix.Rows);
        var shouldContinue = true;

        while (shouldContinue)
        {
            var (qMatrix, rMatrix) = qrTransformer.Transform(matrix);
            matrix = rMatrix * qMatrix;

            var eigenValues = new double[eigenValueCount];
            for (var i = 0; i < eigenValueCount; i++)
            {
                eigenValues[i] = matrix[i, i];
            }

            if (eigenValueHistory.Any())
            {
                for (var i = 0; i < eigenValues.Length; i++)
                {
                    if (Math.Abs(eigenValues[i] - eigenValueHistory[iteration - 2].Values[i]) < tolerance)
                    {
                        shouldContinue = false;
                    }
                }
            }
            eigenValueHistory.Add(new Vector(eigenValues));

            iteration++;
        }
        return (eigenValueHistory[eigenValueHistory.Count - 1], iteration);
    }
}
