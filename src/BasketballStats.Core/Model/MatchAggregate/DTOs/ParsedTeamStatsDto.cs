using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballStats.UseCases.Matches.DTOs;
public class ParsedTeamStatsDto
{
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
  // Add other relevant raw team stats if needed
}
