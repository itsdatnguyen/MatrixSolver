
namespace MatrixSolver;

internal class Program
{
    /// Sample output
    /// <example>
    /// |#    |Starting Eigen Vector         |Eigen Value                   |Number of iterations|
    /// |0    |1,0.5,1,2                     |4.000021758462922             |6                   |
    /// |1    |999,-999,5,5                  |2.0000194591485543            |7                   |
    /// |2    |5,23.44,-42,-57               |2.999974952296109             |6                   |
    /// |3    |-52300,22300,-42324,-125      |2.999974952296155             |5                   |
    /// 
    /// The eigen values are: 3.997550682007448,2.989222588421614,2.0132368304830472,0.9999898990878896
    /// The number of iterations for the convergence is: 6
    /// </example>
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var matrix = new RectangularMatrix(
            new double[][]
            {
                new double[] { 2.9766, 0.3945, 0.4198, 1.1159 },
                new double[] { 0.3945, 2.7328, -0.3097, 0.1129 },
                new double[] { 0.4198, -0.3097, 2.5675, 0.6079 },
                new double[] { 1.1159, 0.1129, 0.6079, 1.7231 },
            }
        );

        ExecuteQuotientsAndLog(
            matrix,
            new Vector[]
            {
                new Vector(new double[] { 1, 0.5, 1, 2 }),
                new Vector(new double[] { 999, -999, 5, 5 }),
                new Vector(new double[] { 5, 23.44, -42, -57 }),
                new Vector(new double[] { -52300, 22300, -42324, -125 }),
            }
        );

        var qrIterator = new HouseholderQRIterator();
        var (eigenValues, iteration) = qrIterator.SolveEigenvalues(matrix);
        Console.WriteLine();
        Console.WriteLine(@$"The eigen values are: {eigenValues}
The number of iterations for the convergence is: {iteration}");


        var matrix2 = new RectangularMatrix(
            new double[][]
            {
                new double[] { -233, 2, -15, 515, 47 },
                new double[] { 15, 1, -12524, 12, 4 },
                new double[] { 23, 2, 23, 1, -155 },
                new double[] { -47, -5, 51, -515, 1453 },
                new double[] { -56, -578, 2, 515, 1 },
            }
        );

        var (eigenValues2, iteration2) = qrIterator.SolveEigenvalues(matrix2);
        Console.WriteLine("Extras:");
        Console.WriteLine(@$"The eigen values are: {eigenValues2}
The number of iterations for the convergence is: {iteration2}");
    }

    public static void ExecuteQuotientsAndLog(RectangularMatrix matrix, Vector[] tests)
    {
        var rayleighQuotient = new RayleighQuotientIteration();

        Console.WriteLine(string.Format("|{0,-5}|{1,-30}|{2,-30}|{3,-20}|", "#", "Starting Eigen Vector", "Eigen Value", "Number of iterations"));
        for (var i = 0; i < tests.Length; i++)
        {
            var test = tests[i];
            var (eigenvalue, iterations) = rayleighQuotient.Converge(matrix, test);
            Console.WriteLine(string.Format("|{0,-5}|{1,-30}|{2,-30}|{3,-20}|", i, test, eigenvalue, iterations));
        }
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