using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MatrixSolver;

public class NewtonMethod
{
    public double FindRoot(double previousX, 
        Func<double, double> function, 
        Func<double, double> derivativeFunction, 
        double tolerance = 0.00001)
    {
        var xList = new List<double> { previousX };
        var iteration = 0;
        Console.WriteLine(string.Format("|{0,-5}|{1,-30}|{2,-30}|{3,-20}|{4,-20}|", "k", "xk", "f(xk)", "f'(xk)", "hk"));
        while (true)
        {
            var evalFunction = function(previousX);
            var evalDerivative = derivativeFunction(previousX);
            Console.WriteLine(string.Format("|{0,-5}|{1,-30}|{2,-30}|{3,-20}|{4,-20}|", iteration, previousX, evalFunction, evalDerivative, -evalFunction / evalDerivative));

            previousX = previousX - evalFunction / evalDerivative;
            xList.Add(previousX);
            iteration++;
            if (Math.Abs(xList[iteration] - xList[iteration - 1]) < tolerance)
            {
                break;
            }
        }
        return previousX;
    }

    public Vector FindRootFromSystem(Vector previousX,
        SystemOfEquations system,
        SystemOfEquations jacobianSystem,
        double tolerance = 0.0001)
    {
        var gaussSolver = new GaussEliminationSolver();
        var xList = new List<Vector> { previousX };
        var iteration = 0;
        Console.WriteLine(string.Format("|{0,-5}|{1,-30}|", "k", "[x1k x2k]"));
        while (true)
        {
            var evalFunctionVector = system.Calculate(previousX);
            var evalJacobianMatrix = jacobianSystem * previousX;
            var solvedSk = new Vector(gaussSolver.Solve(evalJacobianMatrix, (-evalFunctionVector).Values));

            previousX = previousX + solvedSk;
            xList.Add(previousX);
            iteration++;
            Console.WriteLine(string.Format("|{0,-5}|{1,-30}|", iteration, previousX));
            if ((xList[iteration] - xList[iteration - 1]).Select(Math.Abs).Any(x => x < tolerance))
            {
                break;
            }
        }
        return previousX;
    }
}
