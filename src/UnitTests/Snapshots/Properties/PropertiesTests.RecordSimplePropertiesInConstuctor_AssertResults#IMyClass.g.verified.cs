﻿//HintName: IMyClass.g.cs
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
    bool Equals(ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses.MyClass? other);
    ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses.MyClass <Clone>$();
    void Deconstruct(string FirstName, string? MiddleName, string LastName);
}
