// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using SkiaSharp;

namespace rideravalonia.Plotting.UserInput.Inputs;

public record struct LeftMouseDown(SKPoint Point, int ClickCount): IMouseButtonInput
{
    public bool Pressed
    {
        get => true;
    }

    public MouseButton Button
    {
        get => MouseButton.Left;
    }
}
