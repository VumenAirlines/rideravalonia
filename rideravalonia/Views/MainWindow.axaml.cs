using Avalonia.Controls;
using ReactiveUI;
using rideravalonia.ViewModels;

using Splat;

namespace rideravalonia.Views;

public partial class MainWindow :  Window,IViewFor<MainWindowViewModel>
{
    public MainWindow()
    {
        ViewModel = Locator.Current.GetService<MainWindowViewModel>();
        DataContext = ViewModel;

        InitializeComponent(); 
        
        this.WhenActivated(disposables => 
        {
            // Your reactive bindings here
        });
    }


    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (MainWindowViewModel?)value;
    }

    public MainWindowViewModel? ViewModel { get; set; }
}
