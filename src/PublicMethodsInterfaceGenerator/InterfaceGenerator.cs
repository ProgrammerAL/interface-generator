using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator.GeneratorParsers;

namespace ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator;

[Generator]
public class InterfaceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Add the marker attribute
        context.RegisterPostInitializationOutput(static ctx => ctx.AddSource(
            "SimpleInterfaceAttribute.g.cs", SourceText.From(SourceGenerationHelper.AttributeClassCode, Encoding.UTF8)));

        IncrementalValuesProvider<SimpleInterfaceToGenerate?> interfacesToGenerate =
            context.SyntaxProvider
            .ForAttributeWithMetadataName(
                SourceGenerationHelper.GenerateSimpleInterfaceAttributeFullName,
                predicate: static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
                transform: ClassParser.GetTypeToGenerate)
            .Where(static m => m is not null);

        // Generate source code for each interface
        context.RegisterSourceOutput(interfacesToGenerate,
            static (spc, source) => GenerateInterface(source, spc));
    }

    private static void GenerateInterface(in SimpleInterfaceToGenerate? interfaceToGenerate, SourceProductionContext context)
    {
        //System.Diagnostics.Debugger.Launch();

        if (interfaceToGenerate is null)
        {
            return;
        }

        var codeString = SourceGenerationHelper.GenerateInterface(in interfaceToGenerate);
        var sourceText = SourceText.From(codeString, Encoding.UTF8);
        context.AddSource($"{interfaceToGenerate.InterfaceName}.g.cs", sourceText);
    }
}
