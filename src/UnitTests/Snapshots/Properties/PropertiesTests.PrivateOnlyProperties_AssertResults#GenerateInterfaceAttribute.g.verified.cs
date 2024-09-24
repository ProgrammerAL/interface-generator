//HintName: GenerateInterfaceAttribute.g.cs

#nullable enable
namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
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
    }

    [System.AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
    public class ExcludeFromGeneratedInterfaceAttribute : System.Attribute
    {
    }
}
