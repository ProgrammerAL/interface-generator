//HintName: IMyClass.g.cs
#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

public interface IMyClass
{
    void GenerateString1<T>(T arg1, T? arg2);
    T GenerateString2<T>(T arg1, T? arg2);
    T? GenerateString3<T>(T arg1, T? arg2);
}
