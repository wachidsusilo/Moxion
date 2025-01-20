using MediatR;
using Moxion.Abstractions;
using Moxion.Common;
using Moxion.Presentation.Abstractions.Transport;

namespace Moxion.Presentation.Abstractions.Extractors;

internal interface IUnitExtractor<in TRequest, TUnitInfo>
  where TRequest : IRequest<IResponse>
  where TUnitInfo : IUnitInfo<TUnitInfo>
{
  Task<Result<TUnitInfo>> Extract( TRequest request, CancellationToken cancellationToken );
}