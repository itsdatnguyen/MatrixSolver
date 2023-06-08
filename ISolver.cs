namespace MatrixSolver;

public interface ISolver
{
    decimal[] Solve(SquareMatrix matrix, decimal[] parameters);
}