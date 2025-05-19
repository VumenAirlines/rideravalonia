
using Avalonia;
using Avalonia.Skia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Threading;
using Avalonia.Controls;
using rideravalonia.Plotting.Rendering;
using rideravalonia.Plotting.UserInput;
using rideravalonia.Plotting.UserInput.Inputs;
using SkiaSharp;
using Point = Avalonia.Point;
using Size = Avalonia.Size;


namespace rideravalonia.Plotting.Components
{
    /// <summary>
    /// The main container for the plot
    /// </summary>
    public class PlotContainer : Control
    {
        #region Properties
        
        public static readonly StyledProperty<IBrush> FillProperty =
            AvaloniaProperty.Register<PlotContainer, IBrush>(nameof(Fill), Brushes.LightBlue);
        public IBrush Fill
        {
            get => GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        #endregion
        
        public Plot? _plot;
        public InputHandler InputHandler { get; } 
     
        public PlotContainer()
        {
            ClipToBounds = true;
            InputHandler =new InputHandler(this) ;
        }


        #region Render
        
        /// <summary>
        /// The render method for the control
        /// </summary>
        /// <param name="context"><see cref="DrawingContext"/> of the canvas</param>
        public override void Render(DrawingContext context)
        {
            if(_plot is null) return;
            Rect controlBounds = new(Bounds.Size);
            CustomDrawOp customDrawOp = new(controlBounds, _plot);
            context.Custom(customDrawOp);
        }
        /// <summary>
        /// Used for invalidating the canvas visual, triggering a re-render
        /// </summary>
        public void Invalidate()
        {
            Dispatcher.UIThread.Invoke(InvalidateVisual, DispatcherPriority.Background);
        }

        #endregion
        
        
        #region Events
        protected override Size ArrangeOverride(Size finalSize)
        {
            _plot ??= new Plot(new Rect(finalSize));
            return base.ArrangeOverride(finalSize);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            Point pos = e.GetPosition(this);
            InputHandler.Process(new MouseMove(pos.ToSKPoint()));
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            Point pos = e.GetPosition(this);
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
            Point pos = e.GetPosition(this);
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
            Point pos = e.GetPosition(this);
            IUserInput input = e.Delta.Y switch
            {
                <0 => new MouseWheelDown(pos.ToSKPoint()),
                _ => new MouseWheelUp(pos.ToSKPoint())
            };
            InputHandler.Process(input);
            e.Handled = true;
        }
        

        #endregion

    }
}
