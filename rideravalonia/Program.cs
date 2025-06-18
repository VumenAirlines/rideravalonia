using Avalonia;
using rideravalonia.Startup;
using Splat;

namespace rideravalonia;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        DepInj.Register(Locator.CurrentMutable, Locator.Current);

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args); //AppStarter.Start<App>(args, () => {}, () => { });
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppStarter.BuildAvaloniaApp<App>();
    }
}
