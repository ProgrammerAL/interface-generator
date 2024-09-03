#pragma warning disable IDE0058 // Expression value is never used

namespace UnitTests;

public class MethodsTests
{
    private const string SnapshotsDirectory = "Snapshots/Methods";

    [Fact]
    public async Task SimpleMethodArguments_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.InterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.UnitTestClasses;

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
            using ProgrammerAl.SourceGenerators.InterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.UnitTestClasses;

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
            using ProgrammerAl.SourceGenerators.InterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.UnitTestClasses;

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
            using ProgrammerAl.SourceGenerators.InterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.UnitTestClasses;

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
            using ProgrammerAl.SourceGenerators.InterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.UnitTestClasses;

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

    //[Fact]
    //public async Task NoMethods_AssertResults()
    //{
    //    var source = """
    //        using ProgrammerAl.SourceGenerators.InterfaceGenerator.Attributes;
    //        namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.SampleClasses;

    //        [GenerateInterfaceAttribute]
    //        public class MyClass : IMyClass
    //        {
    //        }
    //        """;

    //    await TestHelper.VerifyAsync(source, "Snapshots/Methods");
    //}
}
#pragma warning restore IDE0058 // Expression value is never used
