using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballStats.UseCases.Matches.DTOs;
public class ParsedPlayerStatDto
{
  public string JerseyNumber { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public int MinutesPlayed { get; set; }
  public int Points { get; set; }
  public int FieldGoalsMade { get; set; }
  public int FieldGoalsAttempted { get; set; }
  public int TwoPointersMade { get; set; }
  public int TwoPointersAttempted { get; set; }
  public int ThreePointersMade { get; set; }
  public int ThreePointersAttempted { get; set; }
  public int FreeThrowsMade { get; set; }
  public int FreeThrowsAttempted { get; set; }
  public int OffensiveRebounds { get; set; }
  public int DefensiveRebounds { get; set; }
  public int Assists { get; set; }
  public int Steals { get; set; }
  public int Blocks { get; set; }
  public int Turnovers { get; set; }
  public int PersonalFouls { get; set; }
  public int PlusMinus { get; set; }
  public decimal Efficiency { get; set; } // Assuming EFF from CSV [1, 1, 1, 1, 1, 1]
  public decimal GameScore { get; set; }  // Assuming GmSc from CSV [1, 1, 1, 1, 1, 1]
                                          // Add other relevant raw stats if needed
}
