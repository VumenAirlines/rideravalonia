// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using rideravalonia.Models;

namespace rideravalonia.Controls;

public partial class InputHistory : ReactiveUserControl<InputHistory>
{
    public static readonly StyledProperty<ICommand> OnClickProperty =
        AvaloniaProperty.Register<InputHistory, ICommand>(nameof(OnClick));
    public static readonly StyledProperty<FunctionInput> InputProperty =
        AvaloniaProperty.Register<InputHistory, FunctionInput>(nameof(Input));
    
    
    public FunctionInput Input
    {
        get => GetValue(InputProperty);
        set => SetValue(InputProperty, value);
    }

    public ICommand OnClick
    {
        get => GetValue(OnClickProperty);
        init => SetValue(OnClickProperty, value);
    }

    public InputHistory()
    {
        AvaloniaXamlLoader.Load(this);
        this.GetObservable(InputProperty)
            .Subscribe(input =>
            {
                InvalidateVisual();
            });
       
    }

    private void OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (OnClick?.CanExecute(Input.Id) == true)
        {
            OnClick.Execute(Input.Id);
        }
    }
}
    


