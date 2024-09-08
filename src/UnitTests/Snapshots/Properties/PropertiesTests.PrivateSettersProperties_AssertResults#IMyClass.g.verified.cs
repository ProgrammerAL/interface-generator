//HintName: IMyClass.g.cs
#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

public interface IMyClass
{
    string? FirstName { get; }
    string? MiddleName { get; }
    string LastName { get; set; }
}
