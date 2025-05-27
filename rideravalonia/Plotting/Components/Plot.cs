// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Avalonia;
using rideravalonia.Plotting.Components.Plottables;
using rideravalonia.Plotting.Components.Styles;
using rideravalonia.Plotting.Interfaces;
using rideravalonia.Plotting.Rendering;
using rideravalonia.Plotting.UserInput;
using SkiaSharp;

namespace rideravalonia.Plotting.Components;
public class Plot : IDisposable
{
    public List<IPlottable> Plottables { get; } = [new Axis(),new FunctionPlot()];
    private RenderManager RenderManager { get; }
    public readonly BackgroundStyle BackgroundStyle = new();
    public readonly CoordinateManager CoordinateManager;
    
   
    public object Sync { get; } = new();
    
    
    public Plot(Rect bounds)
    {
        RenderManager = new RenderManager(this);
        CoordinateManager=new CoordinateManager(bounds);
    }
    
    public void Render(SKCanvas canvas, Rect rect)
    {
        lock (Sync)
        {
            RenderManager.Render(canvas, rect);
        }
    }
    
    public void Dispose()
    {
        foreach (IPlottable plottable in Plottables)
        {
            plottable.Dispose();
        }
        BackgroundStyle.Dispose();
    }
}
