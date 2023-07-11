using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class GaussJordanEliminator
{
    public IMatrix Reduce(SquareMatrix matrix)
    {
        var identityMatrix = MatrixExtensions.CreateIdentityMatrix(matrix.Size);
        for (var col = 0; col < matrix.Size; col++)
        {
            for (var row = 0; row < matrix.Size; row++)
            {
                if (row == col)
                {
                    continue;
                }
                var multiplier = matrix[col, row] / matrix[col, col];
                ApplyRowTransformation(col, row, multiplier);
            }
        }

        for (var diagIndex = 0; diagIndex < matrix.Size; diagIndex++)
        {
            if (Math.Round(matrix[diagIndex, diagIndex], 8) != 1D)
            {
                var multiplier = (matrix[diagIndex, diagIndex] - 1D) / matrix[diagIndex, diagIndex];
                ApplyRowTransformation(diagIndex, diagIndex, multiplier);
            }
        }

        void ApplyRowTransformation(int sourceRow, int targetRow, double multiplier)
        {
            for (var col = 0; col < matrix.Size; col++)
            {
                matrix[col, targetRow] = matrix[col, targetRow] - matrix[col, sourceRow] * multiplier;
                identityMatrix[col, targetRow] = identityMatrix[col, targetRow] - identityMatrix[col, sourceRow] * multiplier;
            }
        }

        return identityMatrix;
    }
}
