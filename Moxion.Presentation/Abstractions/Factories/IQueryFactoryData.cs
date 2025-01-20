using Moxion.Abstractions;
using Moxion.Application.Feature.Abstractions;

namespace Moxion.Presentation.Abstractions.Factories;

internal interface IQueryFactoryData<out TQuery, out TUnitInfo>
  where TQuery : IQuery<IQueryResult>
  where TUnitInfo : IUnitInfo<TUnitInfo>
{
  public TQuery Query { get; }
  public TUnitInfo UnitInfo { get; }
}