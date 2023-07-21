
using System.Runtime.InteropServices;

namespace MatrixSolver;

internal class Program
{
    /// <example>
    /// |𝑘(𝑥, 𝑦)    𝑓(𝑥, 𝑦)                                        ∇𝑓(𝑥, 𝑦)                                       |
    /// |1             -4,0                                              2002.0000000000996,-200.0000000000142             |
    /// |2             1600.0000000001137,-800.0000000000284             402.00000000004263,-200.0000000000141             |
    /// Calculated Minimum is: 1.0000000000000353,1.0000000000000702
    /// 
    /// |𝑘(𝑥, 𝑦)    𝑓(𝑥, 𝑦)                                        ∇𝑓(𝑥, 𝑦)                                       |
    /// |1             -2,200                                            4.0403525163505964,202.01005025125627             |
    /// |2             -2.0101010088192535,-0.0050503775157193           795.9400035301187,-197.97989974999996             |
    /// |3             397.9498010125183,-199.98995012750936             395.9900532550491,-197.98995050253126             |
    /// |4             -0.010049996212431855,-1.2627199907910835E-07     402.0100999834794,-199.99999974619146             |
    /// |5             0.010100743629480213,-0.0050503724524446625       401.9999999961484,-199.99999999871613             |
    /// Calculated Minimum is: 0.9999999999967903,0.9999999999935805
    /// 
    /// |𝑘(𝑥, 𝑦)    𝑓(𝑥, 𝑦)                                        ∇𝑓(𝑥, 𝑦)                                       |
    /// |1             2402,-600                                         2397.3444204196558,-599.3344425956739             |
    /// |2             1.9988852036471645,-0.0005537083230819917         800.8920867454805,-200.2209924392788              |
    /// |3             398.4499841223506,-199.11443261967656             402.6599060837698,-200.21988810893959             |
    /// Calculated Minimum is: 1.000549720272349,1.001099742729454
    /// </example>
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var newtonMethod = new NewtonMethod();
        var startingPoints = new Vector[]
        {
            new Vector(new double[] { -1, 1 }),
            new Vector(new double[] { 0, 1 }),
            new Vector(new double[] { 2, 1 })
        };

        foreach (var point in startingPoints)
        {
            var root = newtonMethod.FindRootFromSystem(
                point,
                new SystemOfEquations(
                    new Func<double[], double>[][]
                    {
                        // first partial derivative (gradient)
                        new Func<double[], double>[] { x => -400 * x[0] * x[1] + 400 * Math.Pow(x[0], 3) + 2 * x[0] - 2 },
                        new Func<double[], double>[] { x => 200 * x[1] - 200 * Math.Pow(x[0], 2) }
                    }
                ),
                new SystemOfEquations(
                    new Func<double[], double>[][]
                    {
                        // second partial derivative (hessian)
                        new Func<double[], double>[] { x => -400 * x[1] + 1200 * Math.Pow(x[0], 2) + 2, x => -400 * x[0] },
                        new Func<double[], double>[] { x => -400 * x[0], x => 200 }
                    }
                ),
                0.00001
            );
            Console.WriteLine($"Calculated Minimum is: {root}");
            Console.WriteLine();
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