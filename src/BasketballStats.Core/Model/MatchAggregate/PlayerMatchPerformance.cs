using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketballStats.Core.Model.PlayerAggregate;
using BasketballStats.Core.Model.TeamAggregate;

namespace BasketballStats.Core.Model.MatchAggregate;
public class PlayerMatchPerformance : ValueObject
{
  public Guid PlayerId { get; } // Links to Player entity
  public Guid TeamId { get; }   // Links to Team entity (team player played for in this match)
  public string JerseyNumber { get; } // Player's jersey number for this specific match/team context
  public PlayerStatsVO Statistics { get; } = null!; // Should be initialized

  private PlayerMatchPerformance() { /* For EF Core */ JerseyNumber = string.Empty; }

  public PlayerMatchPerformance(Guid playerId, Guid teamId, string jerseyNumber, PlayerStatsVO statistics)
  {
    PlayerId = Guard.Against.Default(playerId, nameof(playerId));
    TeamId = Guard.Against.Default(teamId, nameof(teamId));
    JerseyNumber = Guard.Against.NullOrWhiteSpace(jerseyNumber, nameof(jerseyNumber)); // Jersey number is important context here
    Statistics = Guard.Against.Null(statistics, nameof(statistics));
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return PlayerId;
    yield return TeamId;
    yield return JerseyNumber;
    yield return Statistics;
  }
  public override string ToString()
  {
    return $"{PlayerId} ({JerseyNumber}) - {Statistics.Points} points, {Statistics.MinutesPlayed} min";
  }
}
  
