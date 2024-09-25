//HintName: IMyClass.g.cs
#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

public interface IMyClass
{
    /// <summary>
    /// This is my GenerateString1 method block comment
    /// </summary>
    /// <returns>It's void so it doesn't return anything</returns>
    void GenerateString1(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6);
    /// This is my single-line triple comment that will appear in the interface
    string GenerateString2(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6);
    string GenerateString3(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6);
    string GenerateString4(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6);
}
