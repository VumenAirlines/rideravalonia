using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using rideravalonia.ViewModels;
using Splat;

namespace rideravalonia;

public class App : Application
{
    //private IClassicDesktopStyleApplicationLifetime? _desktop;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
#if DEBUG
        if (Design.IsDesignMode)
        {
            base.OnFrameworkInitializationCompleted();
            return;
        }
#endif

        try
        {
           
           
           

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainViewModel = Locator.Current.GetService<MainWindowViewModel>();
                if (mainViewModel == null)
                    throw new InvalidOperationException("MainWindowViewModel not registered in DI container");
                
                desktop.MainWindow = new Views.MainWindow { DataContext = mainViewModel };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Startup error: {ex.Message}");
            throw;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
