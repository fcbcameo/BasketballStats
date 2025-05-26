using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballStats.Core.Model.TeamAggregate;
public class Team : EntityBase<Guid>, IAggregateRoot
{
  public string Name { get; private set; } = string.Empty;

  private Team() { } // For EF Core

  public Team(Guid id, string name)
  {
    Id = id;
    Name = Guard.Against.NullOrWhiteSpace(name);
  }

  public void UpdateName(string newName)
  {
    Name = Guard.Against.NullOrWhiteSpace(newName);
  }
}
