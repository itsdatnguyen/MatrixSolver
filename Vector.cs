using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver;

/// <summary>
/// Stores a list of numbers.
/// </summary>
public class Vector : IEnumerable<double>
{
    public double[] Values { get; set; }

    public Vector(IEnumerable<double> values)
    {
        Values = values.ToArray();
    }

    public Vector Clone()
    {
        return new Vector((IEnumerable<double>)Values.Clone());
    }

    public double DotProduct(Vector rhs)
    {
        var sum = 0D;
        for (var i = 0; i < Values.Length; i++)
        {
            sum = sum + Values[i] * rhs.Values[i];
        }
        return sum;
    }

    public static Vector operator *(Vector lhs, double rhs) => new Vector(lhs.Select(v => v * rhs));
    public static Vector operator *(double lhs, Vector rhs) => rhs * lhs;

    /// <summary>
    /// Creates a Square matrix
    /// </summary>
    public static RectangularMatrix operator *(Vector lhs, Vector rhs)
    {
        var data = new double[lhs.Values.Length][];
        for (var left = 0; left < lhs.Values.Length; left++)
        {
            var row = new double[lhs.Values.Length];
            for (var right = 0; right < lhs.Values.Length; right++)
            {
                row[right] = lhs.Values[left] * rhs.Values[right];
            }
            data[left] = row;
        }
        return new RectangularMatrix(data);
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

    public static Vector operator -(Vector lhs, double rhs)
    {
        var outputValues = new double[lhs.Values.Length];
        for (var i = 0; i < lhs.Values.Length; i++)
        {
            outputValues[i] = lhs.Values[i] - rhs;
        }
        return new Vector(outputValues);
    }

    public static Vector operator /(Vector lhs, double rhs)
    {
        var outputValues = new double[lhs.Values.Length];
        for (var i = 0; i < lhs.Values.Length; i++)
        {
            outputValues[i] = lhs.Values[i] / rhs;
        }
        return new Vector(outputValues);
    }

    public IEnumerator<double> GetEnumerator() => Values.ToList().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public double Normalize() => Math.Sqrt(Values.Sum(v => Math.Pow(v, 2)));
    public double NormalizeInfinity() => Values.Max();

    public override string ToString() => string.Join(",", Values);
}
