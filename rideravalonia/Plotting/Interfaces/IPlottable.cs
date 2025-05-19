// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using rideravalonia.Plotting.Rendering;

namespace rideravalonia.Plotting.Interfaces;

public interface IPlottable
{
    /// <summary>
    /// Controls whether the plot is rendered or not
    /// </summary>
    bool IsVisible { get; set; }
    /// <summary>
    /// Render method of the <see cref="IPlottable"/>
    /// </summary>
    /// <param name="rp"><see cref="RenderPack"/> of the <see cref="IPlottable"/></param>
    void Render(RenderPack rp);

 
}
