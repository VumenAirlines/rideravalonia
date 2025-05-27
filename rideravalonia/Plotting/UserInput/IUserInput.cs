// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using SkiaSharp;

namespace rideravalonia.Plotting.UserInput;
/// <summary>
/// Represents a user input
/// </summary>
public interface IUserInput
{
    
}
/// <summary>
/// Represents an input with a mouse button
/// </summary>
public interface IMouseButtonInput:IMouseInput
{
    
    public bool Pressed { get; }
    public int ClickCount { get; }
    public MouseButton Button { get; }
}

public interface IMouseInput:IUserInput
{
    public SKPoint Point { get; }
}


