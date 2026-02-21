// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// This file is ported and adapted from ComputeSharp (Sergio0694/ComputeSharp),
// more info in ThirdPartyNotices.txt in the root of the project.

using Microsoft.CodeAnalysis;

namespace Shockky.SourceGeneration.Models;

/// <summary>
/// A model describing a type info in a type hierarchy.
/// </summary>
/// <param name="QualifiedName">The qualified name for the type.</param>
/// <param name="Kind">The type of the type in the hierarchy.</param>
/// <param name="IsRecord">Whether the type is a record type.</param>
internal sealed record TypeInfo(string QualifiedName, TypeKind Kind, bool IsRecord)
{
    /// <summary>
    /// The keyword for the current type kind.
    /// </summary>
    public string TypeKeyword => Kind switch
    {
        TypeKind.Struct when IsRecord => "record struct",
        TypeKind.Struct => "struct",
        TypeKind.Interface => "interface",
        TypeKind.Class when IsRecord => "record",
        _ => "class"
    };
}