using MediatR;

namespace Moxion.Application.Feature.Abstractions;

internal interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
  where TQuery : IQuery<TResponse>
  where TResponse : IQueryResult;