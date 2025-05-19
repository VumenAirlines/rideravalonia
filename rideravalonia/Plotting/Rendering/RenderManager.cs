// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Avalonia;
using rideravalonia.Plotting.Components;
using rideravalonia.Plotting.Rendering.RenderActions;
using SkiaSharp;

namespace rideravalonia.Plotting.Rendering;
/// <summary>
/// Manages the rendering pipeline for a plot
/// </summary>
/// <param name="plot">The <see cref="Plot"/> to be rendered</param>
public class RenderManager(Plot plot) 
{
    public HashSet<IRenderAction> Actions { get; } = [ new RenderBackground(),new RenderPlottables()];
    public bool EnableRendering { get; set; } = true;
    private Plot Plot { get; } = plot;
    /// <summary>
    /// Start the render pipeline 
    /// </summary>
    /// <param name="rect">The <see cref="Rect"/> representing the screen dimensions</param>
    /// <param name="canvas"><see cref="SKCanvas"/> to drawn on</param>

    public void Render(SKCanvas canvas,Rect rect)
    {
        RenderOnce(canvas,rect);
    }
    /// <summary>
    /// Runs all the registered <see cref="IRenderAction"/>s
    /// </summary>
    /// <param name="rect">The <see cref="Rect"/> representing the screen dimensions</param>
    /// <param name="canvas"><see cref="SKCanvas"/> to drawn on</param>

    private void RenderOnce(SKCanvas canvas,Rect rect)
    {
        if(!EnableRendering) return;
        RenderPack rp = new RenderPack(canvas, Plot, rect);
        foreach (IRenderAction action in Actions)
        {
            action.Render(rp);
        }
    }
 
}
