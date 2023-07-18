using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

/// <summary>
/// <para>
/// This preferred implementation of householder uses the P = I - 2uuT formula
/// where u = v/||v||2
/// </para>
/// <para>
/// This formula captures the final orthogonal Q matrix and the final upper triangular matrix R.
/// </para>
/// </summary>
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

            Console.WriteLine($"Iteration {iteration}");
            Console.WriteLine("Q Matrix (orthogonal matrix)");
            Console.WriteLine(q);
            Console.WriteLine();
            Console.WriteLine("R Matrix (upper triangular matrix)");
            Console.WriteLine(matrixR);
            Console.WriteLine();

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
