using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;

namespace Samples;

[GenerateInterface]
public class MyClass4 : IMyClass4
{
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public string Tag1 { get; private set; }
    public string Tag2 { private get; set; }

    public string GenerateString() => GenerateString_Private();
    private string GenerateString_Private() => "";
}
