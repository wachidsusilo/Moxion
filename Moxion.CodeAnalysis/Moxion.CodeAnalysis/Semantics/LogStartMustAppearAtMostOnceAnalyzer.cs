using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Moxion.CodeAnalysis.Semantics;

[DiagnosticAnalyzer( LanguageNames.CSharp )]
public class LogStartMustAppearAtMostOnceAnalyzer : DiagnosticAnalyzer
{
  private const string DiagnosticId = "MX0002";
  private const string Category = "Logging";

  private static readonly LocalizableString Title = new LocalizableResourceString(
    nameof(Resources.MX0002Title),
    Resources.ResourceManager,
    typeof(Resources)
  );

  private static readonly LocalizableString MessageFormat = new LocalizableResourceString(
    nameof(Resources.MX0002MessageFormat),
    Resources.ResourceManager,
    typeof(Resources)
  );

  private static readonly LocalizableString Description = new LocalizableResourceString(
    nameof(Resources.MX0002Description),
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
    context.RegisterSyntaxNodeAction( AnalyzeSyntaxNode, SyntaxKind.MethodDeclaration );
  }

  private static void AnalyzeSyntaxNode( SyntaxNodeAnalysisContext context )
  {
    const string loggerExtensionsClassName = "global::Moxion.Application.Extensions.LoggerExtensions";
    const string logStart = "LogStart";

    if (context.Node is not MethodDeclarationSyntax methodDeclaration)
    {
      return;
    }

    if (methodDeclaration.Body is not { } blockSyntax)
    {
      return;
    }

    var semanticModel = context.SemanticModel;
    var logStartCalls = new List<InvocationExpressionSyntax>();

    // Collect LogStart calls
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

      if (symbol.Name is logStart)
      {
        logStartCalls.Add( invocation );
      }
    }

    if (logStartCalls.Count <= 1)
    {
      return;
    }

    // If more than one LogStart is found, report a diagnostic
    var diagnostic = Diagnostic.Create( Rule, logStartCalls.Last().GetLocation() );
    context.ReportDiagnostic( diagnostic );
  }
}