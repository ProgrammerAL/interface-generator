using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;

namespace ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator.GeneratorParsers;

public static class MethodParser
{
    public static SimpleInterfaceToGenerate.Method? ExtractMethod(IMethodSymbol symbol)
    {
        if (!IsSymbolValid(symbol))
        {
            return null;
        }

        string? methodName = symbol!.Name;
        string returnType;
        if (symbol.ReturnsVoid)
        {
            returnType = "void";
        }
        else
        {
            returnType = symbol.ReturnType.ToString();
        }

        var argumentBuilder = ImmutableArray.CreateBuilder<SimpleInterfaceToGenerate.Argument>();

        //_ = System.Diagnostics.Debugger.Launch();

        foreach (var methodParameter in symbol.Parameters)
        {
            var argName = methodParameter.Name;
            var dataType = methodParameter.Type.ToDisplayString();
            var nullableAnnotation = methodParameter.NullableAnnotation;

            var interfaceArgument = new SimpleInterfaceToGenerate.Argument(argName, dataType, nullableAnnotation);
            argumentBuilder.Add(interfaceArgument);
        }

        return new SimpleInterfaceToGenerate.Method(methodName, returnType, argumentBuilder.ToImmutableArray());
    }

    private static bool IsSymbolValid(IMethodSymbol symbol)
    {
        if (string.Equals(".ctor", symbol.Name, StringComparison.Ordinal))
        {
            //Don't make a method for the constructor
            return false;
        }
        else if (symbol.IsStatic)
        {
            //Only instance methods
            return false;
        }
        else if (symbol.DeclaredAccessibility != Accessibility.Public)
        {
            //Only public methods
            return false;
        }
        else if (symbol.Name.StartsWith("get_")
            || symbol.Name.StartsWith("set_"))
        {
            //These are the methods for properties
            //  Don't include those here because properties are handled separately
            return false;
        }
        else if (symbol.Name.StartsWith("add_")
            || symbol.Name.StartsWith("remove_"))
        {
            //These are the methods for events
            //  Don't include those here because events are handled separately
            return false;
        }

        return true;
    }
}
