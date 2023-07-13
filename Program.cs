
namespace MatrixSolver;

internal class Program
{
    /// Note that this program uses the Func<> data type to represent user customizable systems of equations.
    /// This is done using the SystemOfEquations class.
    /// 
    /// Example code
    /// <example>
    /// |k    |xk                            |f(xk)                         |f'(xk)              |hk                  |
    /// |0    |0.5                           |-3.75                         |1                   |3.75                |
    /// |1    |4.25                          |14.0625                       |8.5                 |-1.6544117647058822 |
    /// |2    |2.5955882352941178            |2.7370782871972317            |5.1911764705882355  |-0.5272558740209965 |
    /// |3    |2.0683323612731215            |0.27799875668964624           |4.136664722546243   |-0.06720359887386028|
    /// |4    |2.001128762399261             |0.004516323701597713          |4.002257524798522   |-0.001128444052791198|
    /// |5    |2.00000031834647              |1.2733859815483584E-06        |4.00000063669294    |-3.183464447148561E-07|
    /// Calculated root is: 2.0000000000000253
    /// 
    /// ------------------------
    /// 
    /// |k    |[x1k x2k]                     |
    /// |1    |-0.8333333333333333,1.4166666666666665|
    /// |2    |-0.18939393939393911,1.0946969696969695|
    /// |3    |-0.015079135302065144,1.0075395676510326|
    /// |4    |-0.00011200127829965635,1.00005600063915|
    /// |5    |-6.2714405382182856E-09,1.0000000031357204|
    /// Calculated root is: -6.2714405382182856E-09,1.0000000031357204
    /// </example>
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var newtonMethod = new NewtonMethod();

        var resultNewton = newtonMethod.FindRoot(
            0.5,
            x => Math.Pow(x - 2, 2) + 4 * x - 8, // (𝑥 − 2)^2 + 4𝑥 − 8 = 0
            x => 2 * x // derivative of (𝑥 − 2)^2 + 4𝑥 − 8 = 0
        );

        Console.WriteLine($"Calculated root is: {resultNewton}");
        Console.WriteLine();
        Console.WriteLine($"------------------------");
        Console.WriteLine();

        var resultSystem = newtonMethod.FindRootFromSystem(
            new Vector(new double[] { 1, 2 }),
            new SystemOfEquations
            (
                // 𝑥 + 2 * 𝑥 − 2 = 0 original system of equations 
                // 𝑥^2 + 4𝑥^2 − 4 = 0
                new Func<double[], double>[][]
                {
                    new Func<double[], double>[] { x => x[0], x => 2 * x[1], x => -2 },
                    new Func<double[], double>[] { x => Math.Pow(x[0], 2), x => 4 * Math.Pow(x[1], 2), x => -4 }
                }
            ),
            new SystemOfEquations
            (
                // 1 + 2 = 0 jacobian system
                // 2x + 8x = 0
                new Func<double[], double>[][]
                {
                    new Func<double[], double>[] { x => 1, x => 2 },
                    new Func<double[], double>[] { x => 2 * x[0], x => 8*x[1] }
                }
            )
        );
        Console.WriteLine($"Calculated root is: {resultSystem}");
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