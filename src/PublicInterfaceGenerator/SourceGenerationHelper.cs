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

    public const string AttributeClassCode =
@$"
#nullable enable
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

        /// <summary>
        /// Set this to specify the interfaces the generated interface will inherit from. For example, IDisposable. 
        /// This should be a syntax-valid list as you would type it out normally because it will be concatenated directly into the interface definition.
        /// For example: ""MyNamespace.MyInterface1, MyNamespace.MyInterface2""
        /// </summary>
        public string? {AttributeProperty_Interfaces} {{ get; set; }}

        /// <summary>
        /// Set this to specify the generates interface inherits from System.IDisposable.
        /// This will be appended to the list of interfaces.
        /// If you are also specifying interfaces with the ""{AttributeProperty_Interfaces}"" property, either set this to false and include ""System.IDisposable"" in the ""{AttributeProperty_Interfaces}"" property string, or set this to true and don't include ""System.IDisposable"" in the ""{AttributeProperty_Interfaces}"" property string.
        /// </summary>
        public bool {AttributeProperty_IsIDisposable} {{ get; set; }} = false;
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
        if (ShouldEnableNullableReferences(interfaceInfo))
        {
            _ = builder.AppendLine($"#nullable enable");
        }

        var interfaceDefinitionLine = $"public interface {interfaceInfo.InterfaceName}";
        if (!string.IsNullOrWhiteSpace(interfaceInfo.Interfaces))
        {
            interfaceDefinitionLine += $" : {interfaceInfo.Interfaces}";
            if (interfaceInfo.InheritsFromIDisposable)
            {
                interfaceDefinitionLine += ", System.IDisposable";
            }
        }
        else if (interfaceInfo.InheritsFromIDisposable)
        {
            interfaceDefinitionLine += " : System.IDisposable";
        }

        _ = builder.AppendLine($"namespace {interfaceInfo.FullNamespace};");
        _ = builder.AppendLine();
        _ = builder.AppendLine(interfaceDefinitionLine);
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

    private static bool ShouldEnableNullableReferences(in InterfaceToGenerateInfo interfaceInfo)
    {
        return interfaceInfo.Methods.Any(m => m.Arguments.Any(a => a.NullableAnnotation == NullableAnnotation.Annotated)
                                              || m.ReturnType.EndsWith("?"))
            || interfaceInfo.Properties.Any(x => x.ReturnType.EndsWith("?"))
            || interfaceInfo.Events.Any(x => x.EventDataType.EndsWith("?"));


    }
}
