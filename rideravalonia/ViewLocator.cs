// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;
using rideravalonia.ViewModels;

namespace rideravalonia;
public class ViewLocator : IViewLocator
{
    

    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
    {
        var name = viewModel.GetType().FullName.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null && Activator.CreateInstance(type) is IViewFor view)
        {
            return view;
        }

        // If view is not found, return a default placeholder.
        return new TextBlock { Text = $"View not found for {name}" } as IViewFor;
    }

  
}
