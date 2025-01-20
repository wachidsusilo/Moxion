using Moxion.Common;

namespace Moxion.Application.Abstractions.Simulators;

internal interface ISimulator<in TParam, TResult>
  where TParam : ISimulationParam
  where TResult : ISimulationResult?
{
  Task<Result<TResult>> Execute( TParam param, CancellationToken cancellationToken );
}