using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;

namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator;

public static class SourceGenerationHelper
{
    public const string GenerateInterfaceAttributeName = "GenerateInterfaceAttribute";
    public const string GenerateInterfaceAttributeNameSpace = "ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes";
    public const string GenerateInterfaceAttributeFullName = $"{GenerateInterfaceAttributeNameSpace}.{GenerateInterfaceAttributeName}";

    public const string ExcludeFromGeneratedInterfaceAttributeName = "ExcludeFromGeneratedInterfaceAttribute";

    public const string AttributeProperty_InterfaceName = "InterfaceName";
    public const string AttributeProperty_NamespaceName = "Namespace";
    public const string AttributeProperty_Interfaces = "Interfaces";
    public const string AttributeProperty_IsIDisposable = "IsIDisposable";

    public static string GenerateInterface(in InterfaceToGenerateInfo interfaceInfo)
    {
        var builder = new StringBuilder();
        if (ShouldEnableNullableReferences(interfaceInfo))
        {
            _ = builder.AppendLine($"#nullable enable");
        }

        var interfaceDefinitionLine = interfaceInfo.GenerateInterfaceDefinitionString();

        _ = builder.AppendLine($"namespace {interfaceInfo.FullNamespace};");
        _ = builder.AppendLine();
        _ = builder.AppendLine(interfaceDefinitionLine);
        _ = builder.AppendLine("{");
        foreach (var outputEvent in interfaceInfo.Events)
        {
            var codeString = outputEvent.ToEventString();
            AppendCodeLines(builder, codeString);
        }

        foreach (var property in interfaceInfo.Properties)
        {
            var codeString = property.ToPropertyString();
            AppendCodeLines(builder, codeString);
        }

        foreach (var method in interfaceInfo.Methods)
        {
            var codeString = method.ToMethodString();
            AppendCodeLines(builder, codeString);
        }

        _ = builder.AppendLine("}");

        return builder.ToString();
    }

    private static void AppendCodeLines(StringBuilder builder, string code)
    {
        var codeLines = code.Split('\n');
        foreach (var line in codeLines)
        {
            _ = builder.AppendLine($"    {line.Trim()}");
        }
    }

    private static bool ShouldEnableNullableReferences(in InterfaceToGenerateInfo interfaceInfo)
    {
        return interfaceInfo.Methods.Any(m => m.Arguments.Any(a => a.NullableAnnotation == NullableAnnotation.Annotated)
                                              || m.ReturnType.EndsWith("?"))
            || interfaceInfo.Properties.Any(x => x.ReturnType.EndsWith("?"))
            || interfaceInfo.Events.Any(x => x.EventDataType.EndsWith("?"));


    }
}
