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

    public static Vector CreateUnitVector(int length, int index)
    {
        var values = Enumerable.Repeat(0D, length).ToArray();
        values[index] = 1;
        return new Vector(values);
    }
}
