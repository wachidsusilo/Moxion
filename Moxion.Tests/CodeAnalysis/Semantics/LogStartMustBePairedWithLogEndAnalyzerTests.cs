using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Moxion.Application.Extensions;
using Moxion.CodeAnalysis.Semantics;
using Moxion.Common;
using Moxion.Common.Enumerations;

namespace Moxion.Tests.CodeAnalysis.Semantics;

public class LogStartMustBePairedWithLogEndAnalyzerTests
{
  [Fact]
  public async Task LogStart_WithoutLogEnd_ReportsDiagnostic()
  {
    const string testCode = """
                            using Microsoft.Extensions.Logging;
                            using Moxion.Application.Extensions;

                            public class SomeClass
                            {
                                private readonly ILogger<SomeClass> _logger;
                            
                                public SomeClass(ILogger<SomeClass> logger)
                                {
                                    _logger = logger;
                                }
                            
                                public void DoSomething()
                                {
                                    _logger.LogStart(); // Missing LogEnd
                                }
                            }
                            """;

    var expectedDiagnostic = DiagnosticResult.CompilerError( "MX0003" )
      .WithLocation( 15, 9 );

    var analyzerTest = new CSharpAnalyzerTest<LogStartMustBePairedWithLogEndAnalyzer, DefaultVerifier>
    {
      TestCode = testCode,
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
  public async Task LogStart_WithLogEnd_NoDiagnostics()
  {
    const string testCode = """
                            using Microsoft.Extensions.Logging;
                            using Moxion.Common.Enumerations;
                            using Moxion.Application.Extensions;

                            public class SomeClass
                            {
                                private readonly ILogger<SomeClass> _logger;
                            
                                public SomeClass(ILogger<SomeClass> logger)
                                {
                                    _logger = logger;
                                }
                            
                                public void DoSomething()
                                {
                                    _logger.LogStart();
                                    _logger.LogEnd(ErrorCode.NoError); // Correct pairing
                                }
                            }
                            """;

    var analyzerTest = new CSharpAnalyzerTest<LogStartMustBePairedWithLogEndAnalyzer, DefaultVerifier>
    {
      TestCode = testCode,
      TestState =
      {
        AdditionalReferences =
        {
          MetadataReference.CreateFromFile( typeof(ErrorCode).Assembly.Location ),
          MetadataReference.CreateFromFile( typeof(Result).Assembly.Location ),
          MetadataReference.CreateFromFile( typeof(LoggerExtensions).Assembly.Location )
        }
      },
      ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        .AddPackages( [new PackageIdentity( "Microsoft.Extensions.Logging", "9.0.1" )] )
    };

    await analyzerTest.RunAsync();
  }
}