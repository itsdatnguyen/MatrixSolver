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
    public IMatrix Transform(RectangularMatrix matrix)
    {
        var matricesQ = new List<RectangularMatrix>();
        for (var iteration = 0; iteration < matrix.Columns; iteration++)
        {
            var alpha = matrix.SliceColumn(iteration, iteration).Normalize();
            var vector = matrix.SliceColumn(iteration, iteration) - alpha * MatrixExtensions.CreateUnitVector(matrix.Rows, iteration);
            var transformationVector = vector / vector.Normalize();
            var identity = MatrixExtensions.CreateIdentityMatrix(Math.Max(matrix.Rows, matrix.Columns));
            var q = identity - 2 * transformationVector * transformationVector;
            matrix = q * matrix;

            Console.WriteLine($"Iteration {iteration}");
            Console.WriteLine("Q Matrix");
            Console.WriteLine(q);
            Console.WriteLine();
            Console.WriteLine("Transformed Matrix");
            Console.WriteLine(matrix);
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
        return matrixQ;
    }
}
