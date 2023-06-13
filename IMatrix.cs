using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public interface IMatrix
{
    decimal this[int column, int row] { get; set; }
    int Rows { get; }
    int Columns { get; }
    decimal CalculateConditionNumberInfinite();
    decimal[] Calculate(decimal[] parameters);
}
