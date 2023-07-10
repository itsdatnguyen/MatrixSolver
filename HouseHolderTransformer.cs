using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MatrixSolver;

public class HouseholderTransformer
{
    public IMatrix Transform(IMatrix matrix)
    {
        var matricesQ = new List<RectangularMatrix>();
        for (var iteration = 0; iteration < matrix.Columns; iteration++)
        {
            var alpha = -matrix.SliceColumn(iteration, iteration).Normalize();
            var vector = matrix.SliceColumn(iteration, iteration) - alpha * MatrixExtensions.CreateUnitVector(matrix.Rows, iteration);

            var identityMatrix = MatrixExtensions.CreateIdentityMatrix(matrix.Columns, matrix.Rows);
            for (var column = iteration; column < matrix.Columns; column++)
            {
                var currentColumn = matrix.SliceColumn(column);
                var transform = ApplyHouseholderTransform(currentColumn, vector);
                matrix.ReplaceColumn(transform, column);

                var currentColumnQ = identityMatrix.SliceColumn(column);
                var transformQ = ApplyHouseholderTransform(currentColumnQ, vector);
                identityMatrix.ReplaceColumn(transformQ, column);
            }
            matricesQ.Add(identityMatrix);
        }

        var enumerator = matricesQ.GetEnumerator();
        enumerator.MoveNext();
        var matrixQ = enumerator.Current;
        while (enumerator.MoveNext())
        {
            matrixQ *= enumerator.Current;
        }
        return matrixQ;
    }

    private Vector ApplyHouseholderTransform(Vector currentColumn, Vector vector)
    {
        return currentColumn - 2 * (vector.DotProduct(currentColumn) / vector.DotProduct(vector)) * vector;
    }  
}
