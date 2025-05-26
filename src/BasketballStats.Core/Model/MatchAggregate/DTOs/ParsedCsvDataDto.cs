using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballStats.UseCases.Matches.DTOs;
public class ParsedCsvDataDto
{
  public List<ParsedPlayerStatDto> PlayerStats { get; set; } = new();
  public ParsedTeamStatsDto? TeamTotals { get; set; }
}
