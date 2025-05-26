using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketballStats.Core.Common;
using BasketballStats.Core.Interfaces;
using BasketballStats.Core.Model.MatchAggregate;
using BasketballStats.Core.Model.SeasonAggregate;
using BasketballStats.Core.Model.TeamAggregate;
using BasketballStats.UseCases.Matches.DTOs;
using MediatR;
using Ardalis.Result;
using Ardalis.SharedKernel;
using BasketballStats.Core.Model.PlayerAggregate;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace BasketballStats.UseCases.Matches.Commands.ProcessMatchCsvFiles;
public class ProcessMatchCsvFilesCommandHandler : IRequestHandler<ProcessMatchCsvFilesCommand, Result<Guid>>
{
  private readonly IRepository<Match> _matchRepository; // Using generic repository from Ardalis.SharedKernel
  private readonly IReadRepository<Team> _teamRepository;
  private readonly IReadRepository<Season> _seasonRepository;
  private readonly ICsvFileProcessor _csvFileProcessor;
  private readonly IPlayerMatcherService _playerMatcherService;
  //private readonly IUnitOfWork _unitOfWork; // From Ardalis.SharedKernel, for transactional saves

  public ProcessMatchCsvFilesCommandHandler(
      IRepository<Match> matchRepository,
      IReadRepository<Team> teamRepository,
      IReadRepository<Season> seasonRepository,
      ICsvFileProcessor csvFileProcessor,
      IPlayerMatcherService playerMatcherService
    //,      IUnitOfWork unitOfWork
    )
  {
    _matchRepository = matchRepository;
    _teamRepository = teamRepository;
    _seasonRepository = seasonRepository;
    _csvFileProcessor = csvFileProcessor;
    _playerMatcherService = playerMatcherService;
    //_unitOfWork = unitOfWork;
  }

  public async Task<Result<Guid>> Handle(ProcessMatchCsvFilesCommand request, CancellationToken cancellationToken)
  {
    // 1. Validate input entities exist
    var season = await _seasonRepository.GetByIdAsync(request.SeasonId, cancellationToken);
    if (season == null) return Result<Guid>.NotFound($"Season with ID {request.SeasonId} not found.");

    var homeTeam = await _teamRepository.GetByIdAsync(request.HomeTeamId, cancellationToken);
    if (homeTeam == null) return Result<Guid>.NotFound($"Home Team with ID {request.HomeTeamId} not found.");

    var visitorTeam = await _teamRepository.GetByIdAsync(request.VisitorTeamId, cancellationToken);
    if (visitorTeam == null) return Result<Guid>.NotFound($"Visitor Team with ID {request.VisitorTeamId} not found.");

    if (request.HomeTeamId == request.VisitorTeamId)
      return Result<Guid>.Invalid(new ValidationError { ErrorMessage = "Home and Visitor teams cannot be the same.", Identifier = nameof(request.VisitorTeamId) });

    // 2. Parse Home Team CSV
    Result<ParsedCsvDataDto> homeParsedResult;
    using (var homeStream = request.HomeTeamCsv.OpenReadStream())
    {
      homeParsedResult = await _csvFileProcessor.ProcessCsvAsync(homeStream, cancellationToken);
    }
    if (!homeParsedResult.IsSuccess  || homeParsedResult.Value.TeamTotals == null)
      return Result<Guid>.Error(homeParsedResult.Errors.Any() ? homeParsedResult.Errors.First() : "Failed to parse home team CSV or missing team totals.");

    //Only one team to parse for now!

    //// 3. Parse Visitor Team CSV
    //Result<ParsedCsvDataDto> visitorParsedResult;
    //using (var visitorStream = request.VisitorTeamCsv.OpenReadStream())
    //{
    //  visitorParsedResult = await _csvFileProcessor.ProcessCsvAsync(visitorStream, cancellationToken);
    //}
    //if (!visitorParsedResult.IsSuccess || visitorParsedResult.Value.TeamTotals == null)
    //  return Result<Guid>.Error(visitorParsedResult.Errors.Any() ? visitorParsedResult.Errors.First() : "Failed to parse visitor team CSV or missing team totals.");

    // 4. Create new Match entity
    var matchId = Guid.NewGuid();
    var match = new Match(matchId, request.MatchDate, request.SeasonId, request.HomeTeamId);

    // 5. Process and add Home Player Performances
    foreach (var rawPlayerStat in homeParsedResult.Value.PlayerStats)
    {
      var playerResult = await _playerMatcherService.FindOrCreatePlayerAsync(rawPlayerStat.Name, rawPlayerStat.JerseyNumber, request.HomeTeamId, request.SeasonId, cancellationToken);
      if (!playerResult.IsSuccess) return Result<Guid>.Error($"Failed to find or create player '{rawPlayerStat.Name}': {playerResult.Errors.FirstOrDefault()}");

      var playerStatsVO = MapToPlayerStatsVO(rawPlayerStat);
      var performance = new PlayerMatchPerformance(playerResult.Value.Id, request.HomeTeamId, rawPlayerStat.JerseyNumber, playerStatsVO);
      match.AddPlayerPerformance(performance);
    }
    var homeTeamStatsVO = MapToTeamStatsVO(homeParsedResult.Value.TeamTotals);
    match.SetTeamStats(homeTeamStatsVO);

    //// 6. Process and add Visitor Player Performances
    //foreach (var rawPlayerStat in visitorParsedResult.Value.PlayerStats)
    //{
    //  var playerResult = await _playerMatcherService.FindOrCreatePlayerAsync(rawPlayerStat.Name, rawPlayerStat.JerseyNumber, request.VisitorTeamId, request.SeasonId, cancellationToken);
    //  if (!playerResult.IsSuccess) return Result<Guid>.Error($"Failed to find or create player '{rawPlayerStat.Name}': {playerResult.Errors.FirstOrDefault()?.ErrorMessage}");

    //  var playerStatsVO = MapToPlayerStatsVO(rawPlayerStat);
    //  var performance = new PlayerMatchPerformance(playerResult.Value.Id, request.VisitorTeamId, rawPlayerStat.JerseyNumber, playerStatsVO);
    //  match.AddPlayerPerformance(performance);
    //}
    //var visitorTeamStatsVO = MapToTeamStatsVO(visitorParsedResult.Value.TeamTotals);
    //match.SetVisitorTeamStats(visitorTeamStatsVO);

    // 7. Save the Match aggregate
    await _matchRepository.AddAsync(match, cancellationToken);
   // await _unitOfWork.SaveChangesAsync(cancellationToken); // Save changes transactionally

    return Result<Guid>.Success(match.Id);
  }

  // Helper methods to map from Parsed DTOs to Domain VOs
  private PlayerStatsVO MapToPlayerStatsVO(ParsedPlayerStatDto raw)
  {
    return new PlayerStatsVO(
        raw.Points, raw.MinutesPlayed,
        new ShotStatistic(raw.FieldGoalsMade, raw.FieldGoalsAttempted),
        new ShotStatistic(raw.TwoPointersMade, raw.TwoPointersAttempted),
        new ShotStatistic(raw.ThreePointersMade, raw.ThreePointersAttempted),
        new ShotStatistic(raw.FreeThrowsMade, raw.FreeThrowsAttempted),
        raw.OffensiveRebounds, raw.DefensiveRebounds,
        raw.Assists, raw.Steals, raw.Blocks, raw.Turnovers, raw.PersonalFouls,
        raw.PlusMinus, raw.Efficiency, raw.GameScore
    );
  }

  private TeamMatchStats MapToTeamStatsVO(ParsedTeamStatsDto raw)
  {
    return new TeamMatchStats(
        raw.Points,
        new ShotStatistic(raw.FieldGoalsMade, raw.FieldGoalsAttempted),
        new ShotStatistic(raw.TwoPointersMade, raw.TwoPointersAttempted),
        new ShotStatistic(raw.ThreePointersMade, raw.ThreePointersAttempted),
        new ShotStatistic(raw.FreeThrowsMade, raw.FreeThrowsAttempted),
        raw.OffensiveRebounds, raw.DefensiveRebounds,
        raw.Assists, raw.Steals, raw.Blocks, raw.Turnovers, raw.PersonalFouls
    );
  }
}
