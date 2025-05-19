// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using rideravalonia.Plotting.Components;
using rideravalonia.Plotting.UserInput.Inputs;

namespace rideravalonia.Plotting.UserInput.UserInputAction;
/// <summary>
/// Handles zooming with the scroll wheel
/// </summary>
public class MouseWheelZoom:IUserInputAction
{
    public double ZoomValue { get; set; } = 0.15d;
    private double ZoomInValue => 1 + ZoomValue;
    private double ZoomOutValue => 1 - ZoomValue;
    public (bool isLocked, bool isRefresh) Execute(IUserInput input, PlotContainer container)
    {

        double zoomFactor = input switch
        {
            MouseWheelDown => ZoomOutValue,
            MouseWheelUp => ZoomInValue,
            _ => -1f
        };
        if(double.IsNegative(zoomFactor))
            return (false, false);
        
        container._plot?.CoordinateManager.ZoomAroundPoint(((IMouseButtonInput)input).Point,(float)zoomFactor);
        return (false, true);
    }
}
