using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Text;

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
                                var createModel = attributeClass.TypeArguments[0];
                                var updateModel = attributeClass.TypeArguments[1];
                                var listActivityModel = attributeClass.TypeArguments[2];
                                var dbContextArgument = attributeClass.TypeArguments[3];
                                var dbEntityArgument = attributeClass.TypeArguments[4];
                                if (createModel != null &&
                                    updateModel != null &&
                                    listActivityModel != null)
                                {
                                    var namespaceStrng =
                                        createModel!
                                        .ContainingNamespace.ToString();
                                    var createtypeName = createModel!.Name;
                                    var fullQualifiedName = $"{namespaceStrng}.{createtypeName}";
                                    var symbolNamespace = symbol.ContainingNamespace.ToString();
                                    var entityName = dbEntityArgument.Name;
                                    var createModelProperties = createModel.GetMembers()
                                        .Where(p => p.Kind == SymbolKind.Property)
                                        .Select(p => p.Name);
                                    var listModelProperties = listActivityModel.GetMembers()
                                        .Where(p => p.Kind == SymbolKind.Property)
                                        .Select(p => p.Name);
                                    var dbEntityProperties =
                                        dbEntityArgument.GetMembers()
                                        .Where(p => p.Kind == SymbolKind.Property)
                                        .Select(p => p.Name);
                                    var propertiesInBothCreateModelAndDbEntity =
                                        createModelProperties.Join(dbEntityProperties,
                                        Inner => Inner, Outer => Outer,
                                        (a, b) => a);
                                    var propertiesInBothListModelAndDbEntity =
                                        listModelProperties.Join(dbEntityProperties,
                                        Inner => Inner, Outer => Outer,
                                        (a, b) => a);
                                    StringBuilder createAssignment = new StringBuilder();
                                    foreach (var property in propertiesInBothCreateModelAndDbEntity)
                                    {
                                        createAssignment.AppendLine($"{property} = createModel.{property},");
                                    };
                                    StringBuilder listAssignment = new StringBuilder();
                                    foreach (var property in propertiesInBothListModelAndDbEntity)
                                    {
                                        listAssignment.AppendLine($"{property} = p.{property},");
                                    };
                                    string classContent = $$"""
                                        using System.Threading.Tasks;
                                        using {{createModel.ContainingNamespace.ToString()}};
                                        using {{dbContextArgument.ContainingNamespace}};
                                        using Microsoft.EntityFrameworkCore;
                                        using {{dbEntityArgument.ContainingNamespace}};
                                        namespace {{symbolNamespace}};
                                        public partial class {{symbol.Name}}(
                                        IDbContextFactory<{{dbContextArgument.Name}}> dbContextFactory
                                        )
                                        {
                                            public async Task {{createtypeName}}Async(
                                            {{createtypeName}} createModel,
                                            CancellationToken cancellationToken
                                            )
                                            {
                                                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                                                {{entityName}} entity = new()
                                                {
                                                    {{createAssignment.ToString()}}
                                                };
                                                await dbContext.{{entityName}}.AddAsync(entity, cancellationToken);
                                                await dbContext.SaveChangesAsync(cancellationToken);
                                            }

                                            public async Task<{{listActivityModel.Name}}[]> GetAll{{entityName}}Async(
                                            CancellationToken cancellationToken)
                                            {
                                                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                                                var result = await dbContext.{{entityName}}
                                                .Select(p=>new {{listActivityModel.Name}}()
                                                {
                                                    {{listAssignment.ToString()}}
                                                }).ToArrayAsync(cancellationToken);
                                                return result;
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
