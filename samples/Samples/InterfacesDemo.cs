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
        Console.WriteLine(nameof(IMyClass));
        Console.WriteLine(nameof(IMyClass.GenerateString));
    }
}
