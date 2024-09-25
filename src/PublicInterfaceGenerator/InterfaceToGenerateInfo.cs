using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;

using static ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.InterfaceToGenerateInfo;

namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator;
public record InterfaceToGenerateInfo(
    string InterfaceName,
    string ClassName,
    string FullNamespace,
    string Interfaces,
    bool InheritsFromIDisposable,
    ImmutableArray<Method> Methods,
    ImmutableArray<Property> Properties,
    ImmutableArray<Event> Events)
{
    internal static string CombineLineWithComments(string comments, string definitionLine)
    {
        if (string.IsNullOrWhiteSpace(comments))
        {
            return definitionLine;
        }
        else
        {
            return $"{comments}\n{definitionLine}";
        }
    }

    public record Method(string Name, string ReturnType, ImmutableArray<Argument> Arguments, string Comments)
    {
        public string ToMethodString()
        {
            var definitionLine = $"{ReturnType} {Name}({string.Join(", ", Arguments.Select(a => a.ToArgumentString()))});";
            return InterfaceToGenerateInfo.CombineLineWithComments(Comments, definitionLine);
        }
    }

    public record Argument(string Name, string DataType, NullableAnnotation NullableAnnotation)
    {
        public string ToArgumentString()
        {
            return $"{DataType} {Name}";
        }
    }

    public record Property(string Name, string ReturnType, bool HasValidGet, bool HasValidSet, string Comments)
    {
        public string ToPropertyString()
        {
            var propertyLine = $"{ReturnType} {Name} {{ {(HasValidGet ? "get; " : "")}{(HasValidSet ? "set; " : "")}}}";
            return InterfaceToGenerateInfo.CombineLineWithComments(Comments, propertyLine);
        }
    }

    public record Event(string Name, string EventDataType, string Comments)
    {
        public string ToEventString()
        {
            var eventLine = $"event {EventDataType} {Name};";
            return InterfaceToGenerateInfo.CombineLineWithComments(Comments, eventLine);
        }
    }
}
