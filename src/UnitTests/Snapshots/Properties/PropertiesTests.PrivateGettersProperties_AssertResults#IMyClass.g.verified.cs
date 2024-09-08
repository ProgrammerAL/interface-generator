//HintName: IMyClass.g.cs
#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

public interface IMyClass
{
    string? FirstName { set; }
    string? MiddleName { set; }
    string LastName { get; set; }
}
