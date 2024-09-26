#pragma warning disable IDE0058 // Expression value is never used

using System.Net.Http.Headers;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UnitTests;

public class MethodsTests
{
    private const string SnapshotsDirectory = "Snapshots/Methods";

    [Fact]
    public async Task SimpleMethodArguments_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public void GenerateString1(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => Console.WriteLine($"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}");
            
                public string GenerateString2(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => $"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}";

                public async Task<string> GenerateString3(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => Task.FromResult($"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}");
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task RecordSimpleMethodArguments_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public record MyClass : IMyClass
            {
                public void GenerateString1(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => Console.WriteLine($"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}");
            
                public string GenerateString2(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => $"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}";

                public async Task<string> GenerateString3(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => Task.FromResult($"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}");
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task EmptyMethodsArguments_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public string GenerateString() => GenerateString_Private();
                private string GenerateString_Private() => "";//Not Included in Interface
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task ComplexMethodArgument_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public string GenerateString(NonDemoClass1 demoClass1, int arg2) => "";
            }
            
            public class NonDemoClass1
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task WithExcludedMethods_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public string GenerateString() => "";

                [ExcludeFromGeneratedInterface]
                public string GenerateString2(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => $"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}";
            
                [ExcludeFromGeneratedInterfaceAttribute]
                public async Task<string> GenerateString3(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => Task.FromResult($"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}");
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task MethodHasComments_AssertCommentsInInterface()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                /// <summary>
                /// This is my GenerateString1 method block comment
                /// </summary>
                /// <returns>It's void so it doesn't return anything</returns>
                public void GenerateString1(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => Console.WriteLine($"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}");
            
                /// This is my single-line triple comment that will appear in the interface
                public string GenerateString2(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => $"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}";

                //This is my single-line comment that will not appear in the interface
                public string GenerateString3(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => $"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}";

                public string GenerateString4(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => $"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}";
                        }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task ImplementsIDisposable_AssertMethodNotInGeneratedInterface()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass, System.IDisposable
            {
                public void GenerateString1(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => Console.WriteLine($"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}");

                public void Dispose(){}            
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task IsIDisposiblePropertyTrue_AssertDisposeMethodNotInGeneratedInterface()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute(IsIDisposable = true)]
            public class MyClass : IMyClass
            {
                public void GenerateString1(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => Console.WriteLine($"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}");

                public void Dispose(){}
                public string Dispose(){ return ""; }
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task ImplementsInterfaceMethod_AssertMethodNotInGeneratedInterface()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass, System.IDisposable
            {
                public void GenerateString1(int arg1, string arg2, string? arg3, float arg4, double arg5, int? arg6)
                    => Console.WriteLine($"{arg1} {arg2} {arg3} {arg4} {arg5} {arg6}");

                public void Dispose(){}            
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task WhenMethodHasGenericTypes_AssertGenericTypesInInterface()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public void GenerateString1<T>(T arg1, T? arg2)
                    => Console.WriteLine($"{arg1} {arg2}");

                public T GenerateString2<T>(T arg1, T? arg2)
                    => arg1;
            
                public T? GenerateString3<T>(T arg1, T? arg2)
                    => arg2;
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task WhenMethodHasMultipleGenericTypes_AssertGenericTypesInInterface()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public void GenerateString1<T, U, V>(T arg1, U? arg2, V arg3)
                    => Console.WriteLine($"{arg1} {arg2} {arg3}");

                public T GenerateString2<T, U, V>(T arg1, U? arg2, V arg3)
                    => arg1;
            
                public U? GenerateString3<T, U, V>(T arg1, U? arg2, V arg3)
                    => arg2;
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    public class MyClass
    {
        public string MyMehod<T, U>(T ar, U asd)
            where T : class
            where U : struct
        {
            return "";
        }
    }

    [Fact]
    public async Task WhenMethodHasMultipleGenericTypesWithTypeConstraints_AssertGenericTypesInInterface()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public void GenerateString1<T, U, V>(T arg1, U? arg2, V arg3)
                    where T : class
                    where U : struct
                    => Console.WriteLine($"{arg1} {arg2} {arg3}");

                public T GenerateString2<T, U, V>(T arg1, U? arg2, V arg3)
                    where T : class
                    => arg1;
            
                public U? GenerateString3<T, U, V>(T arg1, U? arg2, V arg3)
                    where U : struct
                    => arg2;
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }
}
#pragma warning restore IDE0058 // Expression value is never used
