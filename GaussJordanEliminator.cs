using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class GaussJordanEliminator
{
    public RectangularMatrix Reduce(RectangularMatrix originalMatrix)
    {
        var matrix = originalMatrix.Clone();
        var identityMatrix = MatrixExtensions.CreateIdentityMatrix(matrix.Rows);
        for (var col = 0; col < matrix.Rows; col++)
        {
            for (var row = 0; row < matrix.Rows; row++)
            {
                if (row == col)
                {
                    continue;
                }

                if (matrix[col, col] == 0)
                {
                    // if zero, we cannot divide by 0, so pivot instead
                    var tempMatrixRow = matrix.Matrix[row];
                    matrix.Matrix[row] = matrix.Matrix[col];
                    matrix.Matrix[col] = tempMatrixRow;

                    var tempIdentityRow = identityMatrix.Matrix[row];
                    identityMatrix.Matrix[row] = identityMatrix.Matrix[col];
                    identityMatrix.Matrix[col] = tempIdentityRow;
                    continue;
                }

                var multiplier = matrix[col, row] / matrix[col, col];
                ApplyRowTransformation(col, row, multiplier);
            }
        }

        for (var diagIndex = 0; diagIndex < matrix.Rows; diagIndex++)
        {
            if (Math.Round(matrix[diagIndex, diagIndex], 8) != 1D)
            {
                var multiplier = (matrix[diagIndex, diagIndex] - 1D) / matrix[diagIndex, diagIndex];
                ApplyRowTransformation(diagIndex, diagIndex, multiplier);
            }
        }

        void ApplyRowTransformation(int sourceRow, int targetRow, double multiplier)
        {
            for (var col = 0; col < matrix.Rows; col++)
            {
                matrix[col, targetRow] = matrix[col, targetRow] - matrix[col, sourceRow] * multiplier;
                identityMatrix[col, targetRow] = identityMatrix[col, targetRow] - identityMatrix[col, sourceRow] * multiplier;
            }
        }

        return identityMatrix;
    }
}
