//HintName: IMyClass.g.cs
#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

public interface IMyClass
{
    string FirstName { get; set; }
    string? MiddleName { get; set; }
    string LastName { get; set; }
    string ToString();
    int GetHashCode();
    bool Equals(object? obj);
    bool Equals(IMyClass? other);
    ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses.MyClass <Clone>$();
    void Deconstruct(out string FirstName, out string? MiddleName, out string LastName);
}
