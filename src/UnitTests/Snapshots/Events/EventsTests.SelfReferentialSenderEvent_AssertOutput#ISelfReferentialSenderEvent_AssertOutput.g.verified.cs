//HintName: ISelfReferentialSenderEvent_AssertOutput.g.cs
#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.SampleClasses;

public interface ISelfReferentialSenderEvent_AssertOutput
{
    event Action<ISelfReferentialSenderEvent_AssertOutput>? MyEvent;
    event Action<ISelfReferentialSenderEvent_AssertOutput, string>? MyEvent2;
    event Action<ISelfReferentialSenderEvent_AssertOutput, string?, int?>? MyEvent3;
    void HandEvents();
}
