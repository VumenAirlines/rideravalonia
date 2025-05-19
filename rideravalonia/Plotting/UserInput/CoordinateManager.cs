using Avalonia;
using SkiaSharp;

namespace rideravalonia.Plotting.UserInput;


public class CoordinateManager
{
    public SKPoint Origin { get; private set; } = new SKPoint(0, 0);
    private SKMatrix TransformedSpace { get; set; } = SKMatrix.CreateIdentity();
    private SKMatrix ScreenSpace { get; set; } = SKMatrix.CreateIdentity();

    private float CoordinateScale { get; set; } = 10f;
    
    private SKPoint _scale = new(1, -1);
    private Rect _bounds;
    private SKPoint _topLeft;
    private SKPoint _bottomRight;
    public float Top { get => _topLeft.Y;  }
    public float Bottom { get => _bottomRight.Y;  }
    public float Left { get => _topLeft.X;  }
    public float Right { get => _bottomRight.X; }

    public CoordinateManager(Rect bounds)
    {
        _bounds = bounds;
        UpdateTransform(bounds);
    }

    public void UpdateTransform(Rect size)
    {
        _bounds = size;
        _scale = GetComputedScale(size);
        SKMatrix scaleMatrix = SKMatrix.CreateScale(_scale.X, _scale.Y);
        SKMatrix centerMatrix = SKMatrix.CreateTranslation((float)size.Width / 2f, (float)size.Height / 2f);
        SKMatrix panMatrix = SKMatrix.CreateTranslation(Origin.X, Origin.Y);
        
        TransformedSpace = SKMatrix.Identity.PreConcat(scaleMatrix).PostConcat(centerMatrix).PostConcat(panMatrix);
      
        if (TransformedSpace.TryInvert(out SKMatrix inv))
            ScreenSpace = inv;
        _topLeft = GetTransformSpacePoint(new SKPoint(0, 0));
        _bottomRight = GetTransformSpacePoint(new SKPoint((float)_bounds.Width, (float)_bounds.Height));
    }
 
    public void MousePan(SKPoint delta)
    {
        Origin = new SKPoint(Origin.X + delta.X, Origin.Y + delta.Y);
    }
    
   
    public void ZoomAroundPoint(SKPoint point, float factor)
    {
        SKPoint zoomPoint = GetTransformSpacePoint(point);
        CoordinateScale = Math.Clamp(CoordinateScale * factor, 1.0f, 100.0f);
        UpdateTransform(_bounds);
        SKPoint newZoomPoint = GetScreenSpacePoint(zoomPoint);
        MousePan(SKPoint.Subtract(point,newZoomPoint));
        UpdateTransform(_bounds);

    }
    
  
    public SKPoint GetScreenSpacePoint(SKPoint displayVector) => TransformedSpace.MapPoint(displayVector);
  
    public SKPoint GetScreenSpaceVector(SKPoint displayVector) => TransformedSpace.MapVector(displayVector);
 
    public SKPoint GetTransformSpacePoint(SKPoint screenVector) => ScreenSpace.MapPoint(screenVector);
 
    public SKPoint GetTransformSpaceVector(SKPoint screenVector) => ScreenSpace.MapVector(screenVector);
 
    
    private SKPoint GetComputedScale(Rect canvasSize)
    {
        // Scale based on canvas size and desired coordinate range
        float scaleFactor = (float)Math.Min(canvasSize.Width, canvasSize.Height) /(2*  CoordinateScale);
        return new SKPoint(scaleFactor, -scaleFactor); // Flip Y so positive Y is up
    }
}
