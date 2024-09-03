//HintName: IMyClass.g.cs
namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.SampleClasses;

public interface IMyClass
{
    event Action<int>? MyEvent1;
    event Action<int, string, double, float, NonDemoClass1>? MyEvent2;
    void HandEvents();
}
