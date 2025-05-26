using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketballStats.Core.Model.PlayerAggregate;

namespace BasketballStats.Core.Interfaces;
public interface IPlayerMatcherService
{
  Task<Result<Player>> FindOrCreatePlayerAsync(string name, string jerseyNumber, Guid teamId, Guid seasonId, CancellationToken cancellationToken = default);
}
