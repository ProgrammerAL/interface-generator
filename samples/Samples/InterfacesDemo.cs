using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples;

public class InterfacesDemo
{
    public void DisplayMyClass()
    {
        Console.WriteLine(nameof(IMyClass1));
        Console.WriteLine(nameof(IMyClass1.GenerateString));
    }
}
