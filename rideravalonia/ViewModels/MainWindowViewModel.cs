using System.Reactive.Disposables;
using ReactiveUI;
using Splat;

namespace rideravalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase, IScreen, IActivatableViewModel
{
    public RoutingState Router { get; } = new();
    public ViewModelActivator Activator { get; } = new();

    public MainWindowViewModel()
    {
        this.WhenActivated(() =>
        {
            var firstViewModel = Locator.Current.GetService<FirstViewModel>();
            if (firstViewModel is not null)
                Router.Navigate.Execute(firstViewModel);
            return [Disposable.Empty];
        });
    }
}
