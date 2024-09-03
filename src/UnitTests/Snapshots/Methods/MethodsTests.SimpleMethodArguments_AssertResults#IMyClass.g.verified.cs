//HintName: IMyClass.g.cs
namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.UnitTestClasses;

public interface IMyClass
{
    void GenerateString1(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6);
    string GenerateString2(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6);
    Task<string> GenerateString3(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6);
}
