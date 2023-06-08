using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class HilbertGenerator
{
    public (SquareMatrix Matrix, decimal[] Parameters) GeneratorHb(int size)
    {
        if (size < 1)
        {
            throw new ArgumentException($"{nameof(size)} must be greater than 0");
        }

        var matrix = new decimal[size][];

        for (var row = 0; row < size; row++)
        {
            var currentRow = new decimal[size];
            for (var col = 0; col < size; col++)
            {
                currentRow[col] = 1M / ((row + 1) + (col + 1) - 1);
            }
            matrix[row] = currentRow;
        }

        var parameters = new decimal[size];
        for (var i = 0; i < size; i++)
        {
            parameters[i] = 1;
        }

        return (new SquareMatrix(matrix), parameters);
    }
}
