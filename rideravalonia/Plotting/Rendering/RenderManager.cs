// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Linq;
using Avalonia;
using rideravalonia.Plotting.Components;
using rideravalonia.Plotting.Rendering.RenderActions;
using SkiaSharp;

namespace rideravalonia.Plotting.Rendering;


public class RenderManager(Plot plot)
{
    private HashSet<IRenderAction> Actions { get; } = [new RenderBackground(), new RenderPlottables()];
    public bool EnableRendering { get; set; } = true;
    private Plot Plot { get; } = plot;

    
    private readonly List<double> fps = [];

    public void Render(SKCanvas canvas, Rect rect)
    {
        RenderOnce(canvas, rect);
    }

   
    private void RenderOnce(SKCanvas canvas, Rect rect)
    {
        
        if (!EnableRendering) return;
        RenderPack rp = new RenderPack(canvas, Plot, rect);
        Stopwatch sw = new Stopwatch();
        foreach (IRenderAction action in Actions)
        {
            sw.Restart();
            action.Render(rp);
            fps.Add(sw.Elapsed.TotalSeconds);
            Console.WriteLine(1 / (fps.Sum() / fps.Count));
        }
    }
}
