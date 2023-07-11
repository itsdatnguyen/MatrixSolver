using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class HouseholderTransformer
{
    public (RectangularMatrix q, RectangularMatrix r) Transform(RectangularMatrix sourceMatrix)
    {
        var matrixR = sourceMatrix.Clone();
        var matricesQ = new List<RectangularMatrix>();
        for (var iteration = 0; iteration < matrixR.Columns; iteration++)
        {
            var alpha = matrixR.SliceColumn(iteration, iteration).Normalize();
            var vector = matrixR.SliceColumn(iteration, iteration) - alpha * MatrixExtensions.CreateUnitVector(matrixR.Rows, iteration);
            var transformationVector = vector / vector.Normalize();
            var identity = MatrixExtensions.CreateIdentityMatrix(Math.Max(matrixR.Rows, matrixR.Columns));
            var q = identity - 2 * transformationVector * transformationVector;
            matrixR = q * matrixR;

            matricesQ.Add(q);
        }

        var enumerator = matricesQ.GetEnumerator();
        enumerator.MoveNext();
        var matrixQ = enumerator.Current;
        while (enumerator.MoveNext())
        {
            matrixQ *= enumerator.Current;
        }
        return (matrixQ, matrixR);
    }
}
