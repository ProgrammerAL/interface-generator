using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;

namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.GeneratorParsers;

public static class EventParser
{
    public static InterfaceToGenerateInfo.Event? ExtractEvent(IEventSymbol symbol)
    {
        if (!IsSymbolValid(symbol))
        {
            return null;
        }

        string? name = symbol!.Name;
        string dataType = symbol.Type.ToDisplayString();

        return new InterfaceToGenerateInfo.Event(name, dataType);
    }

    private static bool IsSymbolValid(IEventSymbol symbol)
    {
        if (symbol.IsStatic)
        {
            //Only instance
            return false;
        }
        else if (symbol.DeclaredAccessibility != Accessibility.Public)
        {
            //Only public
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
