﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator;
using ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;

namespace UnitTests;
public static class TestHelper
{
    public static async Task VerifyAsync(string source, string directory)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);
        // Create references for assemblies we require
        // We could add multiple references if required
        //IEnumerable<PortableExecutableReference> references = new[]
        //{
        //    MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        //};
        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
            .Select(_ => MetadataReference.CreateFromFile(_.Location))
            .Concat(new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(PublicInterfaceSourceGenerator).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(GenerateInterfaceAttribute).Assembly.Location),
            });

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: new[] { syntaxTree },
            references: references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        //GeneratorDriver driver = CSharpGeneratorDriver.Create(new PublicInterfaceSourceGenerator());
        //driver = driver.RunGenerators(compilation);
        var driver = CSharpGeneratorDriver
                .Create(new PublicInterfaceSourceGenerator())
                //.AddAdditionalTexts(LoadEmbeddedResourcesAsAdditionalText())
                //.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);
                .RunGenerators(compilation);

        _ = await Verifier
                  .Verify(driver)
                  .UseDirectory(directory);
    }
}
