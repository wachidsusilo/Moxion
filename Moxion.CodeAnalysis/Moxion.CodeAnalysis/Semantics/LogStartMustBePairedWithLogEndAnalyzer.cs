using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using DiagnosticDescriptor = Microsoft.CodeAnalysis.DiagnosticDescriptor;
using DiagnosticSeverity = Microsoft.CodeAnalysis.DiagnosticSeverity;
using LanguageNames = Microsoft.CodeAnalysis.LanguageNames;
using LocalizableString = Microsoft.CodeAnalysis.LocalizableString;
using SyntaxKind = Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace Moxion.CodeAnalysis.Semantics;

[DiagnosticAnalyzer( LanguageNames.CSharp )]
public class LogStartMustBePairedWithLogEndAnalyzer : DiagnosticAnalyzer
{
  private const string DiagnosticId = "MX0003";
  private const string Category = "Logging";

  private static readonly LocalizableString Title = new LocalizableResourceString(
    nameof(Resources.MX0003Title),
    Resources.ResourceManager,
    typeof(Resources)
  );

  private static readonly LocalizableString MessageFormat = new LocalizableResourceString(
    nameof(Resources.MX0003MessageFormat),
    Resources.ResourceManager,
    typeof(Resources)
  );

  private static readonly LocalizableString Description = new LocalizableResourceString(
    nameof(Resources.MX0003Description),
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
    var logStartInvocations = new List<InvocationExpressionSyntax>();
    var logEndInvocations = new List<InvocationExpressionSyntax>();

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

      if (symbol.Name is logStart)
      {
        logStartInvocations.Add( invocation );
        continue;
      }

      if (symbol.Name is logEnd)
      {
        logEndInvocations.Add( invocation );
      }
    }

    // If there is no LogStart or is already paired with LogEnd, no need to report diagnostic
    if (logStartInvocations.Count == 0 || logEndInvocations.Count > 0)
    {
      return;
    }

    var diagnostic = Diagnostic.Create( Rule, logStartInvocations.Last().GetLocation() );
    context.ReportDiagnostic( diagnostic );
  }
}