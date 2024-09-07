//HintName: IMyClass.g.cs
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

public interface IMyClass
{
    string? FirstName { get; }
    string? MiddleName { get; }
    string LastName { get; set; }
}
