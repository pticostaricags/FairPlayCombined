using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
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

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        private void Execute(SourceProductionContext context, (Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right) tuple)
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
        {
            var (compilation, list) = tuple;
            foreach (var syntax in list)
            {
                if (compilation
                    .GetSemanticModel(syntax.SyntaxTree)
                    .GetDeclaredSymbol(syntax) is INamedTypeSymbol symbol)
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
                                var paginationRequestArgument = attributeClass.TypeArguments[5];
                                var paginationResultArgument = attributeClass.TypeArguments[6];
                                if (createModel != null &&
                                    updateModel != null &&
                                    listActivityModel != null)
                                {
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
                                    StringBuilder createAssignment = new();
                                    foreach (var property in propertiesInBothCreateModelAndDbEntity)
                                    {
                                        createAssignment.AppendLine($"{property} = createModel.{property},");
                                    }
                                    StringBuilder listAssignment = new();
                                    foreach (var property in propertiesInBothListModelAndDbEntity)
                                    {
                                        listAssignment.AppendLine($"{property} = p.{property},");
                                    }
                                    var primaryKeyProperty =
                                        dbEntityArgument.GetMembers()
                                        .Where(p => p.Kind == SymbolKind.Property
                                        &&
                                        (p.GetAttributes().Any(x => x.AttributeClass != null
                                        && x.AttributeClass.Name == "KeyAttribute"))
                                        ).SingleOrDefault() as IPropertySymbol;
                                    string classContent = $$"""
                                        using System.Threading.Tasks;
                                        using {{createModel.ContainingNamespace}};
                                        using {{dbContextArgument.ContainingNamespace}};
                                        using Microsoft.EntityFrameworkCore;
                                        using {{dbEntityArgument.ContainingNamespace}};
                                        using {{paginationResultArgument.ContainingNamespace}};
                                        using System.Linq.Dynamic.Core;
                                        using Microsoft.Extensions.Logging;
                                        namespace {{symbolNamespace}};
                                        public partial class {{symbol.Name}}(
                                        IDbContextFactory<{{dbContextArgument.Name}}> dbContextFactory,
                                        ILogger<{{symbol.Name}}> logger
                                        )
                                        {
                                            public async Task<{{primaryKeyProperty!.Type.ToDisplayString()}}> Create{{entityName}}Async(
                                            {{createModel.Name}} createModel,
                                            CancellationToken cancellationToken
                                            )
                                            {
                                                logger.LogInformation(message: "Start of method: {methodName}", nameof(Create{{entityName}}Async));
                                                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                                                {{entityName}} entity = new()
                                                {
                                                    {{createAssignment}}
                                                };
                                                await dbContext.{{entityName}}.AddAsync(entity, cancellationToken);
                                                await dbContext.SaveChangesAsync(cancellationToken);
                                                return entity.{{primaryKeyProperty.Name}};
                                            }

                                            public async Task<{{listActivityModel.Name}}[]> GetAll{{entityName}}Async(
                                            CancellationToken cancellationToken)
                                            {
                                                logger.LogInformation(message: "Start of method: {methodName}", nameof(GetAll{{entityName}}Async));
                                                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                                                var result = await dbContext.{{entityName}}
                                                .AsNoTracking()
                                                .Select(p=>new {{listActivityModel.Name}}()
                                                {
                                                    {{listAssignment}}
                                                }).ToArrayAsync(cancellationToken);
                                                return result;
                                            }

                                            public async Task<{{listActivityModel.Name}}> Get{{entityName}}ByIdAsync(
                                            {{primaryKeyProperty!.Type.ToDisplayString()}} id,
                                            CancellationToken cancellationToken)
                                            {
                                                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                                                var result = await dbContext.{{entityName}}
                                                .Where(p=>p.{{primaryKeyProperty.Name}}==id)
                                                .AsNoTracking()
                                                .Select(p=>new {{listActivityModel.Name}}
                                                {
                                                    {{listAssignment}}
                                                })
                                                .SingleOrDefaultAsync(cancellationToken);
                                                return result;
                                            }

                                            public async Task Delete{{entityName}}ByIdAsync(
                                            {{primaryKeyProperty!.Type.ToDisplayString()}} id,
                                            CancellationToken cancellationToken)
                                            {
                                                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                                                await dbContext.{{entityName}}
                                                .Where(p=> p.{{primaryKeyProperty.Name}} == id)
                                                .ExecuteDeleteAsync(cancellationToken);
                                            }

                                            public async Task<{{paginationResultArgument.Name}}<{{listActivityModel.Name}}>> GetPaginated{{entityName}}Async(
                                            {{paginationRequestArgument.Name}} paginationRequest,
                                            CancellationToken cancellationToken
                                            )
                                            {
                                                logger.LogInformation(message: "Start of method: {methodName}", nameof(GetPaginated{{entityName}}Async));
                                                {{paginationResultArgument.Name}}<{{listActivityModel.Name}}> result=new();
                                                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                                                string orderByString = string.Empty;
                                                if (paginationRequest.SortingItems?.Length > 0)
                                                    orderByString =
                                                        String.Join(",",
                                                        paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
                                                var query = dbContext.{{entityName}}
                                                    .Select(p=>new {{listActivityModel}}
                                                    {
                                                        {{listAssignment}}
                                                    });
                                                if (!String.IsNullOrEmpty(orderByString))
                                                    query = query.OrderBy(orderByString);
                                                result.TotalItems = await query.CountAsync(cancellationToken);
                                                result.PageSize = paginationRequest.PageSize;
                                                result.TotalPages = (int)Math.Ceiling((decimal)result.TotalItems / result.PageSize);
                                                result.Items = await query
                                                .Skip(paginationRequest.StartIndex)
                                                .Take(paginationRequest.PageSize)
                                                .ToArrayAsync(cancellationToken);
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
