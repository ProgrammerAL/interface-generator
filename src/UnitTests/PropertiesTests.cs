#pragma warning disable IDE0058 // Expression value is never used

namespace UnitTests;

public class PropertiesTests
{
    private const string SnapshotsDirectory = "Snapshots/Properties";

    [Fact]
    public async Task SimpleProperties_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public string? FirstName { get; set; }
                public string? MiddleName { get; set; }
                public required string LastName { get; set; }
                public string Tag1 { get; private set; }
                public string Tag2 { private get; set; }
   
                public int Arg1 { get; set; }
                public double Arg2 { get; set; }
                public float Arg3 { get; set; }
                public decimal Arg4 { get; set; }
                public char Arg5 { get; set; }
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task RecordSimpleProperties_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public record MyClass : IMyClass
            {
                public string? FirstName { get; set; }
                public string? MiddleName { get; set; }
                public required string LastName { get; set; }
                public string Tag1 { get; private set; }
                public string Tag2 { private get; set; }
   
                public int Arg1 { get; set; }
                public double Arg2 { get; set; }
                public float Arg3 { get; set; }
                public decimal Arg4 { get; set; }
                public char Arg5 { get; set; }
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task RecordSimplePropertiesInConstuctor_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public record MyClass(string FirstName, string? MiddleName, string LastName) : IMyClass;
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task PrivateGettersProperties_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public string? FirstName { private get; set; }
                public string? MiddleName { private get; set; }
                public string LastName { get; set; }
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task PrivateSettersProperties_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public string? FirstName { get; private set; }
                public string? MiddleName { get; private set; }
                public string LastName { get; set; }
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task PrivateOnlyProperties_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                private string? FirstName { get; set; }
                private string? MiddleName { get; set; }
                private string LastName { get; set; }
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task ComplexProperties_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public NonDemoClass1? ComplexProperty1 { get; set; }
            }

            public class NonDemoClass1
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task WithExcludedProperties_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public string? FirstName { get; set; }
                public string? MiddleName { get; set; }
                public required string LastName { get; set; }
   
                [ExcludeFromGeneratedInterface]
                public string Tag1 { get; private set; }
                
                [ExcludeFromGeneratedInterfaceAttribute]
                public string Tag2 { private get; set; }
             }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task PropertiesWithComments_AssertCommentsInGeneratedInterface()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                /// <summary>
                /// This is my FirstName block comment
                /// </summary>
                public string? FirstName { get; set; }

                /// This is my single line triple comment that will appear in the generated interface
                public string? MiddleName { get; set; }

                //This is my single line comment that will not appear in the generated interface
                public required string LastName { get; set; }

                public required string Address { get; set; }
             }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task PropertiesFromInheritedInterface_AssertNotInGeneratedInterface()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            public interface IMyInterface
            {
                string PublicGetAndSet { get; set; }
                string PublicGet1 { get; }
                string PublicGet2 { get; }
                string PublicSet1 { set; }
                string PublicSet2 { set; }
            }

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass, IMyInterface
            {
                public string PublicGetAndSet { get; set; }
                public string PublicGet1 { get; }
                public string PublicGet2 { get; set; }
                public string PublicSet1 { set; }
                public string PublicSet2 { get; set; }

                public string Tag1 { get; set; }
             }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }
}
#pragma warning restore IDE0058 // Expression value is never used
