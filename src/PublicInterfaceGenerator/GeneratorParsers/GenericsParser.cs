using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;

namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.GeneratorParsers;

public static class GenericsParser
{
    public static ImmutableArray<InterfaceToGenerateInfo.GenericParameter> ParseGenericParameters(ImmutableArray<ITypeParameterSymbol> typeParameters)
    {
        var builder = ImmutableArray.CreateBuilder<InterfaceToGenerateInfo.GenericParameter>();
        foreach (var typeParameter in typeParameters)
        {
            var genericConstraintsBuilder = new List<string>();
            genericConstraintsBuilder.Clear();

            if (typeParameter.HasReferenceTypeConstraint)
            {
                if (typeParameter.ReferenceTypeConstraintNullableAnnotation == NullableAnnotation.Annotated)
                {
                    genericConstraintsBuilder.Add("class?");
                }
                else
                {
                    genericConstraintsBuilder.Add("class");
                }
            }

            if (typeParameter.HasUnmanagedTypeConstraint)
            {
                genericConstraintsBuilder.Add("unmanaged");
            }
            else if (typeParameter.HasValueTypeConstraint)
            {
                genericConstraintsBuilder.Add("struct");
            }

            if (typeParameter.HasNotNullConstraint)
            {
                genericConstraintsBuilder.Add("notnull");
            }

            if (typeParameter.HasConstructorConstraint)
            {
                genericConstraintsBuilder.Add("new()");
            }

            foreach (var constraintType in typeParameter.ConstraintTypes)
            {
                var constraintTypeName = constraintType.ToString();
                if (constraintType is ITypeParameterSymbol constraintTypeParameter)
                {
                    constraintTypeName = constraintTypeParameter.Name;
                }

                genericConstraintsBuilder.Add(constraintTypeName);
            }

            var constraintTypes = string.Join(", ", genericConstraintsBuilder);

            var genericParameter = new InterfaceToGenerateInfo.GenericParameter(typeParameter.Ordinal, typeParameter.Name, typeParameter.NullableAnnotation, constraintTypes);
            builder.Add(genericParameter);
        }

        return builder.ToImmutableArray();
    }

}
