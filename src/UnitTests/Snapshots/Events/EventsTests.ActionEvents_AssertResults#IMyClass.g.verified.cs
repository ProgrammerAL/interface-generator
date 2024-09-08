//HintName: IMyClass.g.cs
#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.SampleClasses;

public interface IMyClass
{
    event Action<int>? MyEvent1;
    event Action<int, string, double, float, NonDemoClass1>? MyEvent2;
    void HandEvents();
}
