//HintName: IMyClass.g.cs
#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

public interface IMyClass
{
    void GenerateString1<T, U, V>(T arg1, U? arg2, V arg3);
    T GenerateString2<T, U, V>(T arg1, U? arg2, V arg3);
    U? GenerateString3<T, U, V>(T arg1, U? arg2, V arg3);
}
