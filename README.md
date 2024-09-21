# interface-generator

The purpose of this project is to make a C# Source Generator to create Interfaces at compile time. The Source Generator will inspect all classes with the provided `[GenerateInterfaceAttribute]` attribute and generate an Interface of all public Methods, Properties, and Events.

The default naming convention is to generate an interface of the same name as the class, with 'I' prepended to it. For example, `MyClass` would generate an interface called `IMyClass`.

The `GenerateInterfaceAttribute` attribute has 2 properties. 
  - `InterfaceName` - This will override the default name for the interface that is generated. The name will be the string provided.
  - `Namespace` - By default, the same namespace as the class would be used for the generated interface. Setting thie property will override the default namespace and use the string provided.

# NuGet Package

The NuGet package is hosted on nuget.org. You can get it from: https://www.nuget.org/packages/PublicInterfaceGenerator

## Why does this exist?

Why spend my time on this and not something else? I'm glad you asked. 

I don't know.

# Seriously

Codifying code patterns can help keep code clean. You know it will always be a certain way, so why make a human write it out artisinally? That can be error prone, and also a waste of time. Make the machine do it.

A common pattern in C# applications is to create a class and an underlying interface with a similar name. A lot of the time this is done for unit testing purposes. The interface can be mocked in a unit test to simplify test setup. The interface always follows a pattern; look like the concrete implimentation, but an interface.

This means that this project will codify that pattern. A standard thing done in a project can have some code generated, so just have a tool generate it. Yes it saves a bit of time on manual work. But the real value is in knowing the pattern is always followed because the code is generated.

# Sample Code

Imagine you have code like below. An interface, and concrete class, and a section of code where you register the interface/class with an IoC container.

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

When generating the interface, your code would look like this:

```csharp
[GenerateInterface]
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

Does that look cleaner? I'll leave that up to you. But that's what this project does.

