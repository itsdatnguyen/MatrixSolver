using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class GaussEliminationSolver : ISolver
{
    public decimal[] Solve(SquareMatrix matrix, decimal[] parameters)
    {
        if (matrix.Size != parameters.Length)
        {
            throw new ArgumentException($"{nameof(parameters)} does not match the matrix size.");
        }

        for (var row = 1; row < matrix.Size; row++)
        {
            for (var col = 0; col < row; col++) 
            { 
                if (matrix[col, row] == 0) 
                {
                    continue;
                }

                // find first upper row with non-zero value
                var multiplierRow = 0;
                var multiplier = 0M;
                for (var rowFind = row - 1; rowFind >= 0; rowFind--)
                {
                    var rowFindValue = matrix[col, rowFind];
                    if (rowFindValue != 0)
                    {
                        multiplierRow = rowFind;
                        multiplier = matrix[col, row] / rowFindValue;
                        break;
                    }
                }

                // apply transformations to all items in the current row
                for (var colApplyTransform = 0; colApplyTransform < matrix.Size; colApplyTransform++)
                {
                    matrix[colApplyTransform, row] = matrix[colApplyTransform, row] - multiplier * matrix[colApplyTransform, multiplierRow];
                }
                parameters[row] = parameters[row] - multiplier * parameters[multiplierRow];
            }
        }

        // run backwards substitution
        var backwardSolver = new BackwardSubstitutionSolver();
        return backwardSolver.Solve(matrix, parameters);
    }
}
