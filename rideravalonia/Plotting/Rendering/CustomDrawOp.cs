// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using rideravalonia.Plotting.Components;
using SkiaSharp;

namespace rideravalonia.Plotting.Rendering;
/// <summary>
/// Class for rendering onto the <see cref="SKCanvas"/> of the control
/// </summary>
/// <param name="bounds">The <see cref="Rect"/> representing the screen dimensions</param>
/// <param name="plot">The <see cref="Plot"/> to be drawn</param>
public class CustomDrawOp(Rect bounds, Plot plot) : ICustomDrawOperation
{
    public Rect Bounds { get; } = bounds;
    public bool HitTest(Point p) => true;
    public bool Equals(ICustomDrawOperation? other) => false;

    public void Dispose()
    {
        // No-op
    }

    public void Render(ImmediateDrawingContext context)
    {
        ISkiaSharpApiLeaseFeature? leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
        if (leaseFeature is null) return;

        using ISkiaSharpApiLease lease = leaseFeature.Lease();
                
        using (var _ = new SKAutoCanvasRestore(lease.SkCanvas, false))
        {
            lease.SkCanvas.Save();
                 
            plot.Render(lease.SkCanvas, new Rect((Bounds.Size)));
                   
            lease.SkCanvas.Restore();
        }
        lease.SkCanvas.SaveLayer();
    }
}
