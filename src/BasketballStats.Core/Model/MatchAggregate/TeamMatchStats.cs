using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketballStats.Core.Common;

namespace BasketballStats.Core.Model.MatchAggregate;

// This VO is very similar to PlayerStatsVO but represents aggregated team stats
// from the "TOTALS" row of a CSV. [4, 4, 4, 4, 4, 4]
public class TeamMatchStats : ValueObject
{
  public int Points { get; }
  public ShotStatistic FieldGoals { get; } = new(0, 0);
  public ShotStatistic TwoPointers { get; } = new(0, 0);
  public ShotStatistic ThreePointers { get; } = new(0, 0);
  public ShotStatistic FreeThrows { get; } = new(0, 0);
  public int OffensiveRebounds { get; }
  public int DefensiveRebounds { get; }
  public int TotalRebounds => OffensiveRebounds + DefensiveRebounds;
  public int Assists { get; }
  public int Steals { get; }
  public int Blocks { get; }
  public int Turnovers { get; }
  public int PersonalFouls { get; }
  // Add other team-level aggregate stats as needed (e.g., TeamEfficiency, TeamGameScore)

  private TeamMatchStats() { /* For EF Core */ }

  public TeamMatchStats(int points, ShotStatistic fieldGoals, ShotStatistic twoPointers,
                        ShotStatistic threePointers, ShotStatistic freeThrows,
                        int offensiveRebounds, int defensiveRebounds,
                        int assists, int steals, int blocks, int turnovers, int personalFouls)
  {
    Points = points;
    FieldGoals = fieldGoals;
    TwoPointers = twoPointers;
    ThreePointers = threePointers;
    FreeThrows = freeThrows;
    OffensiveRebounds = offensiveRebounds;
    DefensiveRebounds = defensiveRebounds;
    Assists = assists;
    Steals = steals;
    Blocks = blocks;
    Turnovers = turnovers;
    PersonalFouls = personalFouls;
  }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Points;
        yield return FieldGoals;
        yield return TwoPointers;
        yield return ThreePointers;
        yield return FreeThrows;
        yield return OffensiveRebounds;
        yield return DefensiveRebounds;
        yield return Assists;
        yield return Steals;
        yield return Blocks;
        yield return Turnovers;
        yield return PersonalFouls;
    }
}
