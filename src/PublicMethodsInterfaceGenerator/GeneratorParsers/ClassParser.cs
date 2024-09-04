﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;

namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.GeneratorParsers;

public static class ClassParser
{
    public static InterfaceToGenerateInfo? GetTypeToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        var symbol = context.TargetSymbol as INamedTypeSymbol;
        if (symbol is null)
        {
            // nothing to do if this type isn't available
            return null;
        }

        ct.ThrowIfCancellationRequested();

        string? interfaceName = null;
        string? namespaceName = null;

        foreach (AttributeData attributeData in symbol.GetAttributes())
        {
            var attributeClassName = attributeData.AttributeClass?.Name;
            if (string.IsNullOrWhiteSpace(attributeClassName)
                || attributeClassName != SourceGenerationHelper.GenerateInterfaceAttributeName
                || attributeData.AttributeClass!.ToDisplayString() != SourceGenerationHelper.GenerateInterfaceAttributeFullName)
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
                else if (namedArgument.Key == SourceGenerationHelper.AttributeProperty_NamespaceName
                    && namedArgument.Value.Value?.ToString() is { } nsName)
                {
                    namespaceName = nsName;
                }
            }
        }

        return TryExtractSymbols(symbol, interfaceName, namespaceName);
    }

    private static InterfaceToGenerateInfo? TryExtractSymbols(INamedTypeSymbol symbol, string? customInterfaceName, string? namespaceName)
    {
        var interfaceName = customInterfaceName ?? $"I{symbol.Name}";
        var nameSpace = namespaceName ?? (symbol.ContainingNamespace.IsGlobalNamespace ? string.Empty : symbol.ContainingNamespace.ToString());

        var methodsBuilder = ImmutableArray.CreateBuilder<InterfaceToGenerateInfo.Method>();
        var propertiesBuilder = ImmutableArray.CreateBuilder<InterfaceToGenerateInfo.Property>();
        var eventsBuilder = ImmutableArray.CreateBuilder<InterfaceToGenerateInfo.Event>();

        var members = symbol.GetMembers();
        foreach (var member in members)
        {
            if (member is IMethodSymbol memberSymbol)
            {
                var method = MethodParser.ExtractMethod(memberSymbol);
                if (method is object)
                {
                    methodsBuilder.Add(method);
                }
            }
            else if (member is IPropertySymbol propertySymbol)
            {
                var property = PropertyParser.ExtractProperty(propertySymbol);
                if (property is object)
                {
                    propertiesBuilder.Add(property);
                }
            }
            else if (member is IEventSymbol eventSymbol)
            {
                var parsedEvent = EventParser.ExtractEvent(eventSymbol);
                if (parsedEvent is object)
                {
                    eventsBuilder.Add(parsedEvent);
                }
            }
        }

        return new InterfaceToGenerateInfo(
            InterfaceName: interfaceName,
            ClassName: symbol.Name,
            FullNamespace: nameSpace,
            Methods: methodsBuilder.ToImmutableArray(),
            Properties: propertiesBuilder.ToImmutableArray(),
            Events: eventsBuilder.ToImmutableArray());
    }
}
