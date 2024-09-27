using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;

namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator;

public static class SourceGenerationHelper
{
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
        foreach (var method in interfaceInfo.Methods)
        {
            //If the method returns a nullable value
            //  or a non-nullable value but it's a generic type and the generic value can be null
            //  Ex: Task<string?>
            if (method.ReturnType.Contains("?"))
            {
                return true;
            }

            foreach (var arg in method.Arguments)
            {
                if (arg.NullableAnnotation == NullableAnnotation.Annotated)
                {
                    return true;
                }
            }
        }

        if (interfaceInfo.Properties.Any(x => x.ReturnType.EndsWith("?")))
        {
            return true;
        }

        if (interfaceInfo.Events.Any(x => x.EventDataType.EndsWith("?")))
        {
            return true;
        }

        return false;
    }
}
