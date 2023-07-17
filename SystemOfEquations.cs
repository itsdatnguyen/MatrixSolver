using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

/// <summary>
/// Class used to represent a list of equations
/// </summary>
public class SystemOfEquations 
{
    public Func<double[], double>[][] Equations { get; set; }

    public SystemOfEquations(Func<double[], double>[][] equations) 
    { 
        Equations = equations;
    }

    public Vector Calculate(Vector x)
    {
        var data = new double[Equations.Length];
        for (var row = 0; row < Equations.Length; row++)
        {
            data[row] = Equations[row].Sum(fx => fx(x.Values));
        }
        return new Vector(data);
    }

    public static RectangularMatrix operator *(SystemOfEquations lhs, Vector rhs)
    {
        var data = new double[lhs.Equations.Length][];
        for (var row = 0; row < lhs.Equations.Length; row++)
        {
            var dataRow = new double[lhs.Equations[0].Length];
            for (var col = 0; col < lhs.Equations[0].Length; col++)
            {
                dataRow[col] = lhs.Equations[row][col](rhs.Values);
            }
            data[row] = dataRow;
        }
        return new RectangularMatrix(data);
    }
}
