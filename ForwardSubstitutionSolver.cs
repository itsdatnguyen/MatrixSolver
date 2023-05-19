using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class ForwardSubstitutionSolver : ISolver
{
    public double[] Solve(SquareMatrix matrix, double[] parameters)
    {
        AssertLowerTriangularSystem(matrix);
        if (matrix.Size != parameters.Length)
        {
            throw new ArgumentException($"{nameof(parameters)} does not match the matrix size.");
        }

        var solvedValues = new double[matrix.Size];
        for (var row = 0; row < matrix.Size; row++)
        {
            var rowSum = parameters[row];
            for (var col = 0; col < row; col++)
            {
                rowSum -= solvedValues[col] * matrix[col, row];
            }
            solvedValues[row] = rowSum / matrix[row, row];
        }

        return solvedValues;
    }

    private void AssertLowerTriangularSystem(SquareMatrix matrix)
    {
        for (var column = 1; column < matrix.Size; column++)
        {
            for (var row = 0; row < column; row++)
            {
                if (matrix[column, row] != 0d)
                {
                    throw new ArgumentException($"{nameof(ForwardSubstitutionSolver)} expects a lower triangular system!");
                }
            }
        }
    }
}
