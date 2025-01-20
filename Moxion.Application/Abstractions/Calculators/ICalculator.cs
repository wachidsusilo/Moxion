using Moxion.Common;

namespace Moxion.Application.Abstractions.Calculators;

internal interface ICalculator<in TParam, TResult>
  where TParam : ICalculationParam
  where TResult : ICalculationResult?
{
  Task<Result<TResult>> Execute( TParam param, CancellationToken cancellationToken );
}