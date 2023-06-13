using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class HilbertGenerator
{
    public (SquareMatrix Matrix, double[] Parameters) GeneratorHb(int size)
    {
        if (size < 1)
        {
            throw new ArgumentException($"{nameof(size)} must be greater than 0");
        }

        var matrix = new double[size][];

        for (var row = 0; row < size; row++)
        {
            var currentRow = new double[size];
            for (var col = 0; col < size; col++)
            {
                currentRow[col] = 1D / ((row + 1) + (col + 1) - 1);
            }
            matrix[row] = currentRow;
        }

        var parameters = new double[size];
        for (var i = 0; i < size; i++)
        {
            parameters[i] = 1;
        }

        return (new SquareMatrix(matrix), parameters);
    }
}
