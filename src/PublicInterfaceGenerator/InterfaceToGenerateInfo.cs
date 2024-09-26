using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;

using static ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.InterfaceToGenerateInfo;

namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator;
public record InterfaceToGenerateInfo(
    string InterfaceName,
    string ClassName,
    string FullNamespace,
    string Interfaces,
    bool InheritsFromIDisposable,
    ImmutableArray<GenericParameter> GenericParameters,
    ImmutableArray<Method> Methods,
    ImmutableArray<Property> Properties,
    ImmutableArray<Event> Events)
{
    public string GenerateInterfaceDefinitionString()
    {
        var builder = new StringBuilder();

        _ = builder.Append($"public interface {InterfaceName}");

        if (GenericParameters.Any())
        {
            _ = builder.Append('<');

            for (int i = 0; i < GenericParameters.Length; i++)
            {
                var parameter = GenericParameters[i];
                _ = builder.Append(parameter.Name);
                if (parameter.NullableAnnotation == NullableAnnotation.Annotated)
                {
                    _ = builder.Append('?');
                }

                //If there are more generic parameters, add a comma to separate items in the list
                if (i + 1 < GenericParameters.Length)
                {
                    _ = builder.Append($", ");
                }
            }

            _ = builder.Append('>');
        }

        if (!string.IsNullOrWhiteSpace(Interfaces))
        {
            _ = builder.Append($" : {Interfaces}");
            if (InheritsFromIDisposable)
            {
                _ = builder.Append(", System.IDisposable");
            }
        }
        else if (InheritsFromIDisposable)
        {
            _ = builder.Append(" : System.IDisposable");
        }

        foreach (var genericParam in GenericParameters)
        {
            if (!string.IsNullOrWhiteSpace(genericParam.ConstraintTypes))
            {
                _ = builder.Append($" where {genericParam.Name} : {genericParam.ConstraintTypes}");
            }
        }
        return builder.ToString();
    }

    public record GenericParameter(int Ordinal, string Name, NullableAnnotation NullableAnnotation, string ConstraintTypes);

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

    public record Method(string Name, string ReturnType, ImmutableArray<MethodArgument> Arguments, ImmutableArray<GenericParameter> GenericParameters, string Comments)
    {
        public string ToMethodString()
        {
            var builder = new StringBuilder();

            _ = builder.Append($"{ReturnType} {Name}");

            if (GenericParameters.Any())
            {
                _ = builder.Append('<');

                for (int i = 0; i < GenericParameters.Length; i++)
                {
                    var parameter = GenericParameters[i];
                    _ = builder.Append(parameter.Name);
                    if (parameter.NullableAnnotation == NullableAnnotation.Annotated)
                    {
                        _ = builder.Append('?');
                    }

                    //If there are more generic parameters, add a comma to separate items in the list
                    if (i + 1 < GenericParameters.Length)
                    {
                        _ = builder.Append($", ");
                    }
                }

                _ = builder.Append('>');
            }

            _ = builder.Append($"({string.Join(", ", Arguments.Select(a => a.ToArgumentString()))})");

            foreach (var genericParam in GenericParameters)
            {
                if (!string.IsNullOrWhiteSpace(genericParam.ConstraintTypes))
                {
                    _ = builder.Append($" where {genericParam.Name} : {genericParam.ConstraintTypes}");
                }
            }

            _ = builder.Append(';');

            var definitionLine = builder.ToString();
            return InterfaceToGenerateInfo.CombineLineWithComments(Comments, definitionLine);
        }
    }

    public record MethodArgument(string Name, string DataType, NullableAnnotation NullableAnnotation)
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
