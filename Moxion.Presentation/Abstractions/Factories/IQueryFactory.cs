using MediatR;
using Moxion.Abstractions;
using Moxion.Application.Feature.Abstractions;
using Moxion.Common;
using Moxion.Presentation.Abstractions.Transport;

namespace Moxion.Presentation.Abstractions.Factories;

internal interface IQueryFactory<TQuery, TUnitInfo, TData, in TRequest>
  where TQuery : IQuery<IQueryResult>
  where TUnitInfo : IUnitInfo<TUnitInfo>
  where TData : IQueryFactoryData<TQuery, TUnitInfo>?
  where TRequest : IRequest<IResponse>
{
  Task<Result<TData>> Create( TRequest request, CancellationToken cancellationToken );
}