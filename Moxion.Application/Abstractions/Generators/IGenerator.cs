using Moxion.Common;

namespace Moxion.Application.Abstractions.Generators;

internal interface IGenerator<in TParam, TResult>
  where TParam : IGenerationParam
  where TResult : IGenerationResult?
{
  Task<Result<TResult>> Execute( TParam param, CancellationToken cancellationToken );
}