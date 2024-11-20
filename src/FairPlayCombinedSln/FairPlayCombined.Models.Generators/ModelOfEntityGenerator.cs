using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Xml.Serialization;

namespace FairPlayCombined.Models.Generators
{
    [Generator]
    public class ModelOfEntityGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
#if DEBUG

#pragma warning disable S125 // Sections of code should not be commented out
            //System.Diagnostics.Debugger.Launch();
#endif

            try
            {
                var provider = context.SyntaxProvider.CreateSyntaxProvider(
                    predicate: static (node, token) => node is ClassDeclarationSyntax,
                    transform: static (ctx, token) => (ClassDeclarationSyntax)ctx.Node
                    ).Where(node => node is not null);

                var compilation = context.CompilationProvider.Combine(provider.Collect());

                context.RegisterSourceOutput(compilation, Execute);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        private static void Execute(SourceProductionContext context, (Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right) tuple)
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
                        foreach (var attributeData in attributes)
                        {
                            if (attributeData.AttributeClass?.Name == "ModelOfEntityAttribute")
                            {
                                // Extract the type argument
                                var modelGenericType = attributeData.AttributeClass.TypeArguments[0];

                                // Extract the constructor argument (e.g., "dbo.AspNetUsers")
                                if (attributeData.ConstructorArguments.Length > 0)
                                {
                                    var constructorArg = attributeData.ConstructorArguments[0].Value?.ToString();
                                    // Log or use the constructorArg value
                                    Debug.WriteLine($"Constructor Argument: {constructorArg}");
#pragma warning disable RS1035 // Do not use APIs banned for analyzers
                                    var modelSourceFilePath = syntax.GetLocation().SourceTree!.FilePath;
                                    var searchPath = Directory.GetParent(modelSourceFilePath).Parent.FullName;
                                    var dacPacFiles = Directory.GetFiles(searchPath, "model.xml", SearchOption.AllDirectories);
                                    if (dacPacFiles?.Length > 0)
                                    {
                                        var firstFile = dacPacFiles[0];
                                        using var stream = File.Open(firstFile, FileMode.Open);
                                        XmlSerializer xmlSerializer = new(typeof(DataSchemaModel));
                                        DataSchemaModel? dataSchemaModel = xmlSerializer.Deserialize(stream) as DataSchemaModel;
                                        if (dataSchemaModel != null)
                                        {
                                            var tables = dataSchemaModel.Model.Where(p => p.Type == "SqlTable");
                                            var matchingTable = tables.Where(p => p.Name == constructorArg);
                                            var columnsRelationship = matchingTable.First().Relationship.Single(p => p.Name == "Columns");
                                            var columnsEntries = columnsRelationship.Entry;
                                            foreach (var columnEntry in columnsEntries)
                                            {
                                                string columnName = columnEntry.Element.Name.Replace(constructorArg, String.Empty)
                                                    .Replace("[", String.Empty).Replace("]", String.Empty)
                                                    .TrimStart('.');
                                            }
                                            Debug.WriteLine(dataSchemaModel.DspName);
                                        }
                                    }
#pragma warning restore RS1035 // Do not use APIs banned for analyzers
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}