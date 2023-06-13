namespace MatrixSolver;

internal class Program
{
    /// Note that many of the numbers in the output are impossibly small
    /// ex: -3.3306690738754696E-16
    /// 
    /// Note that I used the double data type here instead of a float data type.
    /// 
    /// <example>
    /// Input:
    /// Parameters: 2.95,1.74,-1.45,1.32,1.23,4.45,1.61,3.21,0.45,-2.75
    /// 
    /// 1 0 0 0
    /// 0 1 0 0
    /// 0 0 1 0
    /// 0 0 0 1
    /// 1 -1 0 0
    /// 1 0 -1 0
    /// 1 0 0 -1
    /// 0 1 -1 0
    /// 0 1 0 -1
    /// 0 0 1 -1
    /// 
    /// Output:
    /// -2 0.5 0.5 0.5
    /// 0 -1.9364916731037076 0.6454972243679027 0.6454972243679027
    /// 0 0 -1.8257418583505536 0.9128709291752767
    /// 0 0 0 -1.5811388300841895
    /// 0 -3.3306690738754696E-16 -3.469446951953614E-18 -3.469446951953614E-18
    /// 0 5.551115123125783E-17 -2.220446049250313E-16 -1.3877787807814457E-17
    /// 0 5.551115123125783E-17 5.551115123125783E-17 -1.1102230246251565E-16
    /// 0 3.3306690738754696E-16 -2.220446049250313E-16 -6.938893903907228E-18
    /// 0 3.3306690738754696E-16 5.551115123125783E-17 -1.1102230246251565E-16
    /// 0 0 2.220446049250313E-16 -1.1102230246251565E-16
    /// 
    /// Actual values(X): 2.95,1.74,-1.45,1.32
    /// Solved values(X̂): 2.9600000000000004,1.7460000000000002,-1.46,1.3139999999999998
    /// Absolute Backwards error(∆x) : 0.010000000000000231,0.006000000000000227,-0.010000000000000009,-0.006000000000000227
    /// </example>
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var actualX = new double[]
        {
            2.95, 1.74, -1.45, 1.32
        };
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

        var matrix = new RectangularMatrix(altitudes);
        var solver = new HouseholderSolver();

        Console.WriteLine("Input:");
        Console.WriteLine($"Parameters: {string.Join(",", measurements)}");
        Console.WriteLine();
        Console.WriteLine(matrix.ToString());
        Console.WriteLine();

        var solvedValues = solver.Solve(matrix, measurements);
        Console.Write("Output:");
        Console.WriteLine();
        Console.WriteLine(matrix.ToString());
        Console.WriteLine();
        Console.WriteLine($"Actual values (X): {string.Join(",", actualX)}");
        Console.WriteLine($"Solved values (X̂): {string.Join(",", solvedValues)}");
        var backwardsError = solvedValues.Select((value, index) => value - actualX[index]);
        Console.WriteLine($"Absolute Backwards error (∆x): {string.Join(",", backwardsError)}");
        Console.WriteLine();

        //SolveHouseholderSystems();
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
            PrintValues(backwardError.Select(e => double.Round(Math.Abs(e * 100), 0) + "%"));
            Console.WriteLine();
            Console.WriteLine("Residuals (r):");
            PrintValues(residuals);
            Console.WriteLine();
        }
    }

    static void SolveHouseholderSystems()
    {
        var solver = new HouseholderSolver();

        RunOutput
        (
            solver,
            new RectangularMatrix(
                new double[][]
                {
                    new double[] { 1, 0, 0 },
                    new double[] { 0, 1, 0 },
                    new double[] { 0, 0, 1 },
                    new double[] { -1, 1, 0 },
                    new double[] { -1, 0, 1 },
                    new double[] { 0, -1, 1 }
                }
            ),
            new double[]
            {
                1237, 1941, 2417, 711, 1177, 475
            }
        );
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

    static private void RunOutput(ISolver solver, IMatrix matrix, double[] parameters)
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

    static void PrintValues(IEnumerable<double> values)
    {
        foreach (var value in values)
        {
            Console.WriteLine(value);
        }
    }
}