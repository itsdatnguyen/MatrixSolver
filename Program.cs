namespace MatrixSolver;

internal class Program
{
    /// <example>
    /// Example Output:
    /// 
    /// Running GaussEliminationSolver
    /// Parameters: 5,16,22,15
    /// 1 2 1 -1
    /// 3 2 4 4
    /// 4 4 3 4
    /// 2 0 1 5
    /// Solved values: 16,-6,-2,-3
    /// 
    /// Running GaussEliminationSolver
    /// Parameters: 3,6,10
    /// 1 2 2
    /// 4 4 2
    /// 4 6 4
    /// Solved values: -1,3,-1
    /// 
    /// Running ForwardSubstitutionSolver
    /// Parameters: 1,-2,19
    /// 4 0 0
    /// 2 -2 0
    /// 1 3 4
    /// Solved values: 0.25,1.25,3.75
    /// 
    /// Running ForwardSubstitutionSolver
    /// Parameters: 1,-7,8
    /// -1 0 0
    /// 3 -2 0
    /// -2 1 4
    /// Solved values: -1,2,1
    /// 
    /// Running BackwardSubstitutionSolver
    /// Parameters: 11,-2,4
    /// 1 3 4
    /// 0 -2 2
    /// 0 0 4
    /// Solved values: 1,2,1
    /// 
    /// Running BackwardSubstitutionSolver
    /// Parameters: 7,-7,9
    /// 1 2 -4
    /// 0 -2 -1
    /// 0 0 3
    /// Solved values: 15,2,3
    /// </example>
    static void Main(string[] args)
    {
        SolveWithGaussElimination();
        SolveForwardSystems();
        SolveBackwardSystems();
    }

    static void SolveForwardSystems()
    {
        var solver = new ForwardSubstitutionSolver();

        RunOutput
        (
            solver,
            new SquareMatrix(
                new double[][]
                {
                    new double[] { 4, 0, 0 },
                    new double[] { 2, -2, 0 },
                    new double[] { 1, 3, 4 }
                }
            ),
            new double[] { 1, -2, 19 }
        );

        RunOutput
        (
            solver,
            new SquareMatrix(
                new double[][]
                {
                    new double[] { -1, 0, 0 },
                    new double[] { 3, -2, 0 },
                    new double[] { -2, 1, 4 }
                }
            ),
            new double[] { 1, -7, 8 }
        );
    }

    static void SolveBackwardSystems()
    {
        var solver = new BackwardSubstitutionSolver();

        RunOutput
        (
            solver,
            new SquareMatrix(
                new double[][]
                {
                    new double[] { 1, 3, 4 },
                    new double[] { 0, -2, 2 },
                    new double[] { 0, 0, 4 }
                }
            ),
            new double[] { 11, -2, 4 }
        );

        RunOutput
        (
            solver,
            new SquareMatrix(
                new double[][]
                {
                    new double[] { 1, 2, -4 },
                    new double[] { 0, -2, -1 },
                    new double[] { 0, 0, 3 }
                }
            ),
            new double[] { 7, -7, 9 }
        );
    }

    private static void SolveWithGaussElimination()
    {
        var solver = new GaussEliminationSolver();

        RunOutput
        (
            solver,
            new SquareMatrix(
                new double[][]
                {
                    new double[] { 1, 2, 1, -1 },
                    new double[] { 3, 2, 4, 4 },
                    new double[] { 4, 4, 3, 4 },
                    new double[] { 2, 0, 1, 5 },
                }
            ),
            new double[] { 5, 16, 22, 15 }
        );

        RunOutput
        (
            solver,
            new SquareMatrix(
                new double[][]
                {
                    new double[] { 1, 2, 2 },
                    new double[] { 4, 4, 2 },
                    new double[] { 4, 6, 4 }
                }
            ),
            new double[] { 3, 6, 10 }
        );
    }

    static private void RunOutput(ISolver solver, SquareMatrix matrix, double[] parameters)
    {
        Console.WriteLine($"Running {solver.GetType().Name}");
        Console.WriteLine($"Parameters: {string.Join(",", parameters)}");
        Console.WriteLine(matrix.ToString());

        var solvedValues = solver.Solve(matrix, parameters);
        Console.WriteLine($"Solved values: {string.Join(",", solvedValues)}");
        Console.WriteLine();
    }
}