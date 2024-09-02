using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;

namespace ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator.GeneratorParsers;

public static class MethodParser
{
    public static ImmutableArray<SimpleInterfaceToGenerate.Method> ExtractMethods(INamedTypeSymbol symbol)
    {
        var methodsBuilder = ImmutableArray.CreateBuilder<SimpleInterfaceToGenerate.Method>();
        var classMembers = symbol.GetMembers();

        foreach (var member in classMembers)
        {
            if (!TryGetValidMethodSymbol(member, out var methodSymbol))
            {
                continue;
            }

            string? methodName = methodSymbol!.Name;
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

            //_ = System.Diagnostics.Debugger.Launch();

            foreach (var methodParameter in methodSymbol.Parameters)
            {
                var argName = methodParameter.Name;
                var dataType = methodParameter.Type.ToDisplayString();
                var nullableAnnotation = methodParameter.NullableAnnotation;

                var interfaceArgument = new SimpleInterfaceToGenerate.Argument(argName, dataType, nullableAnnotation);
                argumentBuilder.Add(interfaceArgument);
            }

            var interfaceMethod = new SimpleInterfaceToGenerate.Method(methodName, returnType, argumentBuilder.ToImmutableArray());

            methodsBuilder.Add(interfaceMethod);
        }

        return methodsBuilder.ToImmutable();
    }

    private static bool TryGetValidMethodSymbol(ISymbol? symbol, out IMethodSymbol? outMethodSymbol)
    {
        outMethodSymbol = null;

        if (symbol is not IMethodSymbol methodSymbol)
        {
            //Only looking at methods right now
            return false;
        }
        else if (string.Equals(".ctor", methodSymbol.Name, StringComparison.Ordinal))
        {
            //Don't make a method for the constructor
            return false;
        }
        else if (methodSymbol.IsStatic)
        {
            //Only instance methods
            return false;
        }
        else if (methodSymbol.DeclaredAccessibility != Accessibility.Public)
        {
            //Only public methods
            return false;
        }

        outMethodSymbol = methodSymbol;
        return true;
    }
}
