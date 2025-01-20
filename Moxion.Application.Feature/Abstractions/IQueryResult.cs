using Moxion.Common.Enumerations;

namespace Moxion.Application.Feature.Abstractions;

internal interface IQueryResult
{
  public ErrorCode ErrorCode { get; }
};