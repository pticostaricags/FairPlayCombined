using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Text;

namespace FairPlayCombined.Models.Generators
{
    [Generator]
    public class ModelLocalizerIncrementalGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
#if DEBUG

#pragma warning disable S125 // Sections of code should not be commented out
            System.Diagnostics.Debugger.Launch();
#endif
            // Do a simple filter for enums
            IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations =
                context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s), // select enums with attributes
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx)) // sect the enum with the [EnumExtensions] attribute
                .Where(static m => m is not null)!; // filter out attributed enums that we don't care about
#pragma warning restore S125 // Sections of code should not be commented out

            // Combine the selected interfaces with the `Compilation`
            IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)>
                compilationAndClasses
                = context.CompilationProvider.Combine(classDeclarations.Collect());

            // Generate the source using the compilation and classes
            context.RegisterSourceOutput(compilationAndClasses,
                static (spc, source) => Execute(source.Item1, source.Item2, spc));
        }

        private static ClassDeclarationSyntax GetSemanticTargetForGeneration(GeneratorSyntaxContext generatorSyntaxContext)
        {
            var classDeclarationSyntax = generatorSyntaxContext.Node as ClassDeclarationSyntax;
            return classDeclarationSyntax!;
        }

        private static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
            {
                foreach (var singleAttributeList in classDeclarationSyntax.AttributeLists)
                {
                    foreach (var singleAttribute in singleAttributeList.Attributes)
                    {
                        if ((singleAttribute.Name) is GenericNameSyntax identifierNameSyntax)
                        {
                            string identifierText = identifierNameSyntax!.Identifier.Text;
                            if (identifierText == "LocalizerOfT")
                                return true;
                        }
                    }
                }
            }
            return false;
        }

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        static void Execute(Compilation compilation,
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
            ImmutableArray<ClassDeclarationSyntax> classesDeclarationSyntax, SourceProductionContext context)
        {
            foreach (var singleClassDeclarationSyntax in classesDeclarationSyntax)
            {
                foreach (var singleAttributeList in singleClassDeclarationSyntax.AttributeLists)
                {
                    foreach (var singleAttribute in singleAttributeList.Attributes)
                    {
                        if ((singleAttribute.Name) is GenericNameSyntax identifierNameSyntax)
                        {
                            string identifierText = identifierNameSyntax!.Identifier.Text;
                            if (identifierText == "LocalizerOfT")
                            {
                                var typeArgument = identifierNameSyntax!.TypeArgumentList;
                                var typeArgumentIdentifier = typeArgument!.Arguments[0] as IdentifierNameSyntax;
                                var typeArgumentName = typeArgumentIdentifier!.Identifier!.ValueText;
                                var newClassName = $"{typeArgumentName}Localizer";
                                var indexOfLastModel = typeArgumentName.LastIndexOf("Model");
                                var entityName = typeArgumentName.Substring(6, indexOfLastModel - 6);
                                string namespaceValue = $"FairPlayShop.Models.{entityName}";
                                var fullyQualifiedMetadataName = $"{namespaceValue}.{typeArgumentName}";
                                var typedSymbol = compilation.GetTypeByMetadataName(fullyQualifiedMetadataName);
                                var properties = typedSymbol!.GetMembers().Where(p => p.Kind == SymbolKind.Property);
                                StringBuilder stringBuilder = new();
                                stringBuilder.AppendLine("using Microsoft.Extensions.Localization;");
                                stringBuilder.AppendLine("using FairPlayShop.Common.CustomAttributes;");
                                stringBuilder.AppendLine($"namespace {namespaceValue};");
                                stringBuilder.AppendLine($"public partial class {newClassName}");
                                stringBuilder.AppendLine("{");
                                stringBuilder.AppendLine($"public static IStringLocalizer<{newClassName}> Localizer {{ get; set; }}");
                                foreach (var singleProperty in properties)
                                {
                                    var propertyAttributes = singleProperty.GetAttributes();
                                    foreach (var singlePropertyAttribute in propertyAttributes)
                                    {
                                        var singlePropertyMetadataName = singlePropertyAttribute!.AttributeClass!.MetadataName;
                                        switch (singlePropertyMetadataName)
                                        {
                                            case "RequiredAttribute":
                                                stringBuilder.AppendLine("[ResourceKey(defaultValue: \"{0} is required\")]");
                                                stringBuilder.AppendLine($"public const string {singleProperty.Name}_RequiredTextKey = \"{singleProperty.Name}_RequiredText\";");
                                                stringBuilder.AppendLine($"public static string {singleProperty.Name}_Required => Localizer[{singleProperty.Name}_RequiredTextKey];");
                                                break;
                                            case "StringLengthAttribute":
                                                stringBuilder.AppendLine("[ResourceKey(defaultValue: \"{0} cannot have more than {1} characters\")]");
                                                stringBuilder.AppendLine($"public const string {singleProperty.Name}StringLengthTextKey = \"{singleProperty.Name}StringLengthText\";");
                                                stringBuilder.AppendLine($"public static string {singleProperty.Name}_StringLength => Localizer[{singleProperty.Name}StringLengthTextKey];");
                                                break;
                                            case "EmailAddressAttribute":
                                                stringBuilder.AppendLine("[ResourceKey(defaultValue: \"{0} must have a valid Email Address format\")]");
                                                stringBuilder.AppendLine($"public const string {singleProperty.Name}EmailAddressTextKey = \"{singleProperty.Name}EmailAddressText\";");
                                                stringBuilder.AppendLine($"public static string {singleProperty.Name}_EmailAddress => Localizer[{singleProperty.Name}EmailAddressTextKey];");
                                                break;
                                            case "PhoneAttribute":
                                                stringBuilder.AppendLine("[ResourceKey(defaultValue: \"{0} must have a valid Phone format\")]");
                                                stringBuilder.AppendLine($"public const string {singleProperty.Name}PhoneTextKey = \"{singleProperty.Name}PhoneText\";");
                                                stringBuilder.AppendLine($"public static string {singleProperty.Name}_Phone => Localizer[{singleProperty.Name}PhoneTextKey];");
                                                break;
                                            case "LengthAttribute":
                                                stringBuilder.AppendLine("[ResourceKey(defaultValue: \"{0} must have between {1} and {2} items\")]");
                                                stringBuilder.AppendLine($"public const string {singleProperty.Name}LengthTextKey = \"{singleProperty.Name}LengthText\";");
                                                stringBuilder.AppendLine($"public static string {singleProperty.Name}_Length => Localizer[{singleProperty.Name}LengthTextKey];");
                                                break;
                                            case "RangeAttribute":
                                                stringBuilder.AppendLine("[ResourceKey(defaultValue: \"{0} must have a value between {1} and {2}\")]");
                                                stringBuilder.AppendLine($"public const string {singleProperty.Name}RangeTextKey = \"{singleProperty.Name}RangeText\";");
                                                stringBuilder.AppendLine($"public static string {singleProperty.Name}_Range => Localizer[{singleProperty.Name}RangeTextKey];");
                                                break;
                                        }
                                    }
                                }
                                stringBuilder.AppendLine("}");
                                context.AddSource($"{newClassName}.g.cs",
                            SourceText.From(stringBuilder.ToString(), Encoding.UTF8));
                            }
                        }
                    }
                }
            }
        }
    }
}
