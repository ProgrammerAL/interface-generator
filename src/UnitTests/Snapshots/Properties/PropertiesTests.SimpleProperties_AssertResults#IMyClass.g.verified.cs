﻿//HintName: IMyClass.g.cs
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

public interface IMyClass
{
    string? FirstName { get; set; }
    string? MiddleName { get; set; }
    string LastName { get; set; }
    string Tag1 { get; }
    string Tag2 { set; }
    int Arg1 { get; set; }
    double Arg2 { get; set; }
    float Arg3 { get; set; }
    decimal Arg4 { get; set; }
    char Arg5 { get; set; }
}
