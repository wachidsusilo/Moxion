using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Moxion.Application.Extensions;
using Moxion.CodeAnalysis.Semantics;

namespace Moxion.Tests.CodeAnalysis.Semantics;

public class LogStartMustAppearAtMostOnceAnalyzerTests
{
  [Fact]
  public async Task LogStart_MultipleCallsInMethod_ReportsDiagnostic()
  {
    const string text = """
                        using Microsoft.Extensions.Logging;
                        using Moxion.Application.Extensions;

                        public class SomeRandomClass
                        {
                            private readonly ILogger<SomeRandomClass> _logger;
                        
                            public SomeRandomClass(ILogger<SomeRandomClass> logger) {
                              _logger = logger;
                            }
                        
                            public void DoSomething()
                            {
                                _logger.LogStart(); // First LogStart
                                _logger.LogStart(); // Second LogStart (Should trigger the diagnostic)
                            }
                        }
                        """;

    var expectedDiagnostic = DiagnosticResult.CompilerError( "MX0002" )
      .WithLocation( 15, 9 );

    var analyzerTest = new CSharpAnalyzerTest<LogStartMustAppearAtMostOnceAnalyzer, DefaultVerifier>
    {
      TestCode = text,
      ExpectedDiagnostics = { expectedDiagnostic },
      TestState =
      {
        AdditionalReferences =
        {
          MetadataReference.CreateFromFile( typeof(LoggerExtensions).Assembly.Location )
        }
      },
      ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        .AddPackages( [new PackageIdentity( "Microsoft.Extensions.Logging", "9.0.1" )] )
    };

    await analyzerTest.RunAsync();
  }

  [Fact]
  public async Task LogStart_SingleCallInMethod_NoDiagnostic()
  {
    const string text = """
                        using Microsoft.Extensions.Logging;
                        using Moxion.Application.Extensions;

                        public class SomeRandomClass
                        {
                            private readonly ILogger<SomeRandomClass> _logger;
                        
                            public SomeRandomClass(ILogger<SomeRandomClass> logger) {
                              _logger = logger;
                            }
                        
                            public void DoSomething()
                            {
                                _logger.LogStart(); // Only one LogStart (No diagnostic expected)
                            }
                        }
                        """;

    var analyzerTest = new CSharpAnalyzerTest<LogStartMustAppearAtMostOnceAnalyzer, DefaultVerifier>
    {
      TestCode = text,
      TestState =
      {
        AdditionalReferences =
        {
          MetadataReference.CreateFromFile( typeof(LoggerExtensions).Assembly.Location )
        }
      },
      ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        .AddPackages( [new PackageIdentity( "Microsoft.Extensions.Logging", "9.0.1" )] )
    };

    await analyzerTest.RunAsync();
  }
}