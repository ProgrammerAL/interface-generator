using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator;

[Generator]
public class InterfaceGenerator : IIncrementalGenerator
{
    private const string SimpleInterfaceAttributeName = "SimpleInterfaceAttribute";
    private const string SimpleInterfaceAttributeFullName = $"ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator.Extensions.{SimpleInterfaceAttributeName}";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Add the marker attribute
        context.RegisterPostInitializationOutput(static ctx => ctx.AddSource(
            "SimpleInterfaceAttribute.g.cs", SourceText.From(SourceGenerationHelper.AttributeClassCode, Encoding.UTF8)));

        IncrementalValuesProvider<SimpleInterfaceToGenerate?> interfacesToGenerate =
            context.SyntaxProvider
            .ForAttributeWithMetadataName(
                SimpleInterfaceAttributeFullName,
                predicate: static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
                transform: GetTypeToGenerate)
            .Where(static m => m is not null);

        // Generate source code for each interface
        context.RegisterSourceOutput(interfacesToGenerate,
            static (spc, source) => GenerateInterface(source, spc));
    }

    private static SimpleInterfaceToGenerate? GetTypeToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        var symbol = context.TargetSymbol as INamedTypeSymbol;
        if (symbol is null)
        {
            // nothing to do if this type isn't available
            return null;
        }

        ct.ThrowIfCancellationRequested();

        string? interfaceName = null;

        foreach (AttributeData attributeData in symbol.GetAttributes())
        {
            var attributeClassName = attributeData.AttributeClass?.Name;
            if (string.IsNullOrWhiteSpace(attributeClassName)
                || attributeClassName != SimpleInterfaceAttributeName
                || attributeData.AttributeClass!.ToDisplayString() != SimpleInterfaceAttributeFullName)
            {
                continue;
            }

            foreach (KeyValuePair<string, TypedConstant> namedArgument in attributeData.NamedArguments)
            {
                if (namedArgument.Key == SourceGenerationHelper.AttributeProperty_InterfaceName
                    && namedArgument.Value.Value?.ToString() is { } infName)
                {
                    interfaceName = infName;
                }
            }
        }

        return TryExtractSymbols(symbol, interfaceName);
    }

    private static SimpleInterfaceToGenerate? TryExtractSymbols(INamedTypeSymbol symbol, string? customInterfaceName)
    {
        //System.Diagnostics.Debugger.Launch();

        var methodsBuilder = ImmutableArray.CreateBuilder<SimpleInterfaceToGenerate.Method>();
        var interfaceName = customInterfaceName ?? $"I{symbol.Name}";
        var nameSpace = symbol.ContainingNamespace.IsGlobalNamespace ? string.Empty : symbol.ContainingNamespace.ToString();

        var classMembers = symbol.GetMembers();

        foreach (var member in classMembers)
        {
            if (member is not IMethodSymbol methodSymbol)
            {
                //Only looking at methods right now
                continue;
            }
            else if (string.Equals(".ctor", methodSymbol.Name, StringComparison.Ordinal))
            {
                //Don't make a method for the constructor
                continue;
            }
            else if (methodSymbol.IsStatic)
            {
                continue;
            }

            string? methodName = methodSymbol.Name;
            string returnType;
            if (methodSymbol.ReturnsVoid)
            {
                returnType = "void";
            }
            else
            {
                returnType = methodSymbol.ReturnType.ToString();
            }

            var argumentBuilder = ImmutableArray.CreateBuilder<SimpleInterfaceToGenerate.Argument>();

            foreach (var methodParameter in methodSymbol.Parameters)
            {
                var argName = methodParameter.Name;
                var dataType = methodParameter.Type.Name;
                var nullableAnnotation = methodParameter.NullableAnnotation;

                var interfaceArgument = new SimpleInterfaceToGenerate.Argument(argName, dataType, nullableAnnotation);
            }

            var interfaceMethod = new SimpleInterfaceToGenerate.Method(methodName, returnType, argumentBuilder.ToImmutableArray());

            methodsBuilder.Add(interfaceMethod);
        }

        return new SimpleInterfaceToGenerate(
            InterfaceName: interfaceName,
            ClassName: symbol.Name,
            FullNamespace: nameSpace,
            Methods: methodsBuilder.ToImmutable());
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
