// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// This file is ported and adapted from CommunityToolkit.Mvvm (CommunityToolkit/dotnet),
// more info in ThirdPartyNotices.txt in the root of the project.

using System.Globalization;

using Shockky.SourceGeneration.Extensions;
using Shockky.SourceGeneration.Helpers;

namespace Shockky.SourceGeneration.Models;

/// <summary>
/// A model representing a typed constant item.
/// </summary>
/// <remarks>This model is fully serializable and comparable.</remarks>
internal abstract partial record TypedConstantInfo
{
    /// <summary>
    /// Writes the <see cref="TypedConstantInfo"/> instance syntax.
    /// </summary>
    public abstract void WriteSyntax(IndentedTextWriter writer);

    /// <summary>
    /// A <see cref="TypedConstantInfo"/> type representing an array.
    /// </summary>
    /// <param name="ElementTypeName">The type name for array elements.</param>
    /// <param name="Items">The sequence of contained elements.</param>
    public sealed record Array(string ElementTypeName, EquatableArray<TypedConstantInfo> Items) : TypedConstantInfo
    {
        public override void WriteSyntax(IndentedTextWriter writer)
        {
            writer.Write($"new {ElementTypeName}[] {{ ");
            writer.WriteInitializationExpressions(Items.AsSpan(),
                static (item, writer) => item.WriteSyntax(writer));
            writer.Write(" }");
        }
    }

    /// <summary>
    /// A <see cref="TypedConstantInfo"/> type representing a primitive value.
    /// </summary>
    public abstract record Primitive : TypedConstantInfo
    {
        /// <summary>
        /// A <see cref="TypedConstantInfo"/> type representing a <see cref="string"/> value.
        /// </summary>
        /// <param name="Value">The input <see cref="string"/> value.</param>
        public sealed record String(string Value) : TypedConstantInfo
        {
            public override void WriteSyntax(IndentedTextWriter writer)
            {
                writer.Write($"\"{Value}\"");
            }
        }

        /// <summary>
        /// A <see cref="TypedConstantInfo"/> type representing a <see cref="bool"/> value.
        /// </summary>
        /// <param name="Value">The input <see cref="bool"/> value.</param>
        public sealed record Boolean(bool Value) : TypedConstantInfo
        {
            public override void WriteSyntax(IndentedTextWriter writer)
                => writer.Write(Value ? "true" : "false");
        }

        /// <summary>
        /// A <see cref="TypedConstantInfo"/> type representing a generic primitive value.
        /// </summary>
        /// <typeparam name="T">The primitive type.</typeparam>
        /// <param name="Value">The input primitive value.</param>
        public sealed record Of<T>(T Value) : TypedConstantInfo
            where T : unmanaged, IEquatable<T>
        {
            public override void WriteSyntax(IndentedTextWriter writer)
            {
                writer.Write(Value switch
                {
                    byte b => b.ToString(),
                    char c => c.ToString(),

                    // For floating-point types, we need to manually format it and always add the trailing suffix.
                    // This ensures that the correct type is produced if the expression was assigned to
                    // an object (eg. the literal was used in an attribute object parameter/property).
                    double d => d.ToString("R", CultureInfo.InvariantCulture) + "D",
                    float f => f.ToString("R", CultureInfo.InvariantCulture) + "F",

                    int i => i.ToString(),
                    long l => l.ToString(),
                    sbyte sb => sb.ToString(),
                    short sh => sh.ToString(),
                    uint ui => ui.ToString(),
                    ulong ul => ul.ToString(),
                    ushort ush => ush.ToString(),

                    _ => throw new ArgumentException("Invalid primitive type")
                });
            }
        }
    }

    /// <summary>
    /// A <see cref="TypedConstantInfo"/> type representing a type.
    /// </summary>
    /// <param name="TypeName">The input type name.</param>
    public sealed record Type(string TypeName) : TypedConstantInfo
    {
        /// <inheritdoc/>
        public override void WriteSyntax(IndentedTextWriter writer)
            => writer.Write($"typeof({TypeName})");
    }

    /// <summary>
    /// A <see cref="TypedConstantInfo"/> type representing an enum value.
    /// </summary>
    /// <param name="TypeName">The enum type name.</param>
    /// <param name="Value">The boxed enum value.</param>
    public sealed record Enum(string TypeName, object Value) : TypedConstantInfo
    {
        public override void WriteSyntax(IndentedTextWriter writer)
            => writer.Write($"({TypeName}){Value}");
    }

    /// <summary>
    /// A <see cref="TypedConstantInfo"/> type representing a <see langword="null"/> value.
    /// </summary>
    public sealed record Null : TypedConstantInfo
    {
        public override void WriteSyntax(IndentedTextWriter writer)
            => writer.Write("null");
    }
}