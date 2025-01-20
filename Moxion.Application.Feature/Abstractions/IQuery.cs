using MediatR;

namespace Moxion.Application.Feature.Abstractions;

internal interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : IQueryResult;