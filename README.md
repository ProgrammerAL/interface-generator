# interface-generator

The purpose of this project is to make a C# Source Generator to create Interfaces at compile time. The Source Generator will inspect all classes with the provided `[GenerateInterfaceAttribute]` attribute and generate an Interface of all public Methods, Properties, and Events.

The default naming convention is to generate an interface of the same name as the class with 'I' prepended to it. For example, `MyClass` would generate an interface called `IMyClass`.

The `GenerateInterfaceAttribute` attribute has 2 properties. 
  - `InterfaceName` - This will override the default name for the interface that is generated. The name will be the string provided.
  - `Namespace` - This will override the default namespace the interface is generated in. The default is to use the same namespace the class is in.

## Why does this exist?

A common pattern in C# applications is to create a class and an underlying interface with a similar name. Usually this is done for for unit testing purposes. The interface can be mocked in a unit test to simplify test setup. The interface is 

So the purpose of this project is to save a bit of time on some manual work.


```csharp
public interface IUserManager
{
    Task UpdateUserName(string name);
    Task UpdateUserAddress(string address);
}

public class UserManager : IUserManager
{
    public async Task UpdateUserName(string name)
    {
        await Task.Completed;
    }

    public async Task UpdateUserAddress(string address)
    {
        await Task.Completed;
    }
}

//Register the interface to the .NET IoC Container
builder.Services.AddScoped<IUserManager, UserManager>();
```

