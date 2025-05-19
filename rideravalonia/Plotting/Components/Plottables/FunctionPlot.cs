// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using rideravalonia.Plotting.Interfaces;
using SkiaSharp;

using rideravalonia.Plotting.Rendering;

namespace rideravalonia.Plotting.Components.Plottables;

public class FunctionPlot:IPlottable
{
    public bool IsVisible { get; set; }
    
    public void Render(RenderPack rp)
    {
        var transformedPoint = rp.Plot.CoordinateManager.GetScreenSpacePoint(new SKPoint(0, 0));
        //Console.WriteLine($"Transformed point for (0,0): {transformedPoint}");
        rp.Canvas.DrawCircle( transformedPoint,5f, new SKPaint()
        {
            Color = new SKColor(255, 0, 0),
            Style = SKPaintStyle.Fill
        });
        rp.Canvas.DrawCircle( rp.Plot.CoordinateManager.GetScreenSpacePoint(new SKPoint(10, 0)),5f, new SKPaint()
        {
            Color = new SKColor(255, 0, 0),
            Style = SKPaintStyle.Fill
        });
        rp.Canvas.DrawCircle( rp.Plot.CoordinateManager.GetScreenSpacePoint(new SKPoint(0, 10)),5f, new SKPaint()
        {
            Color = new SKColor(255, 0, 0),
            Style = SKPaintStyle.Fill
        });
    }
}
