//HintName: IMyClass.g.cs
#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

public interface IMyClass
{
    /// <summary>
    /// This is my FirstName block comment
    /// </summary>
    string? FirstName { get; set; }
    /// This is my single line triple comment that will appear in the generated interface
    string? MiddleName { get; set; }
    string LastName { get; set; }
    string Address { get; set; }
}
