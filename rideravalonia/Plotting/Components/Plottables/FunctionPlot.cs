// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using rideravalonia.Plotting.Interfaces;
using rideravalonia.Plotting.Models;
using rideravalonia.Plotting.Rendering;
using rideravalonia.Plotting.UserInput;
using SkiaSharp;

namespace rideravalonia.Plotting.Components.Plottables;

public class FunctionPlot:IPlottable
{
    public bool IsVisible { get; set; }


    public FunctionSource FSrc { get; set; } = new(x =>
        Math.Sin(3 * x) + Math.Sin(9.43 * x) + Math.Sin(3.7 * x) + Math.Sin(3.2 * x)
    );

    private const float MaxDistance  = 5.0f;
    private const float SmallNumber = 1e-6f;
    
    private double _oldWidth;
    
    private readonly SKPaint _paint = new()
    {
        Style = SKPaintStyle.Stroke,
        IsAntialias = true,
        StrokeWidth = 3,
        StrokeJoin = SKStrokeJoin.Round,
        StrokeCap = SKStrokeCap.Round
    };

    private SKShader? _shader;
    private SKBitmap? _distanceBitmap;


    private readonly SKPath _path = new();
    private readonly SKMatrix _shaderMatrix = SKMatrix.CreateScale(0.5f, 1f); //Do we need to calculate this at runtime?
    


    public void Render(RenderPack rp)
    {
        CoordinateManager cm = rp.Plot.CoordinateManager;
        if (_distanceBitmap is null || rp.Bounds.Width - _oldWidth > SmallNumber)
        {
            _oldWidth = rp.Bounds.Width;
            _distanceBitmap?.Dispose();
            _distanceBitmap = new SKBitmap((int)(_oldWidth * 2) + 1, 1);
        }
       
            
        float step = (float)double.Max((cm.Right - cm.Left) / (_oldWidth * 2), SmallNumber);
        bool drawing = false;
        
        _path.Reset();
        _shader?.Dispose();

        using var pixels = _distanceBitmap.PeekPixels();
        var bitmapColors = pixels.GetPixelSpan<SKColor>();
        
        
        int i = 0;
        for (float x = cm.Left; x<=cm.Right; x+=step)
        {
            float y = (float)FSrc.Get(x);

            if (!float.IsFinite(y))
            {
                drawing = false;
                if (i < bitmapColors.Length)
                    bitmapColors[i++] = SKColors.Transparent;
                continue;
            }
            
            if (y < cm.Bottom || y > cm.Top)
                y =  Math.Clamp(y, cm.Bottom-1, cm.Top+1);
            
            
            float expectedY = 0; //Math.Sin(x); 
            
            if (i < bitmapColors.Length) 
                bitmapColors[i++] = InterpolateSignedHSV( expectedY - y, MaxDistance);
            
            SKPoint pos = cm.GetScreenSpacePoint(new SKPoint(x, y));
            
            if (drawing) 
                _path.LineTo(pos);
            else
            {
                _path.MoveTo(pos);
                drawing = true;
            }
        }
        
        
        
        _shader = SKShader
            .CreateBitmap(_distanceBitmap, SKShaderTileMode.Clamp, SKShaderTileMode.Clamp)
            .WithLocalMatrix(_shaderMatrix);
        _paint.Shader = _shader;
        
        rp.Canvas.DrawPath(_path, _paint);

    }
    
    private static SKColor InterpolateSignedHSV(float distance, float maxDistance, byte alpha = 255)
    {
        float t = float.Clamp(distance / maxDistance, -1.0f, 1.0f);
      
        float hue = (1 + t) * 120; 
        return SKColor.FromHsv(hue, 100.0f, 100.0f, alpha);
    }

    private SKMatrix UpdateMatrix( CoordinateManager cm)
    {
        if (_distanceBitmap is null)
            return SKMatrix.Empty;
        float screenLeftX = cm.GetScreenSpacePoint(new SKPoint(cm.Left, 0)).X;
        float screenRightX = cm.GetScreenSpacePoint(new SKPoint(cm.Right, 0)).X;
        
        float screenWidth = screenRightX - screenLeftX;
        float scaleX = screenWidth / _distanceBitmap.Width;
        
        return SKMatrix.CreateScale(scaleX, 1f).PreConcat(SKMatrix.CreateTranslation(screenLeftX, 0));
    }

    public void Dispose()
    {
        _paint.Dispose();
        _shader?.Dispose();
        _path.Dispose();
        _distanceBitmap?.Dispose();
    }
}
