// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using rideravalonia.Plotting.Components;
using rideravalonia.Plotting.UserInput.UserInputAction;

namespace rideravalonia.Plotting.UserInput;

public class InputHandler(PlotContainer container)
{
    public PlotContainer Container { get; } = container;
    public bool IsEnabled { get; set; } = true;
    public readonly List<IUserInputAction> Actions = [new MousePanAction(),new MouseWheelZoom()];
    private IUserInputAction? _lockedAction;
    public void Process(IUserInput input)
    {
        if(!IsEnabled || Container._plot is null) return;
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
                var res = action.Execute(input, Container);
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
