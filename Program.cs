
using System.Runtime.InteropServices;

namespace MatrixSolver;

internal class Program
{
    /// <example>
    /// |n                   |X - Y                   |
    /// |10                  |47276.820230476354      |
    /// |16.795853149470418  |-10728.726676107384     |
    /// |15.757921000354067  |-317.98948669887614     |
    /// |15.725239700966094  |-0.3025817639136221     |
    /// |15.725208543915993  |-2.742744982242584E-07  |
    /// |15.725208543887751  |0                       |
    /// Years needed to repay loan: 15.725208543887751
    /// 
    /// ------------------------
    /// 
    /// |k    |[x, y]                                            Each Equation Value                               |
    /// |1    |3.5,0.375                                         7,-2.75                                           |
    /// |2    |3.8500000000000005,0.2375                         1.75,-0.453125                                    |
    /// |3    |3.7512342691190703,0.24967933204259443            -0.19250000000000078,0.02921874999999996          |
    /// |4    |3.750000621953734,0.2499998945437453              -0.004811602523313141,0.0010545645018246175       |
    /// |5    |3.7500000000000786,0.24999999999998254            -1.5818440832759961E-06,2.9270070356357536E-07    |
    /// Calculated root is: 3.7500000000000786,0.24999999999998254
    /// </example>
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var newtonMethod = new NewtonMethod();

        var a = 100000;
        var p = 10000;
        var r = 0.06;

        Func<double, double> equation = n => a * Math.Pow(1 + r, n) - p * (Math.Pow(1 + r, n) - 1) / r;
        Func<double, double> derivativeEquation = n => -3884.59387 * Math.Pow(1.06, n);
        // NewtonA
        var years = newtonMethod.FindRoot(
            10,
            equation,
            derivativeEquation,
            0.000001
        );

        Console.WriteLine(string.Format("|{0,-20}|{1,-24}|", "n", "X - Y"));
        for (var i = 0; i < years.Count; i++)
        {
            var numberYears = years[i];
            var loanBalance = equation(numberYears);
            Console.WriteLine(string.Format("|{0,-20}|{1,-24}|", numberYears, loanBalance));
        }
        Console.WriteLine($"Years needed to repay loan: {years.Last()}");

        Console.WriteLine();
        Console.WriteLine($"------------------------");
        Console.WriteLine();

        // Newton B
        var resultSystem = newtonMethod.FindRootFromSystem(
            new Vector(new double[] { 7, 0.5 }),
            new SystemOfEquations
            (
                // original system of equations 
                new Func<double[], double>[][]
                { 
                    // this syntax represents the equation itself
                    new Func<double[], double>[] { x => 5 * x[0] * x[1], x => -x[0] * (1 + x[1]) },
                    new Func<double[], double>[] { x => -x[0] * x[1], x => (1 - x[1]) * (1 + x[1]) }
                }
            ),
            new SystemOfEquations
            (
                // jacobian system
                new Func<double[], double>[][]
                {
                    new Func<double[], double>[] { x => 4 * x[1] - 1, x => 4 * x[0] },
                    new Func<double[], double>[] { x => -x[1], x => -x[0] - 2 * x[1] }
                }
            ),
            0.000001
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