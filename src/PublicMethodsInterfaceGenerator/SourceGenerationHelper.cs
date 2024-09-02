using System;
using System.Collections.Generic;
using System.Text;

namespace ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator;

public static class SourceGenerationHelper
{
    public const string AttributeProperty_InterfaceName = "InterfaceName";

    public const string AttributeClassCode =
@$"
namespace ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator.Extensions
{{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SimpleInterfaceAttribute : Attribute
    {{
        public string? {AttributeProperty_InterfaceName} {{ get; set; }}
    }}
}}
";

    public static string GenerateInterface(in SimpleInterfaceToGenerate interfaceToGenerate)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"namespace {interfaceToGenerate.FullNamespace};");
        builder.AppendLine();
        builder.AppendLine($"public interface {interfaceToGenerate.InterfaceName }");
        builder.AppendLine("{");
        foreach (var method in interfaceToGenerate.Methods)
        {
            builder.AppendLine($"    {method.ToMethodString()}");
        }
        //TODO: Include other items too
        builder.AppendLine("}");

        ////Output the partial class that inherits from the generated interface
        //builder.AppendLine($"public partial class {interfaceToGenerate.ClassName} : {interfaceToGenerate.InterfaceName}");
        //builder.AppendLine("{");
        //builder.AppendLine("}");

        return builder.ToString();
    }
}
