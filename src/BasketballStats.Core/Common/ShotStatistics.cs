using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

namespace BasketballStats.Core.Common;
public class ShotStatistic : ValueObject
{
    public int Made { get; }
    public int Attempted { get; }
    public decimal Percentage => Attempted == 0 ? 0 : Math.Round((decimal)Made / Attempted, 3);

    // Private constructor for EF Core or mapping, ensure immutability
    private ShotStatistic() { Made = 0; Attempted = 0; }

    public ShotStatistic(int made, int attempted)
    {
        Guard.Against.Negative(made, nameof(made));
        Guard.Against.Negative(attempted, nameof(attempted));
    //TODO consider if we want to allow 0 made shots with 0 attempted shots, or if that should be an error.
    // Ensure that made shots cannot exceed attempted shots

    //Guard.Against.Expression(() => made > attempted, nameof(made), $"{nameof(made)} cannot be greater than {nameof(attempted)}.");

    Made = made;
        Attempted = attempted;
    }

    // GetEqualityComponents is used by the ValueObject base class for comparison
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Made;
        yield return Attempted;
    }
    public override string ToString()
    {
        return $"{Made}/{Attempted} ({Percentage:P})";
    }
}
