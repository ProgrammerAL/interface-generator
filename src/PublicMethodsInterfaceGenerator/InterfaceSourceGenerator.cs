using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using ProgrammerAl.SourceGenerators.InterfaceGenerator.GeneratorParsers;

namespace ProgrammerAl.SourceGenerators.InterfaceGenerator;

[Generator]
public class InterfaceSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Add the marker attribute
        context.RegisterPostInitializationOutput(static ctx => ctx.AddSource(
            $"{SourceGenerationHelper.GenerateInterfaceAttributeName}.g.cs", SourceText.From(SourceGenerationHelper.AttributeClassCode, Encoding.UTF8)));

        IncrementalValuesProvider<InterfaceToGenerateInfo?> interfacesToGenerate =
            context.SyntaxProvider
            .ForAttributeWithMetadataName(
                SourceGenerationHelper.GenerateInterfaceAttributeFullName,
                predicate: static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
                transform: ClassParser.GetTypeToGenerate)
            .Where(static m => m is not null);

        // Generate source code for each interface
        context.RegisterSourceOutput(interfacesToGenerate,
            static (spc, source) => GenerateInterface(source, spc));
    }

    private static void GenerateInterface(in InterfaceToGenerateInfo? interfaceInfo, SourceProductionContext context)
    {
        if (interfaceInfo is null)
        {
            return;
        }

        var codeString = SourceGenerationHelper.GenerateInterface(in interfaceInfo);
        var sourceText = SourceText.From(codeString, Encoding.UTF8);
        context.AddSource($"{interfaceInfo.InterfaceName}.g.cs", sourceText);
    }
}
