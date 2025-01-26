using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Moxion.CodeAnalysis.Semantics;

[DiagnosticAnalyzer( LanguageNames.CSharp )]
public class LogStartMustAppearBeforeLogEndAnalyzer : DiagnosticAnalyzer
{
  private const string DiagnosticId = "MX0004";
  private const string Category = "Logging";

  private static readonly LocalizableString Title = new LocalizableResourceString(
    nameof(Resources.MX0004Title),
    Resources.ResourceManager,
    typeof(Resources)
  );

  private static readonly LocalizableString MessageFormat = new LocalizableResourceString(
    nameof(Resources.MX0004MessageFormat),
    Resources.ResourceManager,
    typeof(Resources)
  );

  private static readonly LocalizableString Description = new LocalizableResourceString(
    nameof(Resources.MX0004Description),
    Resources.ResourceManager,
    typeof(Resources)
  );


  private static readonly DiagnosticDescriptor Rule = new(
    DiagnosticId,
    Title,
    MessageFormat,
    Category,
    DiagnosticSeverity.Error,
    isEnabledByDefault: true,
    description: Description
  );

  public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create( Rule );

  public override void Initialize( AnalysisContext context )
  {
    context.ConfigureGeneratedCodeAnalysis( GeneratedCodeAnalysisFlags.None );
    context.EnableConcurrentExecution();
    context.RegisterSyntaxNodeAction( AnalyzeSyntaxNode, SyntaxKind.MethodDeclaration );
  }

  private static void AnalyzeSyntaxNode( SyntaxNodeAnalysisContext context )
  {
    const string loggerExtensionsClassName = "global::Moxion.Application.Extensions.LoggerExtensions";
    const string logStart = "LogStart";
    const string logEnd = "LogEnd";

    if (context.Node is not MethodDeclarationSyntax methodDeclaration)
    {
      return;
    }

    if (methodDeclaration.Body is not { } blockSyntax)
    {
      return;
    }

    var semanticModel = context.SemanticModel;
    var isLogEndFound = false;

    // Traverse the method body to get LogStart and LogEnd calls
    foreach (var statement in blockSyntax.Statements)
    {
      if (statement is not ExpressionStatementSyntax expressionStatement)
      {
        continue;
      }

      if (expressionStatement.Expression is not InvocationExpressionSyntax invocation)
      {
        continue;
      }

      if (semanticModel.GetSymbolInfo( invocation ).Symbol is not IMethodSymbol symbol)
      {
        continue;
      }

      var containingClassName = symbol.ContainingType?.ToDisplayString( SymbolDisplayFormat.FullyQualifiedFormat );

      if (containingClassName is not loggerExtensionsClassName)
      {
        continue;
      }

      if (symbol.Name is logEnd)
      {
        isLogEndFound = true;
        continue;
      }

      if (symbol.Name is not logStart)
      {
        continue;
      }

      // If LogStart appear before LogEnd, no need to report diagnostic
      if (!isLogEndFound)
      {
        return;
      }

      var diagnostic = Diagnostic.Create( Rule, invocation.GetLocation() );
      context.ReportDiagnostic( diagnostic );
    }
  }
}