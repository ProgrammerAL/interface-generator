using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.GeneratorParsers;

public static class MethodParser
{
    public static InterfaceToGenerateInfo.Method? ExtractMethod(
        IMethodSymbol symbol,
        string extraClassInterfaces,
        bool inheritsFromIDisposable)
    {
        if (!IsSymbolValid(symbol, extraClassInterfaces, inheritsFromIDisposable))
        {
            return null;
        }

        var methodComments = CommentsBlockParser.ParseCommentsBlock(symbol);

        string? methodName = symbol.Name;
        string returnType;
        if (symbol.ReturnsVoid)
        {
            returnType = "void";
        }
        else
        {
            returnType = symbol.ReturnType.ToString();
        }

        var argumentBuilder = ImmutableArray.CreateBuilder<InterfaceToGenerateInfo.Argument>();

        foreach (var methodParameter in symbol.Parameters)
        {
            var argName = methodParameter.Name;
            var dataType = methodParameter.Type.ToDisplayString();
            var nullableAnnotation = methodParameter.NullableAnnotation;

            var interfaceArgument = new InterfaceToGenerateInfo.Argument(argName, dataType, nullableAnnotation);
            argumentBuilder.Add(interfaceArgument);
        }

        return new InterfaceToGenerateInfo.Method(methodName, returnType, argumentBuilder.ToImmutableArray(), methodComments);
    }

    private static bool IsSymbolValid(IMethodSymbol symbol, string extraClassInterfaces, bool inheritsFromIDisposable)
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
        else if (symbol.GetAttributes().Any(x => x.AttributeClass?.Name is SourceGenerationHelper.ExcludeFromGeneratedInterfaceAttributeName))
        {
            //Don't include methods that have the [IgnoreInGeneratedInterface] attribute
            return false;
        }
        else if (IsMethodFromInheritedInterface(symbol))
        {
            //If the method exists because of an interface it's implementing
            //  don't include it in the interface we generate
            return false;
        }
        else if (IsDisposeMethodAndImplementsIDisposable(symbol, extraClassInterfaces, inheritsFromIDisposable))
        {
            //If the code uses an attribute to set that the class implements IDisposable
            //  and this is the Dispose() method, don't include it in the interface
            //  Note: This is different from the method check above, because the concrete class won't have IDisposable in the definition list, it's on the interface
            return false;
        }

        return true;
    }

    private static bool IsDisposeMethodAndImplementsIDisposable(IMethodSymbol symbol, string extraClassInterfaces, bool inheritsFromIDisposable)
    {
        if (!symbol.Name.Equals("Dispose"))
        {
            return false;
        }

        if (!symbol.ReturnsVoid)
        {
            return false;
        }

        if (inheritsFromIDisposable)
        {
            return true;
        }

        var interfaces = extraClassInterfaces.
                            Split([','], StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim());
        if (interfaces.Any(x => string.Equals(x, "System.IDisposable") || string.Equals(x, "IDisposable")))
        {
            return true;
        }

        return true;
    }

    private static bool IsMethodFromInheritedInterface(IMethodSymbol symbol)
    {
        var interfaces = symbol.ContainingType.AllInterfaces;

        foreach (var implementedInterface in interfaces)
        {
            var interfaceMethods = implementedInterface.GetMembers().OfType<IMethodSymbol>();
            foreach (var interfaceMethod in interfaceMethods)
            {
                var containingTypeImplementation = symbol.ContainingType.FindImplementationForInterfaceMember(interfaceMethod);
                if (symbol.Equals(containingTypeImplementation, SymbolEqualityComparer.Default))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
