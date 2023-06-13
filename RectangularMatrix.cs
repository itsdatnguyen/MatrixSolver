using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class RectangularMatrix : IMatrix
{
    public decimal[][] Matrix { get; set; }
    public int Rows { get => Matrix.Length; }
    public int Columns { get => Matrix[0].Length; }

    public decimal this[int column, int row]
    {
        get { return Matrix[row][column]; }
        set { Matrix[row][column] = value; }
    }

    public RectangularMatrix(decimal[][] matrix)
    {
        Matrix = matrix;
    }

    public decimal[] Calculate(decimal[] parameters)
    {
        if (parameters.Length != Columns)
        {
            throw new ArgumentException($"{parameters} length does not match matrix size");
        }

        var calculatedValues = new decimal[parameters.Length];
        for (var row = 0; row < Rows; row++)
        {
            var total = 0M;
            for (var col = 0; col < Columns; col++)
            {
                total += Matrix[row][col] * parameters[col];
            }
            calculatedValues[row] = total;
        }

        return calculatedValues;
    }

    public decimal CalculateConditionNumberInfinite()
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
            if (row + 1 < Columns)
            {
                builder.AppendLine();
            }
        }
        return builder.ToString();
    }
}
