using Serilog.Core;
using Serilog.Events;

namespace Moxion.Infrastructure.Diagnostic.Enrichers;

public class ClassNameEnricher : ILogEventEnricher
{
  public void Enrich( LogEvent logEvent, ILogEventPropertyFactory propertyFactory )
  {
    if (!logEvent.Properties.TryGetValue( "SourceContext", out var value ))
    {
      return;
    }

    var className = value.ToString( "l", null ).Split( '.' ).LastOrDefault();
    logEvent.AddPropertyIfAbsent( propertyFactory.CreateProperty( "ClassName", className ) );
  }
}