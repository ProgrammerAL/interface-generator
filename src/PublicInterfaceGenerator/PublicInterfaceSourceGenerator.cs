﻿using System.Text;

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.GeneratorParsers;

namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator;

[Generator]
public class PublicInterfaceSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var interfacesToGenerate =
            context.SyntaxProvider
            .ForAttributeWithMetadataName(
                GenerateInterfaceAttribute.Constants.GenerateInterfaceAttributeFullName,
                predicate: static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
                transform: ClassParser.GetTypeToGenerate);

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
