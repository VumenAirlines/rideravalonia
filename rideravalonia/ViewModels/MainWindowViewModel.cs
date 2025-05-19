// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Avalonia.ReactiveUI;
using ReactiveUI;
namespace rideravalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase, IScreen
{
    public RoutingState Router { get; } = new RoutingState();

    public MainWindowViewModel()
    {
        Router.Navigate.Execute(new FirstViewModel(this));
        
    }
}
