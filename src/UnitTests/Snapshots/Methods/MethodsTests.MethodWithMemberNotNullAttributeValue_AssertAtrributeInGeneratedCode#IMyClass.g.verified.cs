//HintName: IMyClass.g.cs
#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

public interface IMyClass
{
    bool TrySomething(int num, [System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(true)][ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses.SomethingElseAttribute] out string? text);
}
