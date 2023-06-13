namespace MatrixSolver;

internal class Program
{
    static void Main(string[] args)
    {
        var altitudes = new double[][]
        {
            new double[] { 1, 0, 0, 0 },
            new double[] { 0, 1, 0, 0 },
            new double[] { 0, 0, 1, 0 },
            new double[] { 0, 0, 0, 1 },
            new double[] { 1, -1, 0, 0 },
            new double[] { 1, 0, -1, 0 },
            new double[] { 1, 0, 0, -1 },
            new double[] { 0, 1, -1, 0 },
            new double[] { 0, 1, 0, -1 },
            new double[] { 0, 0, 1, -1 },
        };

        var measurements = new double[]
        {
            2.95, 1.74, -1.45, 1.32, 1.23, 4.45, 1.61, 3.21, 0.45, -2.75
        };

        SolveWithGaussElimination();
        SolveBackwardSystems();
        SolveForwardSystems();
    }

    static void RunHilbertGenerator()
    {
        var hilbertGenerator = new HilbertGenerator();
        var solver = new GaussEliminationSolver();

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine(string.Format("|{0,10}|{1,10}|{2,10}|", "n", "‖𝒓‖∞", "Cond(H)"));
        foreach (var i in Enumerable.Range(1, 22))
        {
            // H
            var (matrix, trueX) = hilbertGenerator.GeneratorHb(i);

            // b
            var calculatedValues = matrix.Calculate(trueX);

            var approximateX = solver.Solve(matrix, calculatedValues);
            var backwardError = trueX.Select((value, i) => (approximateX[i] - value) / value);

            (matrix, _) = hilbertGenerator.GeneratorHb(i);
            var calculatedValuesWithApproximateX = matrix.Calculate(approximateX);
            var residuals = calculatedValues.Select((value, i) => calculatedValuesWithApproximateX[i] - value);

            Console.WriteLine($"---------------------------- N={i} ----------------------------");
            //Console.WriteLine(string.Format("|{0,10}|{1,10}|{2,10}|", i, Math.Round(residuals.Sum(r => Math.Pow((double)r, 2)), 5), Math.Round(matrix.CalculateConditionNumberInfinite(), 5)));
            Console.WriteLine("Approximate X (X̂):");
            PrintValues(approximateX);
            Console.WriteLine();
            Console.WriteLine("Absolute Error (𝚫𝒙):");
            PrintValues(trueX.Select((value, i) => (approximateX[i] - value)));
            Console.WriteLine();
            Console.WriteLine("Relative Error:");
            PrintValues(backwardError.Select(e => decimal.Round(Math.Abs(e * 100), 0) + "%"));
            Console.WriteLine();
            Console.WriteLine("Residuals (r):");
            PrintValues(residuals);
            Console.WriteLine();
        }
    }

    static void SolveForwardSystems()
    {
        var solver = new ForwardSubstitutionSolver();

        RunOutput
        (
            solver,
            new SquareMatrix(
                new decimal[][]
                {
                    new decimal[] { 4, 0, 0 },
                    new decimal[] { 2, -2, 0 },
                    new decimal[] { 1, 3, 4 }
                }
            ),
            new decimal[] { 1, -2, 19 }
        );

        RunOutput
        (
            solver,
            new SquareMatrix(
                new decimal[][]
                {
                    new decimal[] { -1, 0, 0 },
                    new decimal[] { 3, -2, 0 },
                    new decimal[] { -2, 1, 4 }
                }
            ),
            new decimal[] { 1, -7, 8 }
        );
    }

    static void SolveBackwardSystems()
    {
        var solver = new BackwardSubstitutionSolver();

        RunOutput
        (
            solver,
            new SquareMatrix(
                new decimal[][]
                {
                    new decimal[] { 1, 3, 4 },
                    new decimal[] { 0, -2, 2 },
                    new decimal[] { 0, 0, 4 }
                }
            ),
            new decimal[] { 11, -2, 4 }
        );

        RunOutput
        (
            solver,
            new SquareMatrix(
                new decimal[][]
                {
                    new decimal[] { 1, 2, -4 },
                    new decimal[] { 0, -2, -1 },
                    new decimal[] { 0, 0, 3 }
                }
            ),
            new decimal[] { 7, -7, 9 }
        );
    }

    private static void SolveWithGaussElimination()
    {
        var solver = new GaussEliminationSolver();

        RunOutput
        (
            solver,
            new SquareMatrix(
                new decimal[][]
                {
                    new decimal[] { 1, 2, 1, -1 },
                    new decimal[] { 3, 2, 4, 4 },
                    new decimal[] { 4, 4, 3, 4 },
                    new decimal[] { 2, 0, 1, 5 },
                }
            ),
            new decimal[] { 5, 16, 22, 15 }
        );

        RunOutput
        (
            solver,
            new SquareMatrix(
                new decimal[][]
                {
                    new decimal[] { 1, 2, 2 },
                    new decimal[] { 4, 4, 2 },
                    new decimal[] { 4, 6, 4 }
                }
            ),
            new decimal[] { 3, 6, 10 }
        );
    }

    static private void RunOutput(ISolver solver, SquareMatrix matrix, decimal[] parameters)
    {
        Console.WriteLine($"Running {solver.GetType().Name}");
        Console.WriteLine($"Parameters: {string.Join(",", parameters)}");
        Console.WriteLine(matrix.ToString());

        var solvedValues = solver.Solve(matrix, parameters);
        Console.WriteLine($"Solved values: {string.Join(",", solvedValues)}");
        Console.WriteLine();
    }

    static void PrintValues(IEnumerable<string> values)
    {
        foreach (var value in values)
        {
            Console.WriteLine(value);
        }
    }

    static void PrintValues(IEnumerable<decimal> values)
    {
        foreach (var value in values)
        {
            Console.WriteLine(value);
        }
    }
}