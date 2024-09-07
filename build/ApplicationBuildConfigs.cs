﻿using Cake.Common;
using Cake.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record ProjectPaths(
    string ProjectName,
    string PathToSln,
    string ProjectFolder,
    string CsprojFile,
    string UnitTestProj,
    string OutDir)
{
    public static ProjectPaths LoadFromContext(ICakeContext context, string buildConfiguration, string srcDirectory)
    {
        var projectName = "PublicInterfaceGenerator";
        var pathToSln = srcDirectory + $"/{projectName}.sln";
        var projectDir = srcDirectory + $"/{projectName}";
        var csProjFile = projectDir + $"/{projectName}.csproj";
        var unitTestsProj = srcDirectory + $"UnitTests/UnitTests.csproj";
        var outDir = projectDir + $"/bin/{buildConfiguration}/cake-build-output";

        return new ProjectPaths(
            projectName,
            pathToSln,
            projectDir,
            csProjFile,
            unitTestsProj,
            outDir);
    }
};
