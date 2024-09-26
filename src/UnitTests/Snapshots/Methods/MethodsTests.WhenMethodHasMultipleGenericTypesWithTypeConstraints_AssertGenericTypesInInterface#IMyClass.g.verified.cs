//HintName: IMyClass.g.cs
#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

public interface IMyClass
{
    void GenerateString1<T, U, V>(T arg1, U? arg2, V arg3) where T : class where U : struct;
    T GenerateString2<T, U, V>(T arg1, U? arg2, V arg3) where T : class;
    U? GenerateString3<T, U, V>(T arg1, U? arg2, V arg3) where U : struct;
    U? GenerateString4<T, U, V>(T arg1, U? arg2, V arg3) where U : new();
    U? GenerateString5<T, U, V>(T arg1, U? arg2, V arg3) where U : unmanaged;
    U? GenerateString6<T, U, V>(T arg1, U? arg2, V arg3) where U : class?;
    U? GenerateString7<T, U, V>(T arg1, U? arg2, V arg3) where U : ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses.MyBaseClass;
    U? GenerateString8<T, U, V>(T arg1, U? arg2, V arg3) where U : ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses.MyBaseClass?;
}
