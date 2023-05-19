using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class SquareMatrix
{
    public double[][] Matrix { get; set; }
    public int Size { get => Matrix.Length; }

    public double this[int column, int row]
    {
        get { return Matrix[row][column]; }
        set { Matrix[row][column] = value; }
    }

    public SquareMatrix(double[][] matrix)
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
