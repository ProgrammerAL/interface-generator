using System;
using System.Collections.Generic;
using System.Text;

namespace ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator;

public static class SourceGenerationHelper
{
    public const string GenerateSimpleInterfaceAttributeName = "GenerateSimpleInterfaceAttribute";
    public const string GenerateSimpleInterfaceAttributeFullName = $"ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator.Extensions.{GenerateSimpleInterfaceAttributeName}";

    public const string AttributeProperty_InterfaceName = "InterfaceName";
    public const string AttributeProperty_NamespaceName = "Namespace";

    public const string AttributeClassCode =
@$"
namespace ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator.Extensions
{{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class {GenerateSimpleInterfaceAttributeName} : System.Attribute
    {{
        public string? {AttributeProperty_InterfaceName} {{ get; set; }}
        public string? {AttributeProperty_NamespaceName} {{ get; set; }}
    }}
}}
";

    public static string GenerateInterface(in SimpleInterfaceToGenerate interfaceToGenerate)
    {
        var builder = new StringBuilder();
        _ = builder.AppendLine($"namespace {interfaceToGenerate.FullNamespace};");
        _ = builder.AppendLine();
        _ = builder.AppendLine($"public interface {interfaceToGenerate.InterfaceName}");
        _ = builder.AppendLine("{");
        foreach (var property in interfaceToGenerate.Properties)
        {
            _ = builder.AppendLine($"    {property.ToPropertyString()}");
        }

        foreach (var method in interfaceToGenerate.Methods)
        {
            _ = builder.AppendLine($"    {method.ToMethodString()}");
        }

        //TODO: Include other items too
        _ = builder.AppendLine("}");

        return builder.ToString();
    }
}
