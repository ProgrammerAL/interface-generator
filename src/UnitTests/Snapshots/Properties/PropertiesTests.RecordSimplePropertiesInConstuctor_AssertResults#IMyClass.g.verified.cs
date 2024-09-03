//HintName: IMyClass.g.cs
namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.UnitTestClasses;

public interface IMyClass
{
    string FirstName { get; set; }
    string? MiddleName { get; set; }
    string LastName { get; set; }
    string ToString();
    int GetHashCode();
    bool Equals(object? obj);
    bool Equals(IMyClass? other);
    bool Equals(ProgrammerAl.SourceGenerators.InterfaceGenerator.UnitTestClasses.MyClass? other);
    ProgrammerAl.SourceGenerators.InterfaceGenerator.UnitTestClasses.MyClass <Clone>$();
    void Deconstruct(string FirstName, string? MiddleName, string LastName);
}
