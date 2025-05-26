using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;

namespace BasketballStats.UseCases.Matches.Commands.ProcessMatchCsvFiles;
public record ProcessMatchCsvFilesCommand(
    Guid SeasonId,
    Guid HomeTeamId,
    Guid VisitorTeamId,
    DateTime MatchDate,
    IFormFile HomeTeamCsv,
    IFormFile VisitorTeamCsv) : IRequest<Result<Guid>>; // Returns Result with the new MatchId
