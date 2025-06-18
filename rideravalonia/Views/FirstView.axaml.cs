// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using rideravalonia.Plotting.Components;
using rideravalonia.Services;
using rideravalonia.ViewModels;
using Splat;

namespace rideravalonia.Views;
    public partial class FirstView : ReactiveUserControl<FirstViewModel>
    {
        
        public FirstView()
        {
            AvaloniaXamlLoader.Load(this);
            this.WhenActivated(disposables =>
            {
                var vm = DataContext as IActivatableViewModel;
                vm?.Activator.Activate().DisposeWith(disposables);
               
                var plotService = Locator.Current.GetService<IPlotService>();
                if (plotService is PlotService concreteService)
                    concreteService.Register( this.FindControl<PlotContainer>("GraphContainer"));
            });
        }
       
    }


