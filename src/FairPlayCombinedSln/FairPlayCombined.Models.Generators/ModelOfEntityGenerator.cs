/*
 * This class is experimental. 
 * The purpose is to find the best way to automatically generate the models classes
 * using Source Generators.
 * We are purposely supressing banned apis messages in order to research.
 * The final version of this class should completely avoid usage of banned APIs
 */
#pragma warning disable RS1035 // Do not use APIs banned for analyzers
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace FairPlayCombined.Models.Generators;
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
        StringBuilder classBuilder = new();
        var (compilation, list) = tuple;
        foreach (var syntax in list)
        {
            ProcessSyntax(context, classBuilder, compilation, syntax);
        }
    }

    private static void ProcessSyntax(SourceProductionContext context, StringBuilder classBuilder, Compilation compilation, ClassDeclarationSyntax syntax)
    {
        if (compilation
                            .GetSemanticModel(syntax.SyntaxTree)
                            .GetDeclaredSymbol(syntax) is INamedTypeSymbol symbol)
        {
            ProcessSymbol(context, classBuilder, syntax, symbol);
        }
    }

    private static void ProcessSymbol(SourceProductionContext context, StringBuilder classBuilder, ClassDeclarationSyntax syntax, INamedTypeSymbol symbol)
    {
        var attributes = symbol.GetAttributes();
        if (attributes.Length <= 0)
        {
            return;
        }
        foreach (var attributeData in attributes)
        {
            ProcessAttributeClass(context, classBuilder, syntax, symbol, attributeData);
        }
    }

    private static void ProcessAttributeClass(SourceProductionContext context, StringBuilder classBuilder, ClassDeclarationSyntax syntax, INamedTypeSymbol symbol, AttributeData attributeData)
    {
        if ((attributeData.AttributeClass?.Name) != "ModelOfEntityAttribute")
        {
            return;
        }

        // Extract the constructor argument (e.g., "dbo.AspNetUsers")
        if (attributeData.ConstructorArguments.Length <= 0)
        {
            return;
        }
        var constructorArg = attributeData.ConstructorArguments[0].Value?.ToString();
        // Log or use the constructorArg value
        Debug.WriteLine($"Constructor Argument: {constructorArg}");
        ProcessDataSchemaFile(context, classBuilder, syntax, symbol, constructorArg);
    }

    private static void ProcessDataSchemaFile(SourceProductionContext context, StringBuilder classBuilder, ClassDeclarationSyntax syntax, INamedTypeSymbol symbol, string? constructorArg)
    {
        var modelSourceFilePath = syntax.GetLocation().SourceTree!.FilePath;
        var searchPath = Directory.GetParent(modelSourceFilePath).Parent.FullName;
        var dataSchemaFiles = Directory.GetFiles(searchPath, "model.xml", SearchOption.AllDirectories);
        if (dataSchemaFiles?.Length == 0)
        {
            return;
        }
        var firstFile = dataSchemaFiles![0];
        using var stream = File.Open(firstFile, FileMode.Open);
        XmlSerializer xmlSerializer = new(typeof(DataSchemaModel));
        if (xmlSerializer.Deserialize(stream) is not DataSchemaModel dataSchemaModel)
        {
            return;
        }
        var tables = dataSchemaModel.Model.Where(p => p.Type == "SqlTable");
        var matchingTable = tables.Where(p => p.Name == constructorArg);
        var tableName = matchingTable.First().Name;
        string pattern = @"\[\w+\]\.\[(\w+)\]";
        Match match = Regex.Match(tableName, pattern, RegexOptions.None, TimeSpan.FromSeconds(1));

        if (!match.Success)
        {
            return;
        }
        tableName = match.Groups![1].Value;
        var symbolNamespace = symbol.ContainingNamespace.ToString();
        classBuilder.AppendLine("#nullable enable");
        classBuilder.AppendLine("using FairPlayCombined.Common.CustomAttributes;");
        classBuilder.AppendLine("using FairPlayCombined.Common.ValidationAttributes;");
        classBuilder.AppendLine($"namespace {symbolNamespace};");
        classBuilder.AppendLine($"public partial class {tableName}");
        classBuilder.AppendLine("{");
        Debug.WriteLine($"{tableName}");
        var columnsRelationship = matchingTable.First().Relationship.Single(p => p.Name == "Columns");
        var columnsEntries = columnsRelationship.Entry;
        foreach (var columnEntryElement in columnsEntries.Select(c => c.Element))
        {
            ProcessColumnEntry(classBuilder, constructorArg, columnEntryElement, symbol.MemberNames);
        }
        classBuilder.AppendLine("}");
        classBuilder.AppendLine("#nullable disable");
        context.AddSource($"{tableName}.g.cs", classBuilder.ToString());
    }

    private static void ProcessColumnEntry(StringBuilder classBuilder, string? constructorArg, DataSchemaModelElementRelationshipEntryElement columnEntryElement,
        IEnumerable<string> declaredMemberNames)
    {
        string columnName = columnEntryElement.Name.Replace(constructorArg, String.Empty)
            .Replace("[", String.Empty).Replace("]", String.Empty)
            .TrimStart('.');
        if (declaredMemberNames.Any(p => p == columnName))
        {
            //if the defining type already has the property we skip processing the column
            return;
        }
        var isNullableProperty = columnEntryElement.Property?.SingleOrDefault(p => p.Name == "IsNullable");
        if (isNullableProperty != null)
        {
            classBuilder.AppendLine("[CustomRequired]");
        }
        if (columnName.IndexOf("Url") >= 0)
        {
            classBuilder.AppendLine("[NullableUrl]");
        }
        string? propertyType = GetPropertyType(classBuilder, columnEntryElement, columnName);
        if (propertyType?.Length > 0)
        {
            Debug.WriteLine(propertyType);
            classBuilder.AppendLine($"public {propertyType} {columnName} {{ get; set; }}");
        }
        Debug.WriteLine(columnName);
    }

    private static string? GetPropertyType(StringBuilder classBuilder, DataSchemaModelElementRelationshipEntryElement columnEntryElement, string columnName)
    {
        var columnTypeSpecifier = columnEntryElement.Relationship;
        var columnSqlTypeSpecifier = columnTypeSpecifier.Entry.Element;
        var columnSqlTypeSpecifierRelationshipEntry = columnSqlTypeSpecifier.Relationship.Entry;
        var columnTypeReference = columnSqlTypeSpecifierRelationshipEntry.References.Name.Trim('[', ']');
        string? propertyType = null;
        switch (columnTypeReference.ToLowerInvariant())
        {
            case "nvarchar":
                propertyType = "string?";
                if (columnSqlTypeSpecifier.Property.Length > 0)
                {
                    var columnLengthProperty = columnSqlTypeSpecifier
                        .Property.SingleOrDefault(p => p.Name == "Length");
                    if (columnLengthProperty != null)
                    {
                        classBuilder
                            .AppendLine($"[CustomStringLength(maximumLength:{columnLengthProperty.Value})]");
                        Debug.WriteLine($"{columnName}.{columnLengthProperty.Value}");
                    }
                }
                break;
            case "bit":
                propertyType = "bool";
                break;
            case "datetimeoffset":
                propertyType = "DateTimeOffset";
                break;
            case "int":
                propertyType = "int";
                break;
            default:
                Debug.WriteLine("did not find a match");
                break;
        }

        return propertyType;
    }
}
#pragma warning restore RS1035 // Do not use APIs banned for analyzers