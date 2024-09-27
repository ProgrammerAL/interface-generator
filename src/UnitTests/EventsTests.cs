#pragma warning disable IDE0058 // Expression value is never used

namespace UnitTests;

public class EventsTests
{
    private const string SnapshotsDirectory = "Snapshots/Events";

    [Fact]
    public async Task SimpleEvents_AssertResults()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.SampleClasses;

            [GenerateInterfaceAttribute]
            public class SimpleEvents_AssertResults : IMyClass
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
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.SampleClasses;

            [GenerateInterfaceAttribute]
            public class ActionEvents_AssertResults : IMyClass
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
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.SampleClasses;

            public delegate void MyEventDelegate(object sender, EventArgs e);
            
            [GenerateInterfaceAttribute]
            public class CustomEvents_AssertResults : IMyClass
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
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.SampleClasses;

            [GenerateInterfaceAttribute]
            public class WithExcludedEvents_AssertResults : IMyClass
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

    [Fact]
    public async Task EventWithComments_AssertCommentsAppearInGeneratedInterfaceEvents()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.SampleClasses;

            [GenerateInterfaceAttribute]
            public class EventWithComments_AssertCommentsAppearInGeneratedInterfaceEvents : IMyClass
            {
                /// <summary>
                /// Some comments for my event
                /// </summary>
                public event EventHandler? MyEvent;

                /// Single line tripple comment that will appear in generated interface
                public event EventHandler? MyEvent2;

                //Single line comment that won't appear in generated interface
                public event EventHandler? MyEvent3;
                        
                public void HandEvents()
                {
                    MyEvent?.Invoke(this, EventArgs.Empty);
                    MyEvent2?.Invoke(this, EventArgs.Empty);
                }
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }

    [Fact]
    public async Task ImplementsInterfaceEvent_AssertEventNotInGeneratedInterface()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.SampleClasses;

            public interface IMyInterface
            {
                event EventHandler? MyInterfaceEvent;
            }

            [GenerateInterfaceAttribute]
            public class ImplementsInterfaceEvent_AssertEventNotInGeneratedInterface : IMyClass, IMyInterface
            {
                //This event should be ignored because it's in the other interface
                public event EventHandler? MyInterfaceEvent;
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
    public async Task SelfReferentialSenderEvent_AssertOutput()
    {
        var source = """
            using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.SampleClasses;

            [GenerateInterfaceAttribute]
            public class SelfReferentialSenderEvent_AssertOutput : IMyClass
            {
                public event Action<ISelfReferentialSenderEvent_AssertOutput>? MyEvent;
                public event Action<ISelfReferentialSenderEvent_AssertOutput, string>? MyEvent2;
                public event Action<ISelfReferentialSenderEvent_AssertOutput, string?, int?>? MyEvent3;

                public void HandEvents()
                {
                    MyEvent?.Invoke(this, EventArgs.Empty);
                    MyEvent2?.Invoke(this, EventArgs.Empty);
                }
            }
            """;

        await TestHelper.VerifyAsync(source, SnapshotsDirectory);
    }
}
#pragma warning restore IDE0058 // Expression value is never used
