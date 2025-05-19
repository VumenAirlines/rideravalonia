// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using rideravalonia.Plotting.Components;

namespace rideravalonia.Plotting.UserInput.UserInputAction;

/// <summary>
/// A step in the input handling pipeline
/// </summary>
public interface IUserInputAction
{
    /// <summary>
    /// Handles a user input event.
    /// </summary>
    /// <param name="input">The type of input to execute as <see cref="IUserInput"/>.</param>
    /// <param name="container">The <see cref="PlotContainer"/> the event is from.</param>
    /// <returns>
    /// A tuple containing:
    /// <para>
    ///   <c>isLocked</c>: Indicates whether all successive events should only be processed by this <see cref="IUserInputAction"/>.
    /// </para>
    /// <para>
    ///   <c>isRefresh</c>: Indicates whether the canvas has to be refreshed.
    /// </para>
    /// </returns>
    
   (bool isLocked,bool isRefresh) Execute( IUserInput input,PlotContainer container);
}
