// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Avalonia.Media;
using SkiaSharp;
using Avalonia;

using Avalonia.Skia;

namespace rideravalonia.Plotting.Components.Styles;

public class BackgroundStyle : IDisposable
{
    public Color Color { get; set; } = Colors.White;

    private SKBitmap? SkBitmap { get; set; }
  
    public bool _antiAlias = true;

    private SKImage? _image;
    public SKImage? Image
    {
        get => _image; 
        set
        {
            _image = value;
            if (value is null) return;
            SkBitmap = SKBitmap.FromImage(value); // TODO: SKImage instead?
        }
    }

    public void Dispose()
    {
        SkBitmap?.Dispose();
    }

   

    public void Render(SKCanvas canvas, Rect target)
    {
        using SKPaint paint = new();
        paint.Color = Color.ToSKColor();
        canvas.DrawRect(target.ToSKRect(), paint);
       
        /*if (Image is not null)
        {
            PixelRect imgRect = ImagePosition.GetRect(Image.Size, target);
            Image.Render(canvas, imgRect, paint, AntiAlias);
        }*/
    }
}
