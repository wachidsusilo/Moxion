using Moxion.Abstractions;
using Moxion.Application.Feature.Abstractions;
using Moxion.Presentation.Abstractions.Transport;

namespace Moxion.Presentation.Abstractions.Factories;

internal interface IResponseFactory<TResponse, in TQueryResult, in TUnitInfo>
  where TResponse : IResponse
  where TQueryResult : IQueryResult
  where TUnitInfo : IUnitInfo<TUnitInfo>
{
  Task<TResponse> Create(
    TQueryResult result,
    TUnitInfo sourceUnit,
    TUnitInfo destinationUnit,
    CancellationToken cancellationToken
  );
}