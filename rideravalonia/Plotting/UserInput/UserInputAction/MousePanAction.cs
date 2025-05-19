// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using SkiaSharp;

using rideravalonia.Plotting.Components;

namespace rideravalonia.Plotting.UserInput.UserInputAction;
/// <summary>
/// Handles panning with the mouse
/// </summary>
public class MousePanAction:IUserInputAction
{
    private SKPoint? _start;
    public (bool isLocked, bool isRefresh) Execute(IUserInput input, PlotContainer container)
    {
        switch (input)
        {
            case IMouseButtonInput { Pressed: true, Button: MouseButton.Left } mouseDown:
                _start = mouseDown.Point;
                return (false, false);
            case IMouseButtonInput { Pressed: false, Button: MouseButton.Left }:
                _start = null;
                return (false, true);

            case IMouseInput drag when _start is not null:
                if (SKPoint.Distance(_start.Value, drag.Point) < 1)
                    return (false, false);

                container._plot?.CoordinateManager.MousePan(SKPoint.Subtract(drag.Point, _start.Value));
                container._plot?.CoordinateManager.UpdateTransform(container.Bounds);
                
                _start = drag.Point; 
                return (true, true);

            
            default:
                return (false, false);
        }
    }
}
