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

/// <summary>
/// Container for an <see cref="IPlottable"/>> object
/// </summary>
public class Plot : IDisposable
{
    public List<IPlottable> Plottables { get; } = [new FunctionPlot(),new Axis()];
    public RenderManager RenderManager { get; }
    public BackgroundStyle BackgroundStyle = new();
    public CoordinateManager CoordinateManager;
    
    /// <summary>
    /// Object used to lock access to the <see cref="Plot"/>
    /// </summary>
    public object Sync { get; } = new();
    
    /// <summary>
    /// Creates a new instance of <see cref="Plot"/>
    /// </summary>
    /// <param name="bounds"><see cref="Rect"/> describing the screen dimensions</param>
    public Plot(Rect bounds)
    {
        RenderManager = new RenderManager(this);
        CoordinateManager=new CoordinateManager(bounds);
    }
    /// <summary>
    /// Begins the rendering of <see cref="IPlottable"/>s
    /// </summary>
    /// <param name="canvas"><see cref="SKCanvas"/> to drawn on</param>
    /// <param name="rect"><see cref="Rect"/> describing the screen dimensions</param>
    public void Render(SKCanvas canvas, Rect rect)
    {
        lock (Sync)
        {
            this.RenderManager.Render(canvas, rect);
        }
    }
    /// <summary>
    /// Disposes of <see cref="BackgroundStyle"/>
    /// </summary>
    public void Dispose()
    {
        BackgroundStyle.Dispose();
    }
}
