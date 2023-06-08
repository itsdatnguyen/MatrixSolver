using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class SquareMatrix
{
    public decimal[][] Matrix { get; set; }
    public int Size { get => Matrix.Length; }

    public decimal this[int column, int row]
    {
        get { return Matrix[row][column]; }
        set { Matrix[row][column] = value; }
    }

    public SquareMatrix(decimal[][] matrix)
    {
        var size = matrix.Length;
        for (int i = 0; i < size; i++)
        {
            if (matrix[i].Length - 1 > size)
            {
                throw new ArgumentException("Matrix is not a square.");
            }
        }
        Matrix = matrix;
    }

    public decimal[] Calculate(decimal[] parameters)
    {
        if (parameters.Length != Size)
        {
            throw new ArgumentException($"{parameters} length does not match matrix size");
        }

        var calculatedValues = new decimal[parameters.Length];
        for (var row = 0; row < Size; row++)
        {
            var total = 0M;
            for (var col = 0; col < Size; col++)
            {
                total += Matrix[row][col] * parameters[col];
            }
            calculatedValues[row] = total;
        }

        return calculatedValues;
    }

    public decimal CalculateConditionNumberInfinite()
    {
        return Enumerable.Range(0, Size)
            .Max(row => Enumerable.Range(0, Size).Sum(col => Math.Abs(Matrix[row][col])));
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        for (var row = 0; row < Size; row++)
        {
            for (var col = 0; col < Size; col++)
            {
                builder.Append(Matrix[row][col] + " ");
            }
            if (row + 1 < Size)
            {
                builder.AppendLine();
            }
        }
        return builder.ToString();
    }
}
