// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Avalonia;
using rideravalonia.Plotting.Interfaces;
using rideravalonia.Plotting.Rendering;
using rideravalonia.Plotting.UserInput;
using SkiaSharp;

namespace rideravalonia.Plotting.Components.Plottables;

public class Axis : IPlottable
{
    public bool IsVisible { get; set; } = true;

    public void Render(RenderPack rp)
    {
        CoordinateManager cm = rp.Plot.CoordinateManager;


        using var paint =new SKPaint();
        
        paint.Color = SKColors.Black;
        paint.StrokeWidth = 2;
        paint.IsAntialias = true;
        
       
        SKPoint left = cm.GetScreenSpacePoint(new SKPoint(cm.Left, 0));
        SKPoint right = cm.GetScreenSpacePoint(new SKPoint(cm.Right, 0));
        SKPoint bottom = cm.GetScreenSpacePoint(new SKPoint(0, cm.Bottom));
        SKPoint top = cm.GetScreenSpacePoint(new SKPoint(0, cm.Top));
            
        rp.Canvas.DrawLine(left, right, paint);
        rp.Canvas.DrawLine(bottom, top, paint);
        
    }
}



