using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public interface IMatrix
{
    double[][] Matrix { get; }
    double this[int column, int row] { get; set; }
    int Rows { get; }
    int Columns { get; }
    void ReplaceColumn(Vector vector, int column);
    double CalculateConditionNumberInfinite();
    double[] Calculate(double[] parameters);
}
