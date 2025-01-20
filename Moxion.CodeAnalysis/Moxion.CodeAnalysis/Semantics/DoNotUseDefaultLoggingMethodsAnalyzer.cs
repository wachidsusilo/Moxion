using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Moxion.CodeAnalysis.Semantics;

[DiagnosticAnalyzer( LanguageNames.CSharp )]
public class DoNotUseDefaultLoggingMethodsAnalyzer : DiagnosticAnalyzer
{
  private const string DiagnosticId = "MX0001";
  private const string Category = "Logging";

  private static readonly LocalizableString Title = new LocalizableResourceString(
    nameof(Resources.MX0001Title),
    Resources.ResourceManager,
    typeof(Resources)
  );

  private static readonly LocalizableString MessageFormat = new LocalizableResourceString(
    nameof(Resources.MX0001MessageFormat),
    Resources.ResourceManager,
    typeof(Resources)
  );

  private static readonly LocalizableString Description = new LocalizableResourceString(
    nameof(Resources.MX0001Description),
    Resources.ResourceManager,
    typeof(Resources)
  );

  private static readonly DiagnosticDescriptor Rule = new(
    DiagnosticId,
    Title,
    MessageFormat,
    Category,
    DiagnosticSeverity.Error,
    true,
    Description
  );

  public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create( Rule );

  public override void Initialize( AnalysisContext context )
  {
    context.ConfigureGeneratedCodeAnalysis( GeneratedCodeAnalysisFlags.None );
    context.EnableConcurrentExecution();
    context.RegisterSyntaxNodeAction( AnalyzeSyntaxNode, SyntaxKind.InvocationExpression );
  }

  private static void AnalyzeSyntaxNode( SyntaxNodeAnalysisContext context )
  {
    const string containingClassName = "global::Microsoft.Extensions.Logging.LoggerExtensions";

    if (context.Node is not InvocationExpressionSyntax invocationSyntax)
    {
      return;
    }

    var symbol = context.SemanticModel.GetSymbolInfo( invocationSyntax ).Symbol;

    if (symbol is not IMethodSymbol methodSymbol)
    {
      return;
    }

    // Logging is an extension method
    if (methodSymbol.MethodKind is not MethodKind.ReducedExtension)
    {
      return;
    }

    var symbolSourceClassName = methodSymbol.ContainingType.ToDisplayString( SymbolDisplayFormat.FullyQualifiedFormat );

    // Make sure that the invoked method is defined in Microsoft.Extensions.Logging.LoggerExtensions
    if (symbolSourceClassName is not containingClassName)
    {
      return;
    }

    // Make sure that the method is invoked from other method
    if (context.ContainingSymbol?.ContainingType is not { } containingType)
    {
      return;
    }

    var callerClassName = containingType.ToDisplayString( SymbolDisplayFormat.FullyQualifiedFormat );

    if (IsLogMethodAllowed( callerClassName, methodSymbol.Name ))
    {
      return;
    }

    var diagnostic = Diagnostic.Create( Rule, invocationSyntax.GetLocation(), methodSymbol.Name );

    context.ReportDiagnostic( diagnostic );
  }

  private static bool IsLogMethodAllowed( in string callerClassName, in string methodName )
  {
    const string programClassName = "global::Program";
    const string loggerExtensionsClassName = "global::Moxion.Application.Extensions.LoggerExtensions";
    const string logStartMethodName = "LogStart";
    const string logCheckPointMethodName = "LogCheckPoint";
    const string logEndMethodName = "LogEnd";

    // Allow if the default logging method is called from Program.cs or Moxion.Application.Extensions.LoggerExtensions
    if (callerClassName is programClassName or loggerExtensionsClassName)
    {
      return true;
    }

    // Only allow custom logging methods
    return methodName is logStartMethodName or logCheckPointMethodName or logEndMethodName;
  }
}