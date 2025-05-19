// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using SkiaSharp;

namespace rideravalonia.Plotting.UserInput.Inputs;
/// <summary>
/// Represents a mouse cursor movement event
/// </summary>
/// <param name="Point">Position of the cursor as a <see cref="SKPoint"/></param>
public record struct MouseMove(SKPoint Point): IMouseInput
{
}
