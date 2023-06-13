using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

public class Vector : IEnumerable<double>
{
    public double[] Values { get; set; }

    public Vector(IEnumerable<double> values)
    {
        Values = values.ToArray();
    }

    public static Vector operator *(Vector lhs, double rhs) => new Vector(lhs.Select(v => v * rhs));
    public static Vector operator *(double lhs, Vector rhs) => rhs * lhs;

    /// <summary>
    /// This assumes a dot product
    /// </summary>
    public static double operator *(Vector lhs, Vector rhs)
    {
        var sum = 0D;
        for (var i = 0; i < lhs.Values.Length; i++)
        {
            sum = sum + lhs.Values[i] * rhs.Values[i];
        }
        return sum;
    }

    public static Vector operator -(Vector lhs, Vector rhs)
    {
        var outputValues = new double[lhs.Values.Length];
        for (var i = 0; i < lhs.Values.Length; i++)
        {
            outputValues[i] = lhs.Values[i] - rhs.Values[i];
        }
        return new Vector(outputValues);
    }

    public IEnumerator<double> GetEnumerator() => Values.ToList().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public double Normalize() => Math.Sqrt(Values.Sum(v => Math.Pow(v, 2)));

    public override string ToString() => string.Join(",", Values);
}
