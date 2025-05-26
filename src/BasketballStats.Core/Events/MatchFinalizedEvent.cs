using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballStats.Core.Events;
public class MatchFinalizedEvent : DomainEventBase
{
  public Guid MatchId { get; }

  public MatchFinalizedEvent(Guid matchId)
  {
    MatchId = matchId;
  }
}
