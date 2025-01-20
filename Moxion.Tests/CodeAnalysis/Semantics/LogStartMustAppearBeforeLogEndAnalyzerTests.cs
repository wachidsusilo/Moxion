using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Moxion.Application.Extensions;
using Moxion.CodeAnalysis.Semantics;
using Moxion.Common;
using Moxion.Common.Enumerations;

namespace Moxion.Tests.CodeAnalysis.Semantics;

public class LogStartMustAppearBeforeLogEndAnalyzerTests
{
  [Fact]
  public async Task LogEndBeforeLogStart_ReportsDiagnostic()
  {
    const string testCode = """
                            using Microsoft.Extensions.Logging;
                            using Moxion.Common.Enumerations;
                            using Moxion.Application.Extensions;

                            public class SomeRandomClass
                            {
                                private readonly ILogger<SomeRandomClass> _logger;
                            
                                public SomeRandomClass(ILogger<SomeRandomClass> logger) {
                                    _logger = logger;
                                }
                            
                                public void DoSomething()
                                {
                                    _logger.LogEnd(ErrorCode.NoError);
                                    _logger.LogStart();
                                }
                            }
                            """;

    var expectedDiagnostic = DiagnosticResult.CompilerError( "MX0004" )
      .WithLocation( 16, 9 );

    var analyzerTest = new CSharpAnalyzerTest<LogStartMustAppearBeforeLogEndAnalyzer, DefaultVerifier>
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
      ExpectedDiagnostics = { expectedDiagnostic },
      ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        .AddPackages( [new PackageIdentity( "Microsoft.Extensions.Logging", "9.0.1" )] )
        .AddAssemblies( [] ),
    };

    await analyzerTest.RunAsync();
  }

  [Fact]
  public async Task LogStartBeforeLogEnd_NoDiagnostics()
  {
    const string testCode = """
                            using Microsoft.Extensions.Logging;
                            using Moxion.Common.Enumerations;
                            using Moxion.Application.Extensions;

                            public class SomeRandomClass
                            {
                                private readonly ILogger<SomeRandomClass> _logger;
                            
                                public SomeRandomClass(ILogger<SomeRandomClass> logger) {
                                    _logger = logger;
                                }
                            
                                public void DoSomething()
                                {
                                    _logger.LogStart();
                                    _logger.LogEnd(ErrorCode.NoError);
                                }
                            }
                            """;

    var analyzerTest = new CSharpAnalyzerTest<LogStartMustAppearBeforeLogEndAnalyzer, DefaultVerifier>
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
        .AddAssemblies( [] ),
    };

    await analyzerTest.RunAsync();
  }

  [Fact]
  public async Task LogStartWithNoLogEnd_NoDiagnostics()
  {
    const string testCode = """
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
                                    _logger.LogStart("Start");
                                }
                            }
                            """;

    var analyzerTest = new CSharpAnalyzerTest<LogStartMustAppearBeforeLogEndAnalyzer, DefaultVerifier>
    {
      TestCode = testCode,
      TestState =
      {
        AdditionalReferences =
        {
          MetadataReference.CreateFromFile( typeof(LoggerExtensions).Assembly.Location )
        }
      },
      ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        .AddPackages( [new PackageIdentity( "Microsoft.Extensions.Logging", "9.0.1" )] )
        .AddAssemblies( [] ),
    };

    await analyzerTest.RunAsync();
  }
}