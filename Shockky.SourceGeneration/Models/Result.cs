// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// This file is ported and adapted from CommunityToolkit.Mvvm (CommunityToolkit/dotnet),
// more info in ThirdPartyNotices.txt in the root of the project.

using Shockky.SourceGeneration.Helpers;

namespace Shockky.SourceGeneration.Models;

/// <summary>
/// A model representing a value and an associated set of diagnostic errors.
/// </summary>
/// <typeparam name="TValue">The type of the wrapped value.</typeparam>
/// <param name="Value">The wrapped value for the current result.</param>
/// <param name="Errors">The associated diagnostic errors, if any.</param>
internal sealed record Result<TValue>(TValue Value, EquatableArray<DiagnosticInfo> Errors)
    where TValue : IEquatable<TValue>?;
