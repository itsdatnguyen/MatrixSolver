namespace MatrixSolver;

public interface ISolver
{
    decimal[] Solve(IMatrix matrix, decimal[] parameters);
}