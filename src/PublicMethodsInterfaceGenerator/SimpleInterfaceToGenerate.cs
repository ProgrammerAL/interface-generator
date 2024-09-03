using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;

using static ProgrammerAl.SourceGenerators.InterfaceGenerator.SimpleInterfaceToGenerate;

namespace ProgrammerAl.SourceGenerators.InterfaceGenerator;
public record SimpleInterfaceToGenerate(
    string InterfaceName, 
    string ClassName, 
    string FullNamespace, 
    ImmutableArray<Method> Methods,
    ImmutableArray<Property> Properties,
    ImmutableArray<Event> Events
    )
{
    public record Method(string Name, string ReturnType, ImmutableArray<Argument> Arguments)
    { 
        public string ToMethodString() => $"{ReturnType} {Name}({string.Join(", ", Arguments.Select(a => a.ToArgumentString()))});";
    }

    public record Argument(string Name, string DataType, NullableAnnotation NullableAnnotation)
    {
        public string ToArgumentString()
        {
            return $"{DataType} {Name}";
        }
    }

    public record Property(string Name, string ReturnType, bool HasValidGet, bool HasValidSet)
    { 
        public string ToPropertyString() 
            => $"{ReturnType} {Name} {{ {(HasValidGet ? "get; " : "")}{(HasValidSet ? "set; " : "")}}}";
    }

    public record Event(string Name, string EventDataType)
    {
        public string ToEventString()
            => $"event {EventDataType} {Name};";
    }
}
