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
    /// <summary>
    /// Indicates whether the button is pressed 
    /// </summary>
    public bool Pressed { get; }
    /// <summary>
    /// Indicates which button was involved in the event as a <see cref="MouseButton"/>
    /// </summary>
    public MouseButton Button { get; }
}
/// <summary>
/// Represents an input with the mouse
/// </summary>
public interface IMouseInput:IUserInput
{
    public SKPoint Point { get; }
}


