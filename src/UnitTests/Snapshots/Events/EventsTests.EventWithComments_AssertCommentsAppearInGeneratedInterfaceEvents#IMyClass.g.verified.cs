//HintName: IMyClass.g.cs
#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.SampleClasses;

public interface IMyClass
{
    /// <summary>
    /// Some comments for my event
    /// </summary>
    event EventHandler? MyEvent;
    /// Single line tripple comment that will appear in generated interface
    event EventHandler? MyEvent2;
    event EventHandler? MyEvent3;
    void HandEvents();
}
