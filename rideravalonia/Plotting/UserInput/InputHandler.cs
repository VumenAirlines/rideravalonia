// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Avalonia.Input;
using rideravalonia.Plotting.Components;
using rideravalonia.Plotting.UserInput.Inputs;
using rideravalonia.Plotting.UserInput.UserInputAction;

namespace rideravalonia.Plotting.UserInput;

public class InputHandler(PlotContainer container)
{
    private PlotContainer Container { get; } = container;
    public bool IsEnabled { get; set; } = true;
    public readonly List<IUserInputAction> Actions = [new MouseOverGraphAction(),new MouseDoubleClickAction(),new MousePanAction(),new MouseWheelZoom()];
    private IUserInputAction? _lockedAction;
    private readonly HashSet<Key> _pressedKeys = []; //todo:make not ava
    public void Process(IUserInput input)
    {
        if(!IsEnabled || Container._plot is null) return;
        
        switch (input)
        {
            case KeyUp keyUp:
                _pressedKeys.Remove(keyUp.key);
                break;
            case KeyDown keyDown:
                _pressedKeys.Add(keyDown.key);
                break;
        }

        if(Execute(input))
            Container.Invalidate();
    }

    private bool Execute(IUserInput input)
    {
        
        bool refresh = false;
        if (Container._plot?.Sync is null)
            return refresh;
        

        lock (Container._plot?.Sync!)
        {
            foreach (var action in Actions)
            {
                if (_lockedAction is not null && _lockedAction != action)
                    continue;
                var res = action.Execute(input, Container,_pressedKeys);
                _lockedAction = res.isLocked switch
                {
                    true when _lockedAction is null => action,
                    false when _lockedAction == action => null,
                    _ => _lockedAction
                };
                refresh = res.isRefresh;
            }
        }


        return refresh;
    }
}
