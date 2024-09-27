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
    bool InheritsFromIAsyncDisposable,
    string Comments,
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

        var hasStartedInterfaceList = false;
        var hasInterfacesList = !string.IsNullOrWhiteSpace(Interfaces);

        if (hasInterfacesList)
        {
            AddInterfacesToLine(Interfaces, builder, hasStartedInterfaceList);
            hasStartedInterfaceList = true;

            if (InheritsFromIDisposable)
            {
                AddInterfacesToLine("System.IDisposable", builder, hasStartedInterfaceList);
            }

            if (InheritsFromIAsyncDisposable)
            {
                AddInterfacesToLine("System.IAsyncDisposable", builder, hasStartedInterfaceList);
            }
        }
        else
        {
            if (InheritsFromIDisposable)
            {
                AddInterfacesToLine("System.IDisposable", builder, hasStartedInterfaceList);
                hasStartedInterfaceList = true;
            }

            if (InheritsFromIAsyncDisposable)
            {
                AddInterfacesToLine("System.IAsyncDisposable", builder, hasStartedInterfaceList);
            }
        }

        foreach (var genericParam in GenericParameters)
        {
            if (!string.IsNullOrWhiteSpace(genericParam.ConstraintTypes))
            {
                _ = builder.Append($" where {genericParam.Name} : {genericParam.ConstraintTypes}");
            }
        }

        return InterfaceToGenerateInfo.CombineLineWithComments(Comments, builder);
    }

    private void AddInterfacesToLine(string interfaces, StringBuilder builder, bool hasStartedInterfaceList)
    {
        if (hasStartedInterfaceList)
        {
            _ = builder.Append($", {interfaces}");
        }
        else
        {
            _ = builder.Append($" : {interfaces}");
        }
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

    internal static string CombineLineWithComments(string comments, StringBuilder builder)
    {
        return CombineLineWithComments(comments, builder.ToString());
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

            return InterfaceToGenerateInfo.CombineLineWithComments(Comments, builder);
        }
    }

    public record MethodArgument(string Name, NullableAnnotation NullableAnnotation, ImmutableArray<string> AttributeStrings, string DefaultValue)
    {
        public string ToArgumentString()
        {
            string parameterString;
            if (AttributeStrings.Any())
            {
                var attributeStringBlocks = AttributeStrings.Select(x => $"[{x}]");

                var attributes = string.Join("", attributeStringBlocks);
                parameterString = $"{attributes} {Name}";
            }
            else
            { 
                parameterString = Name;
            }

            if (!string.IsNullOrWhiteSpace(DefaultValue))
            { 
                parameterString += $" = {DefaultValue}";
            }

            return parameterString;
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
