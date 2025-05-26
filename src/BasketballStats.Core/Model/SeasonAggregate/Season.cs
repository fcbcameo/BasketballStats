using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballStats.Core.Model.SeasonAggregate;
public class Season : EntityBase<Guid> // Could be changed to aggregateroot
{
  public string Name { get; private set; } = string.Empty;
  public DateTime? StartDate { get; private set; }
  public DateTime? EndDate { get; private set; }

  private Season() { } // for EF Core

  public Season(Guid id, string name, DateTime? startDate = null, DateTime? endDate = null)
  {
    Id = id;
    Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    // Add validation for StartDate < EndDate if both are provided
    StartDate = startDate;
    EndDate = endDate;
  }

  public void UpdateDetails(string newName, DateTime? newStartDate, DateTime? newEndDate)
  {
    Name = Guard.Against.NullOrWhiteSpace(newName, nameof(newName));
    StartDate = newStartDate;
    EndDate = newEndDate;
  }

}
