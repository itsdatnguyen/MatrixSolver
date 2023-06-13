using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class BackwardSubstitutionSolver : ISolver
{
    public decimal[] Solve(IMatrix matrix, decimal[] parameters)
    {
        AssertUpperTriangularSystem(matrix);
        if (matrix.Rows != parameters.Length)
        {
            throw new ArgumentException($"{nameof(parameters)} does not match the matrix size.");
        }

        var solvedValues = new decimal[matrix.Columns];
        for (var row = matrix.Rows - 1; row >= 0; row--)
        {
            var rowSum = parameters[row];
            for (var col = matrix.Columns - 1; col > row; col--)
            {
                rowSum -= solvedValues[col] * matrix[col, row];
            }
            solvedValues[row] = rowSum / matrix[row, row];
        }

        return solvedValues;
    }

    private void AssertUpperTriangularSystem(IMatrix matrix)
    {
        for (var row = 1; row < matrix.Rows; row++)
        {
            for (var column = 0; column < row; column++)
            {
                if (matrix[column, row] != 0M)
                {
                    throw new ArgumentException($"{nameof(BackwardSubstitutionSolver)} expects a upper triangular system!");
                }
            }
        }
    }
}
