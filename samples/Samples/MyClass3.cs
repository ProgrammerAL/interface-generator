using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;

using Samples.NonDemoClasses;

namespace Samples;

[GenerateInterface(Namespace = "MyGenerated.DemoNamespace")]
public class MyClass3 : MyGenerated.DemoNamespace.IMyClass3
{
    public string GenerateString() => GenerateString_Private();
    private string GenerateString_Private() => "";

    public string GenerateString2(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
        => $"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}";

    public string? GenerateString3(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
        => $"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}";

    public string GenerateString4(NonDemoClass1 demoClass1) => "";
}
