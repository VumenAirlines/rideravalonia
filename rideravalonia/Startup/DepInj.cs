// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Exp_Parser.Engine;
using rideravalonia.Services;
using rideravalonia.ViewModels;
using Splat;

namespace rideravalonia.Startup;


public static class DepInj
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        RegisterServices(services, resolver);
        RegisterViewModels(services, resolver);
    }

    private static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<IPlotService>(() => new PlotService());
        services.RegisterLazySingleton<IExpTokenizer>(() => new Tokenizer());
        services.RegisterLazySingleton<IExpParser>(()=>new Parser());
    }

    private static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton(() => new MainWindowViewModel());
        services.Register(() => new FirstViewModel(
            resolver.GetService<MainWindowViewModel>(),
          resolver.GetService<IPlotService>()
        ));

    }
    
   
}
