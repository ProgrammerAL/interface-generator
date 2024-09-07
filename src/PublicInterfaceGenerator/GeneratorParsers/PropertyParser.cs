using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;

namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.GeneratorParsers;

public static class PropertyParser
{
    public static InterfaceToGenerateInfo.Property? ExtractProperty(IPropertySymbol symbol)
    {
        if (!IsSymbolValid(symbol))
        {
            return null;
        }

        string? propertyName = symbol!.Name;
        string dataType = symbol.Type.ToDisplayString();

        var hasValidGet = symbol.GetMethod?.DeclaredAccessibility is Accessibility.Public;
        var hasValidSet = symbol.SetMethod?.DeclaredAccessibility is Accessibility.Public;

        if (hasValidGet || hasValidSet)
        {
            return new InterfaceToGenerateInfo.Property(propertyName, dataType, hasValidGet, hasValidSet);
        }

        //If neither getter nor setter is public, then don't include this property
        return null;
    }

    private static bool IsSymbolValid(IPropertySymbol symbol)
    {
        if (symbol.IsStatic)
        {
            //Only instance properties
            return false;
        }
        else if (symbol.DeclaredAccessibility != Accessibility.Public)
        {
            //Only public properties
            return false;
        }
        else if (symbol.GetAttributes().Any(x => x.AttributeClass?.Name is SourceGenerationHelper.ExcludeFromGeneratedInterfaceAttributeName))
        {
            //Don't include methods that have the [IgnoreInGeneratedInterface] attribute
            return false;
        }

        return true;
    }
}
