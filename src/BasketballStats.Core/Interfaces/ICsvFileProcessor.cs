

using BasketballStats.UseCases.Matches.DTOs;

namespace BasketballStats.Core.Interfaces;

public interface ICsvFileProcessor
{
  Task<Result<ParsedCsvDataDto>> ProcessCsvAsync(Stream csvStream, CancellationToken cancellationToken = default);
}
