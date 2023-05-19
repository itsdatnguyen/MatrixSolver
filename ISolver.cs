namespace MatrixSolver;

public interface ISolver
{
    double[] Solve(SquareMatrix matrix, double[] parameters);
}