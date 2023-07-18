using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

/// <summary>
/// This class uses the Hu = (I - 2vvT/vTv)u formula to perform householder transforms
/// </summary>
public class HouseholderSolver : ISolver
{
    public double[] Solve(IMatrix matrix, double[] parameters)
    {
        // Vector class is heavily used here to simplify logic
        // so it looks very object oriented and simple
        var parameterVector = new Vector(parameters);
        for (var iteration = 0; iteration < matrix.Columns; iteration++)
        {
            var alpha = -matrix.SliceColumn(iteration, iteration).Normalize();
            var vector = matrix.SliceColumn(iteration, iteration) - alpha * MatrixExtensions.CreateUnitVector(matrix.Rows, iteration);

            for (var column = iteration; column < matrix.Columns; column++)
            {
                var currentColumn = matrix.SliceColumn(column);
                var transform = ApplyHouseholderTransform(currentColumn, vector);
                matrix.ReplaceColumn(transform, column);
            }

            parameterVector = ApplyHouseholderTransform(parameterVector, vector);
        }

        var finishedValues = new double[matrix.Columns][];
        for (var row = 0; row < matrix.Columns; row++)
        {
            finishedValues[row] = matrix.Matrix[row];
        }
        var squareVector = new SquareMatrix(finishedValues);

        var backwardSolver = new BackwardSubstitutionSolver();
        return backwardSolver.Solve(squareVector, parameterVector.Take(matrix.Columns).ToArray());
    }

    public IMatrix Transform(RectangularMatrix origMatrix)
    {
        var matrix = origMatrix.Clone();
        for (var iteration = 0; iteration < matrix.Columns; iteration++)
        {          
            var alpha = -matrix.SliceColumn(iteration, iteration).Normalize();
            var vector = matrix.SliceColumn(iteration, iteration) - alpha * MatrixExtensions.CreateUnitVector(matrix.Rows, iteration);

            for (var column = iteration; column < matrix.Columns; column++)
            {
                var currentColumn = matrix.SliceColumn(column);
                var transform = ApplyHouseholderTransform(currentColumn, vector);
                matrix.ReplaceColumn(transform, column);
            }

            Console.WriteLine($"Iteration: {iteration}");
            Console.WriteLine(matrix);
            Console.WriteLine();
        }

        return matrix;
    }

    private Vector ApplyHouseholderTransform(Vector currentColumn, Vector vector)
    {
        return currentColumn - 2 * (vector.DotProduct(currentColumn) / vector.DotProduct(vector)) * vector;
    }
}
