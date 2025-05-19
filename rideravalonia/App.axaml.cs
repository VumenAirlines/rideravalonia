using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using rideravalonia.ViewModels;
using rideravalonia.Views;
using Splat;

namespace rideravalonia;

public partial class App : Application
{
    private IClassicDesktopStyleApplicationLifetime? _desktop;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _desktop= desktop;
            
            var mainWindow = new MainWindow()
            {
                DataContext = Locator.Current.GetService<MainWindowViewModel>()
            };
            _desktop.MainWindow = mainWindow;
            _desktop.MainWindow.Activate();
            _desktop.MainWindow.Show();
        }

        base.OnFrameworkInitializationCompleted();
#if DEBUG
        // Required by Avalonia XAML editor to recognize custom XAML namespaces. Until they fix the problem.
        //GC.KeepAlive(typeof(SvgImage));
       //GC.KeepAlive(typeof(EventTriggerBehavior));

        if (Design.IsDesignMode)
        {
            base.OnFrameworkInitializationCompleted();
            return;
        }
#endif
    }
}
