using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;

using static ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator.SimpleInterfaceToGenerate;

namespace ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator;

//TODO: Include other items too
//  Properties
//  Events
public record SimpleInterfaceToGenerate(
    string InterfaceName, 
    string ClassName, 
    string FullNamespace, 
    ImmutableArray<Method> Methods,
    ImmutableArray<Property> Properties
    )
{
    public record Method(string MethodName, string ReturnType, ImmutableArray<Argument> Arguments)
    { 
        public string ToMethodString() => $"{ReturnType} {MethodName}({string.Join(", ", Arguments.Select(a => a.ToArgumentString()))});";
    }

    public record Argument(string ArgumentName, string DataType, NullableAnnotation NullableAnnotation)
    {
        public string ToArgumentString()
        {
            return $"{DataType} {ArgumentName}";
        }
    }

    public record Property(string MethodName, string ReturnType, bool HasValidGet, bool HasValidSet)
    { 
        public string ToPropertyString() 
            => $"{ReturnType} {MethodName} {{ {(HasValidGet ? "get; " : "")}{(HasValidSet ? "set; " : "")}}}";
    }
}
