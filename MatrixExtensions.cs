using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public static class MatrixExtensions
{
    public static Vector SliceColumn (this IMatrix matrix, int column, int start = 0)
    {
        var values = new double[matrix.Rows];
        for (var row = start; row < matrix.Rows; row++)
        {
            values[row] = matrix[column, row];
        }
        return new Vector(values);
    }

    public static Vector SliceColumn(this IMatrix matrix, int column, int start, int end)
    {
        var values = new double[matrix.Rows];
        for (var row = start; row < end; row++)
        {
            values[row] = matrix[column, row];
        }
        return new Vector(values);
    }

    public static Vector CreateUnitVector(int length, int index)
    {
        var values = Enumerable.Repeat(0D, length).ToArray();
        values[index] = 1;
        return new Vector(values);
    }

    public static RectangularMatrix CreateIdentityMatrix(int size)
    {
        var matrix = new double[size][];
        for (var row = 0; row < size; row++)
        {
            matrix[row] = new double[size];
        }

        for (var i = 0; i < size; i++)
        {
            matrix[i][i] = 1D;
        }

        return new RectangularMatrix(matrix);
    }

    public static RectangularMatrix CreateFromColumns(List<Vector> columns)
    {
        var rows = columns.Count;
        var data = new double[rows][];
        for (var row = 0; row < rows; row++)
        {
            data[row] = new double[columns.Count];
            for (var col = 0; col < columns.Count; col++)
            {
                data[row][col] = columns[col].Values[row];
            }
        }

        return new RectangularMatrix(data);
    }

    public static IMatrix Transpose(this IMatrix matrix)
    {
        int w = matrix.Columns;
        int h = matrix.Rows;

        double[][] result = new double[w][];
        for (int i = 0; i < matrix.Columns; i++)
        {
            result[i] = new double[h];
        }

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                result[i][j] = matrix[i, j];
            }
        }

        return new RectangularMatrix(result);
    }
}
