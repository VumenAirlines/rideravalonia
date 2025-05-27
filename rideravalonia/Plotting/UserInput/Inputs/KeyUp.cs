// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Avalonia.Input;

namespace rideravalonia.Plotting.UserInput.Inputs;
//todo: make not avalonia
public record struct KeyUp(Key key) : IUserInput
{
    public readonly Key Key { get; } = key;
}
