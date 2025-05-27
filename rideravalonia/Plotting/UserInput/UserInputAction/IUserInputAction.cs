// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Avalonia.Input;
using rideravalonia.Plotting.Components;

namespace rideravalonia.Plotting.UserInput.UserInputAction;


public interface IUserInputAction
{
 
    
   (bool isLocked,bool isRefresh) Execute( IUserInput input,PlotContainer container, HashSet<Key> pressedKeys);
}
