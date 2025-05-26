using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballStats.Core.Model.PlayerAggregate;
public class Player : EntityBase<Guid>
{
  public string Name { get; private set; } = string.Empty;

  private Player() { } // Private constructor for EF Core

  public Player(Guid id, string name)
  {
      Id = id;
    Name = Guard.Against.NullOrWhiteSpace( name, nameof(name));
  }

  public void UpdateName(string newName)
  {
    Name = Guard.Against.NullOrWhiteSpace( newName, nameof(newName));
    //TODO consider raising a domainevent if name changes are significant
  }
}
