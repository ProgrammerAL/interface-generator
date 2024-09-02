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
    ImmutableArray<Method> Methods)
{
    public record Method(string MethodName, string ReturnType, ImmutableArray<Argument> Arguments)
    { 
        public string ToMethodString() => $"{ReturnType} {MethodName}({string.Join(", ", Arguments.Select(a => a.ToArgumentString()))});";
    }

    public record Argument(string ArgumentName, string DataType, NullableAnnotation NullableAnnotation)
    { 
        public string ToArgumentString() => $"{DataType} {ArgumentName}";
    }
}
