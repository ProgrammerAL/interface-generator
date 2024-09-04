using System;
using System.Collections.Generic;
using System.Text;

namespace ProgrammerAl.SourceGenerators.InterfaceGenerator;

public static class SourceGenerationHelper
{
    public const string GenerateInterfaceAttributeName = "GenerateInterfaceAttribute";
    public const string GenerateInterfaceAttributeNameSpace = "ProgrammerAl.SourceGenerators.InterfaceGenerator.Attributes";
    public const string GenerateInterfaceAttributeFullName = $"{GenerateInterfaceAttributeNameSpace}.{GenerateInterfaceAttributeName}";

    public const string ExcludeFromGeneratedInterfaceAttributeName = "ExcludeFromGeneratedInterfaceAttribute";


    public const string AttributeProperty_InterfaceName = "InterfaceName";
    public const string AttributeProperty_NamespaceName = "Namespace";

    public const string AttributeClassCode =
@$"
namespace {GenerateInterfaceAttributeNameSpace}
{{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class {GenerateInterfaceAttributeName} : System.Attribute
    {{
        /// <summary>
        /// Set this to override the default interface name. Or leave it null to use the class name with an 'I' prepended to it.
        /// </summary>
        public string? {AttributeProperty_InterfaceName} {{ get; set; }}

        /// <summary>
        /// Set this to override the namespace to generate the interface in. By default, it will be the same as the class.
        /// </summary>
        public string? {AttributeProperty_NamespaceName} {{ get; set; }}
    }}

    [System.AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
    public class {ExcludeFromGeneratedInterfaceAttributeName} : System.Attribute
    {{
    }}
}}
";

    public static string GenerateInterface(in InterfaceToGenerateInfo interfaceInfo)
    {
        var builder = new StringBuilder();
        _ = builder.AppendLine($"namespace {interfaceInfo.FullNamespace};");
        _ = builder.AppendLine();
        _ = builder.AppendLine($"public interface {interfaceInfo.InterfaceName}");
        _ = builder.AppendLine("{");
        foreach (var outputEvent in interfaceInfo.Events)
        {
            _ = builder.AppendLine($"    {outputEvent.ToEventString()}");
        }

        foreach (var property in interfaceInfo.Properties)
        {
            _ = builder.AppendLine($"    {property.ToPropertyString()}");
        }

        foreach (var method in interfaceInfo.Methods)
        {
            _ = builder.AppendLine($"    {method.ToMethodString()}");
        }

        _ = builder.AppendLine("}");

        return builder.ToString();
    }
}
