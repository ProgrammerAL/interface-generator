//HintName: IMyClass.g.cs
#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

public interface IMyClass
{
    void MyMethod(int arg1 = 1, int arg2 = 0, int? arg3 = null, int arg4 = 4, string valuedString = "abc123", string? nullString = null, string defaultString = null, string defaultNullableString = null);
}
