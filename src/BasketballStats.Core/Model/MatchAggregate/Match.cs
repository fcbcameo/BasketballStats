using Ardalis.SharedKernel;
using System.Collections.Generic; // For IReadOnlyCollection

namespace BasketballStats.Core.Model.MatchAggregate;

public class Match : EntityBase<Guid>, IAggregateRoot // Match is a clear Aggregate Root
{
  public DateTime MatchDate { get; private set; }
  public Guid SeasonId { get; private set; }
  public Guid TeamId { get; private set; }
  

  // Value Objects owned by the Match aggregate
  public TeamMatchStats? TeamStats { get; private set; }
  
  private readonly List<PlayerMatchPerformance> _playerPerformances = new();
  public IReadOnlyCollection<PlayerMatchPerformance> PlayerPerformances => _playerPerformances.AsReadOnly();

  private Match() { } // For EF Core

  public Match(Guid id, DateTime matchDate, Guid seasonId, Guid teamId)
  {
    Id = id;
    MatchDate = matchDate;
    SeasonId = seasonId; // Should validate these IDs exist via domain/application service before calling constructor
    TeamId = teamId;
    Guard.Against.Default(TeamId, nameof(TeamId));
    
  public void AddPlayerPerformance(PlayerMatchPerformance performance)
  {
    Guard.Against.Null(performance, nameof(performance));
    
    _playerPerformances.Add(performance);
  }

  public void SetTeamStats(TeamMatchStats stats)
  {
    Guard.Against.Null(stats, nameof(stats));
    TeamStats = stats;
  }
  //TODO check if this is a good place to raise domain events

  //public void FinalizeMatch() // Example method that might raise a domain event
  //{
  //  // Perform final consistency checks if needed
  //  // e.g., ensure total player points for a team match team total points if that's a rule.
  //  var newEvent = new MatchFinalizedEvent(this.Id); // Assuming MatchFinalizedEvent exists
  //  RegisterDomainEvent(newEvent);
  //}
}
