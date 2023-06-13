using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class RectangularMatrix : IMatrix
{
    public double[][] Matrix { get; set; }
    public int Rows { get => Matrix.Length; }
    public int Columns { get => Matrix[0].Length; }

    public double this[int column, int row]
    {
        get { return Matrix[row][column]; }
        set { Matrix[row][column] = value; }
    }

    public RectangularMatrix(double[][] matrix)
    {
        Matrix = matrix;
    }

    /// <summary>
    /// This assumes all vectors are column vectors
    /// </summary>
    public RectangularMatrix(IEnumerable<Vector> vectors)
    {
        var vectorArray = vectors.ToArray();
        var cols = vectorArray.Length;
        var rows = vectorArray[0].Values.Length;

        var matrix = new double[rows][];
        for (var row = 0; row < rows; row++)
        {
            var rowArray = new double[cols];
            for (var col = 0; col < cols; col++)
            {
                rowArray[col] = vectorArray[col].Values[row];
            }
            matrix[row] = rowArray;
        }

        Matrix = matrix;
    }

    public double[] Calculate(double[] parameters)
    {
        if (parameters.Length != Columns)
        {
            throw new ArgumentException($"{parameters} length does not match matrix size");
        }

        var calculatedValues = new double[parameters.Length];
        for (var row = 0; row < Rows; row++)
        {
            var total = 0D;
            for (var col = 0; col < Columns; col++)
            {
                total += Matrix[row][col] * parameters[col];
            }
            calculatedValues[row] = total;
        }

        return calculatedValues;
    }

    public double CalculateConditionNumberInfinite()
    {
        return Enumerable.Range(0, Rows)
            .Max(row => Enumerable.Range(0, Columns).Sum(col => Math.Abs(Matrix[row][col])));
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        for (var row = 0; row < Rows; row++)
        {
            for (var col = 0; col < Columns; col++)
            {
                builder.Append(Matrix[row][col] + " ");
            }
            if (row + 1 < Rows)
            {
                builder.AppendLine();
            }
        }
        return builder.ToString();
    }

    public void ReplaceColumn(Vector vector, int column)
    {
        for (var row = 0; row < Rows; row++)
        {
            Matrix[row][column] = vector.Values[row];
        }
    }
}
