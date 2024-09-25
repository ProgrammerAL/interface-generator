//HintName: GenerateInterfaceAttribute.g.cs

#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class GenerateInterfaceAttribute : System.Attribute
    {
        /// <summary>
        /// Set this to override the default interface name. Or leave it null to use the class name with an 'I' prepended to it.
        /// </summary>
        public string? InterfaceName { get; set; }

        /// <summary>
        /// Set this to override the namespace to generate the interface in. By default, it will be the same as the class.
        /// </summary>
        public string? Namespace { get; set; }

        /// <summary>
        /// Set this to specify the interfaces the generated interface will inherit from. For example, IDisposable. 
        /// This should be a syntax-valid list as you would type it out normally because it will be concatenated directly into the interface definition.
        /// For example: "MyNamespace.MyInterface1, MyNamespace.MyInterface2"
        /// </summary>
        public string? Interfaces { get; set; }

        /// <summary>
        /// Set this to specify the generates interface inherits from System.IDisposable.
        /// This will be appended to the list of interfaces.
        /// If you are also specifying interfaces with the "Interfaces" property, either set this to false and include "System.IDisposable" in the "Interfaces" property string, or set this to true and don't include "System.IDisposable" in the "Interfaces" property string.
        /// </summary>
        public bool IsIDisposable { get; set; } = false;
    }

    [System.AttributeUsage(System.AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
    public class ExcludeFromGeneratedInterfaceAttribute : System.Attribute
    {
    }
}

