#pragma warning disable IDE0058 // Expression value is never used

namespace UnitTests;

public class InterfaceDefinitionTests
{
    private const string SnapshotsDirectory = "Snapshots/InterfaceDefinitionTests";

    [Fact]
    public async Task DefaultName()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task DefaultGenerationWithComments_AssertCommentsInOutput()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            /// <summary>
            /// This is my class block comment
            /// </summary>
            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task DefaultName_SimpleAttributeName()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterface]
            public class MyClass : IMyClass
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task DifferentName()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute(InterfaceName = "SomeOtherName")]
            public class MyClass : SomeOtherName
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task CustomNamespace()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute(Namespace = "My.Custom.Namespace")]
            public class MyClass : IMyClass
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task CustomInterfaces()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute(Interfaces = "MyCode.Interface1, MyCode.Interface2, MyCode.Interface3")]
            public class MyClass : IMyClass
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task CustomInterfacesAndIDisposable()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute(IsIDisposable = true, Interfaces = "MyCode.Interface1, MyCode.Interface2, MyCode.Interface3")]
            public class MyClass : IMyClass
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task CustomInterfacesAndIAsyncDisposable()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute(IsIAsyncDisposable = true, Interfaces = "MyCode.Interface1, MyCode.Interface2, MyCode.Interface3")]
            public class MyClass : IMyClass
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task IsIDisposable()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute(IsIDisposable = true)]
            public class MyClass : IMyClass
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task IsIAsyncDisposable()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute(IsIAsyncDisposable = true)]
            public class MyClass : IMyClass
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task IsIDisposableAndIAsyncDisposable()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute(IsIDisposable = true, IsIAsyncDisposable = true)]
            public class MyClass : IMyClass
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task GenericClass()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass<T> : IMyClass<T>
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task GenericClassWithTypeConstraints_Class()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            [GenerateInterfaceAttribute]
            public class MyClass<T> : IMyClass<T> where T : class
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task GenericClassWithTypeConstraints_BaseClass()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            public class MyBase { }
            
            [GenerateInterfaceAttribute]
            public class MyClass<T> : IMyClass<T> where T : MyBase
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task GenericClassWithTypeConstraints_EmptyConstructor()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.UnitTestClasses;

            public class MyBase { }
            
            [GenerateInterfaceAttribute]
            public class MyClass<T> : IMyClass<T> where T : new()
            {
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }
}
#pragma warning restore IDE0058 // Expression value is never used
