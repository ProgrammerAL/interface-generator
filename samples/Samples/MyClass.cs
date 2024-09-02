using ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator.Extensions;

namespace Samples;

[SimpleInterface]
public class MyClass : IMyClass
{
    public string GenerateString() => GenerateString_Private();
    private string GenerateString_Private() => "";
}
