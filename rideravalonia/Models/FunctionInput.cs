// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace rideravalonia.Models;

public readonly struct FunctionInput(string input, DateTime inputTime)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Input { get; } = input;
    public DateTime InputTime { get; } = inputTime;
}
