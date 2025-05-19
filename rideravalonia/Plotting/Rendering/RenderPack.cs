// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Avalonia;
using rideravalonia.Plotting.Components;
using SkiaSharp;

namespace rideravalonia.Plotting.Rendering;

public class RenderPack(SKCanvas canvas,Plot plot, Rect rect) : IDisposable
{
    public SKCanvas Canvas { get; } = canvas;
    public Plot Plot { get; } = plot;
    public Rect Bounds { get; } = rect;
    public SKPaint Paint { get; } = new ();
    public void Dispose()
    {
        Paint.Dispose();
    }
}
