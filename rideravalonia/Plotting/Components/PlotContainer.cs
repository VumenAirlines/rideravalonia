using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Skia;
using Avalonia.Threading;
using rideravalonia.Plotting.Rendering;
using rideravalonia.Plotting.UserInput;
using rideravalonia.Plotting.UserInput.Inputs;
using Size = Avalonia.Size;

namespace rideravalonia.Plotting.Components;

/// <summary>
///     The main container for the plot
/// </summary>
public class PlotContainer : Control
{
    public Plot? _plot;

    public PlotContainer()
    {
        ClipToBounds = true;
        InputHandler = new InputHandler(this);
    }

    private InputHandler InputHandler { get; }

    #region Properties

    public static readonly StyledProperty<IBrush> FillProperty =
        AvaloniaProperty.Register<PlotContainer, IBrush>(nameof(Fill), Brushes.LightBlue);

    public IBrush Fill
    {
        get => GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    #endregion

    #region Render

    
    public override void Render(DrawingContext context)
    {
        if (_plot is null)
        {
            return;
        }

        Rect controlBounds = new(Bounds.Size);
        CustomDrawOp customDrawOp = new(controlBounds, _plot);
        context.Custom(customDrawOp);
    }

    
    public void Invalidate()
    {
        //Dispatcher.UIThread.Invoke(InvalidateVisual, DispatcherPriority.Background);
        
            if (Dispatcher.UIThread.CheckAccess())
            {
                InvalidateVisual();
            }
            else
            {
                Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Render);
            }
        
       
    }

    #endregion

    #region Events

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        Invalidate();
        _plot?.CoordinateManager.UpdateTransform(new Rect(Bounds.Size));
        base.OnSizeChanged(e);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        _plot ??= new Plot(new Rect(finalSize));
        return base.ArrangeOverride(finalSize);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        var pos = e.GetPosition(this);
        InputHandler.Process(new MouseMove(pos.ToSKPoint()));
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        var pos = e.GetPosition(this);
        IUserInput input = e.GetCurrentPoint(this).Properties.PointerUpdateKind switch
        {
            PointerUpdateKind.LeftButtonPressed => new LeftMouseDown(pos.ToSKPoint()),
            _ => new LeftMouseDown(pos.ToSKPoint())
        };
        InputHandler.Process(input);
        e.Pointer.Capture(this);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        var pos = e.GetPosition(this);
        IUserInput input = e.GetCurrentPoint(this).Properties.PointerUpdateKind switch
        {
            PointerUpdateKind.LeftButtonReleased => new LeftMouseUp(pos.ToSKPoint()),
            _ => new LeftMouseUp(pos.ToSKPoint())
        };
        InputHandler.Process(input);
        e.Pointer.Capture(null);
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        var pos = e.GetPosition(this);
        IUserInput input = e.Delta.Y switch
        {
            < 0 => new MouseWheelDown(pos.ToSKPoint()),
            _ => new MouseWheelUp(pos.ToSKPoint())
        };
        InputHandler.Process(input);
        e.Handled = true;
    }

    #endregion
}
