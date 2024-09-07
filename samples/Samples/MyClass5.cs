using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;

using Samples.NonDemoClasses;

namespace Samples;

public delegate void MyEventDelegate(object sender, EventArgs e);

[GenerateInterface]
public class MyClass5 : IMyClass5
{
    public event EventHandler? MyEvent;
    public event Action<int>? MyEvent2;
    public event Action<int, string, double, float, NonDemoClass1>? MyEvent3;
    public event MyEventDelegate MyEvent4;

    public void HandEvents()
    {
        MyEvent?.Invoke(this, EventArgs.Empty);
        MyEvent2?.Invoke(1);
        MyEvent3?.Invoke(1, "2", 3.0, 4.0f, new NonDemoClass1());
        MyEvent4?.Invoke(this, EventArgs.Empty);
    }
}
