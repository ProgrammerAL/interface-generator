#pragma warning disable IDE0058 // Expression value is never used

namespace UnitTests;

public class EventsTests
{
    private const string SnapshotsDirectory = "Snapshots/Events";

    [Fact]
    public async Task SimpleEvents_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.InterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.SampleClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public event EventHandler? MyEvent;

                public void HandEvents()
                {
                    MyEvent?.Invoke(this, EventArgs.Empty);
                }
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task ActionEvents_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.InterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.SampleClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public event Action<int>? MyEvent1;
                public event Action<int, string, double, float, NonDemoClass1>? MyEvent2;

                public void HandEvents()
                {
                    MyEvent1?.Invoke(1);
                    MyEvent2?.Invoke(1, "2", 3.0, 4.0f, new NonDemoClass1());
                }
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task CustomEvents_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.InterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.SampleClasses;

            public delegate void MyEventDelegate(object sender, EventArgs e);
            
            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public event MyEventDelegate MyEvent;
            
                public void HandEvents()
                {
                    MyEvent?.Invoke(this, EventArgs.Empty);
                }
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }


    [Fact]
    public async Task WithExcludedEvents_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.InterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.SampleClasses;

            [GenerateInterfaceAttribute]
            public class MyClass : IMyClass
            {
                public event EventHandler? MyEvent;
                
                [ExcludeFromGeneratedInterface]
                public event EventHandler? MyEvent2;
                
                [ExcludeFromGeneratedInterfaceAttribute]
                public event EventHandler? MyEvent3;

                public void HandEvents()
                {
                    MyEvent?.Invoke(this, EventArgs.Empty);
                    MyEvent2?.Invoke(this, EventArgs.Empty);
                    MyEvent3?.Invoke(this, EventArgs.Empty);
                }
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }
}
#pragma warning restore IDE0058 // Expression value is never used
