namespace MatrixSolver;

public interface ISolver
{
    double[] Solve(IMatrix matrix, double[] parameters);
}