using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketballStats.Core.Common;

namespace BasketballStats.Core.Model.MatchAggregate;
public class PlayerStatsVO : ValueObject
{
  public int Points { get; }
  public int MinutesPlayed { get; } // From "MIN" column [4, 4, 4, 4, 4, 4]
  public ShotStatistic FieldGoals { get; } = new(0, 0); // FGM, FGA [4, 4, 4, 4, 4, 4]
  public ShotStatistic TwoPointers { get; } = new(0, 0); // 2PM, 2PA [4, 4, 4, 4, 4, 4]
  public ShotStatistic ThreePointers { get; } = new(0, 0); // 3PM, 3PA [4, 4, 4, 4, 4, 4]
  public ShotStatistic FreeThrows { get; } = new(0, 0); // FTM, FTA [4, 4, 4, 4, 4, 4]
  public int OffensiveRebounds { get; } // OREB [4, 4, 4, 4, 4, 4]
  public int DefensiveRebounds { get; } // DREB [4, 4, 4, 4, 4, 4]
  public int TotalRebounds => OffensiveRebounds + DefensiveRebounds; // REB (calculated or from CSV) [4, 4, 4, 4, 4, 4]
  public int Assists { get; } // AST [4, 4, 4, 4, 4, 4]
  public int Steals { get; } // STL [4, 4, 4, 4, 4, 4]
  public int Blocks { get; } // BLK [4, 4, 4, 4, 4, 4]
  public int Turnovers { get; } // TO [4, 4, 4, 4, 4, 4]
  public int PersonalFouls { get; } // PF [4, 4, 4, 4, 4, 4]
  public int PlusMinus { get; } // PM [4, 4, 4, 4, 4, 4]
  public decimal Efficiency { get; } // EFF [4, 4, 4, 4, 4, 4]
  public decimal GameScore { get; } // GmSc [4, 4, 4, 4, 4, 4]
                                    // TODO Add other stats from CSV as needed: eFG%, TS%, etc. [4, 4, 4, 4, 4, 4]

  private PlayerStatsVO() { /* For EF Core */ }

  public PlayerStatsVO(int points, int minutesPlayed,
                       ShotStatistic fieldGoals, ShotStatistic twoPointers, ShotStatistic threePointers, ShotStatistic freeThrows,
                       int offensiveRebounds, int defensiveRebounds, /* int totalRebounds, // Can be calculated */
                       int assists, int steals, int blocks, int turnovers, int personalFouls,
                       int plusMinus, decimal efficiency, decimal gameScore)
  {
    Points = points;
    MinutesPlayed = minutesPlayed;
    FieldGoals = fieldGoals;
    TwoPointers = twoPointers;
    ThreePointers = threePointers;
    FreeThrows = freeThrows;
    OffensiveRebounds = offensiveRebounds;
    DefensiveRebounds = defensiveRebounds;
    // TotalRebounds = totalRebounds; // Or calculate: Guard.Against.AgainstExpression(dr => dr!= offensiveRebounds + defensiveRebounds, totalRebounds, "Total rebounds mismatch.");
    Assists = assists;
    Steals = steals;
    Blocks = blocks;
    Turnovers = turnovers;
    PersonalFouls = personalFouls;
    PlusMinus = plusMinus;
    Efficiency = efficiency;
    GameScore = gameScore;
  }

  //TODO Check why not ok?
  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Points;
    yield return MinutesPlayed;
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
    yield return PlusMinus;
    yield return Efficiency;
    yield return GameScore;
  }
}
