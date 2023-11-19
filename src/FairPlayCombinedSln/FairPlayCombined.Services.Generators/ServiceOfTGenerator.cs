using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Immutable;

namespace FairPlayCombined.Services.Generators
{
    [Generator]
    public class ServiceOfTGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
#if DEBUG

#pragma warning disable S125 // Sections of code should not be commented out
            //System.Diagnostics.Debugger.Launch();
#endif
            var provider = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: static (node, token) => node is ClassDeclarationSyntax,
                transform: static (ctx, token) => (ClassDeclarationSyntax)ctx.Node
                ).Where(node => node is not null);

            var compilation = context.CompilationProvider.Combine(provider.Collect());

            context.RegisterSourceOutput(compilation, Execute);
        }

        private void Execute(SourceProductionContext context, (Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right) tuple)
        {
            var (compilation, list) = tuple;
            foreach (var syntax in list)
            {
                var symbol = compilation
                    .GetSemanticModel(syntax.SyntaxTree)
                    .GetDeclaredSymbol(syntax) as INamedTypeSymbol;
                if (symbol != null)
                {
                    var attributes = symbol.GetAttributes();
                    if (attributes.Length > 0)
                    {
                        foreach (var attribute in attributes)
                        {
                            var attributeClass = attribute.AttributeClass as INamedTypeSymbol;
                            if (attributeClass?.Name == "ServiceOfTAttribute")
                            {
                                var createActivityModel = attributeClass.TypeArguments[0];
                                var updateActivityModel = attributeClass.TypeArguments[1];
                                var listActivityModel = attributeClass.TypeArguments[2];
                                if (createActivityModel != null &&
                                    updateActivityModel != null &&
                                    listActivityModel != null)
                                {
                                    var namespaceStrng =
                                        createActivityModel!
                                        .ContainingNamespace.ToString();
                                    var createtypeName = createActivityModel!.Name;
                                    var fullQualifiedName = $"{namespaceStrng}.{createtypeName}";
                                    var symbolNamespace = symbol.ContainingNamespace.ToString();
                                    string classContent = $$"""
                                        using System.Threading.Tasks;
                                        using {{createActivityModel.ContainingNamespace.ToString()}};
                                        namespace {{symbolNamespace}};
                                        public partial class {{symbol.Name}}
                                        {
                                            public void {{createtypeName}}Async(
                                            {{createtypeName}} createModel,
                                            CancellationToken cancellationToken
                                            )
                                            {
                                            }
                                        }
                                        """;
                                    context.AddSource($"{symbol.Name}.g.cs", classContent);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
