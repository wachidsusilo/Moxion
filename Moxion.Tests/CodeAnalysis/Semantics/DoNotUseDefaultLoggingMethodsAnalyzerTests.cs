using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Moxion.CodeAnalysis.Semantics;

namespace Moxion.Tests.CodeAnalysis.Semantics;

public class DoNotUseDefaultLoggingMethodsAnalyzerTests
{
  [Theory]
  [InlineData( "LogDebug" )]
  [InlineData( "LogTrace" )]
  [InlineData( "LogInformation" )]
  [InlineData( "LogWarning" )]
  [InlineData( "LogError" )]
  [InlineData( "LogCritical" )]
  public async Task DefaultLoggingMethods_InProhibitedClass_ReportsDiagnostic( string methodName )
  {
    var text = $$"""
                 using Microsoft.Extensions.Logging;

                 public class SomeRandomClass
                 {
                     private readonly ILogger<SomeRandomClass> _logger;
                 
                     public SomeRandomClass(ILogger<SomeRandomClass> logger) {
                       _logger = logger;
                     }
                 
                     public void DoSomething()
                     {
                       _logger.{{methodName}}("Hello World!");
                     }
                 }
                 """;

    var expectedDiagnostic = DiagnosticResult.CompilerError( "MX0001" )
      .WithLocation( 13, 7 )
      .WithArguments( methodName );

    var analyzerTest = new CSharpAnalyzerTest<DoNotUseDefaultLoggingMethodsAnalyzer, DefaultVerifier>
    {
      TestCode = text,
      ExpectedDiagnostics = { expectedDiagnostic },
      ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        .AddPackages( [new PackageIdentity( "Microsoft.Extensions.Logging", "9.0.1" )] )
    };

    await analyzerTest.RunAsync();
  }

  [Theory]
  [InlineData( "LogDebug" )]
  [InlineData( "LogTrace" )]
  [InlineData( "LogInformation" )]
  [InlineData( "LogWarning" )]
  [InlineData( "LogError" )]
  [InlineData( "LogCritical" )]
  public async Task DefaultLoggingMethods_InProgramClassInProhibitedNamespace_ReportsDiagnostic( string methodName )
  {
    var text = $$"""
                 using Microsoft.Extensions.Logging;

                 namespace Some.Name.Space;

                 public class Program
                 {
                     private readonly ILogger<Program> _logger;
                 
                     public Program(ILogger<Program> logger) {
                       _logger = logger;
                     }
                 
                     public void DoSomething()
                     {
                       _logger.{{methodName}}("Hello World!");
                     }
                 }
                 """;

    var expectedDiagnostic = DiagnosticResult.CompilerError( "MX0001" )
      .WithLocation( 15, 7 )
      .WithArguments( methodName );

    var analyzerTest = new CSharpAnalyzerTest<DoNotUseDefaultLoggingMethodsAnalyzer, DefaultVerifier>
    {
      TestCode = text,
      ExpectedDiagnostics = { expectedDiagnostic },
      ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        .AddPackages( [new PackageIdentity( "Microsoft.Extensions.Logging", "9.0.1" )] )
    };

    await analyzerTest.RunAsync();
  }

  [Theory]
  [InlineData( "LogDebug" )]
  [InlineData( "LogTrace" )]
  [InlineData( "LogInformation" )]
  [InlineData( "LogWarning" )]
  [InlineData( "LogError" )]
  [InlineData( "LogCritical" )]
  public async Task DefaultLoggingMethods_InLoggerExtensionsClassInProhibitedNamespace_ReportsDiagnostic(
    string methodName
  )
  {
    var text = $$"""
                 using Microsoft.Extensions.Logging;

                 namespace Some.Name.Space;

                 public static class LoggerExtensions
                 {
                     public static void LogSomething(this ILogger logger, string message)
                     {
                       logger.{{methodName}}(message);
                     }
                 }
                 """;

    var expectedDiagnostic = DiagnosticResult.CompilerError( "MX0001" )
      .WithLocation( 9, 7 )
      .WithArguments( methodName );

    var analyzerTest = new CSharpAnalyzerTest<DoNotUseDefaultLoggingMethodsAnalyzer, DefaultVerifier>
    {
      TestCode = text,
      ExpectedDiagnostics = { expectedDiagnostic },
      ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        .AddPackages( [new PackageIdentity( "Microsoft.Extensions.Logging", "9.0.1" )] )
    };

    await analyzerTest.RunAsync();
  }

  [Theory]
  [InlineData( "LogDebug" )]
  [InlineData( "LogTrace" )]
  [InlineData( "LogInformation" )]
  [InlineData( "LogWarning" )]
  [InlineData( "LogError" )]
  [InlineData( "LogCritical" )]
  public async Task DefaultLoggingMethods_InProgramClassInAllowedNameSpace_NoDiagnostic( string methodName )
  {
    var text = $$"""
                 using Microsoft.Extensions.Logging;

                 public class Program
                 {
                     private readonly ILogger<Program> _logger;
                 
                     public Program(ILogger<Program> logger) {
                       _logger = logger;
                     }
                 
                     public void DoSomething()
                     {
                       _logger.{{methodName}}("Hello World!");
                     }
                 }
                 """;

    var analyzerTest = new CSharpAnalyzerTest<DoNotUseDefaultLoggingMethodsAnalyzer, DefaultVerifier>
    {
      TestCode = text,
      ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        .AddPackages( [new PackageIdentity( "Microsoft.Extensions.Logging", "9.0.1" )] )
    };

    await analyzerTest.RunAsync();
  }

  [Theory]
  [InlineData( "LogDebug" )]
  [InlineData( "LogTrace" )]
  [InlineData( "LogInformation" )]
  [InlineData( "LogWarning" )]
  [InlineData( "LogError" )]
  [InlineData( "LogCritical" )]
  public async Task DefaultLoggingMethods_InLoggerExtensionsClassInAllowedNamespace_NoDiagnostic( string methodName )
  {
    var text = $$"""
                 using Microsoft.Extensions.Logging;

                 namespace Moxion.Application.Extensions;

                 public static class LoggerExtensions
                 {
                     public static void LogSomething(this ILogger logger, string message)
                     {
                       logger.{{methodName}}(message);
                     }
                 }
                 """;

    var analyzerTest = new CSharpAnalyzerTest<DoNotUseDefaultLoggingMethodsAnalyzer, DefaultVerifier>
    {
      TestCode = text,
      ReferenceAssemblies = ReferenceAssemblies.Net.Net80
        .AddPackages( [new PackageIdentity( "Microsoft.Extensions.Logging", "9.0.1" )] )
    };

    await analyzerTest.RunAsync();
  }
}