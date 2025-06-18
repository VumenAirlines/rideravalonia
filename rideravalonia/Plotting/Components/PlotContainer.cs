using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Skia;
using Avalonia.Threading;
using rideravalonia.Plotting.Components.Plottables;
using rideravalonia.Plotting.Models;
using rideravalonia.Plotting.Rendering;
using rideravalonia.Plotting.UserInput;
using rideravalonia.Plotting.UserInput.Inputs;
using Size = Avalonia.Size;

namespace rideravalonia.Plotting.Components;


public class PlotContainer : Control
{
    public Plot? _plot;

    public PlotContainer()
    {
        ClipToBounds = true;
        InputHandler = new InputHandler(this);
        Focusable = true;
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

    public void UpdateFunction(Func<double,double> newFunc)
    {
        if(_plot?.Plottables.First(x => x is FunctionPlot) is not FunctionPlot fplot) return;
        fplot.FSrc = new FunctionSource(newFunc);
        Invalidate();
    }
    public void ClearPlot()
    {
        if(_plot?.Plottables.First(x => x is FunctionPlot) is not FunctionPlot fplot) return;
        fplot.FSrc = new FunctionSource( x=>0);
    }
    
    #region Render

    
    public override void Render(DrawingContext context)
    {
        if (_plot is null)
        {
            return;
        }

        Rect controlBounds = new(Bounds.Size);
        using (context.PushClip(controlBounds))
        {
            CustomDrawOp customDrawOp = new(controlBounds, _plot);
            context.Custom(customDrawOp);
        }
        
        //todo: changing bounds changes coloring
    }

    
    public void Invalidate()
    {
        
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
            PointerUpdateKind.LeftButtonPressed => new LeftMouseDown(pos.ToSKPoint(),e.ClickCount),
            _ => new LeftMouseDown(pos.ToSKPoint(),0) //todo rightclick
        };
        InputHandler.Process(input);
        e.Pointer.Capture(this);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        var pos = e.GetPosition(this);
        IUserInput input = e.GetCurrentPoint(this).Properties.PointerUpdateKind switch
        {
            PointerUpdateKind.LeftButtonReleased => new LeftMouseUp(pos.ToSKPoint(),0),
            _ => new LeftMouseUp(pos.ToSKPoint(),0)
        };
        InputHandler.Process(input);
        e.Pointer.Capture(null);
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        var pos = e.GetPosition(this);
        IUserInput input = e.Delta.Y switch
        {
            < 0 => new MouseWheelDown(pos.ToSKPoint(),0),
            _ => new MouseWheelUp(pos.ToSKPoint(),0)
        };
        InputHandler.Process(input);
        e.Handled = true;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        InputHandler.Process(new KeyDown(e.Key));
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {       
        InputHandler.Process(new KeyUp(e.Key));
    }

    #endregion
}
