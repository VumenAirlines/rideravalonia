// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.



namespace rideravalonia.Plotting.Rendering.RenderActions;
/// <summary>
/// Represents a step in the render pipeline
/// </summary>
public interface IRenderAction
{
    /// <summary>
    /// Starts a step in the render pipeline
    /// </summary>
    /// <param name="rp"><see cref="RenderPack"/> of the <see cref="Plotting.Interfaces.IPlottable"/></param>
    public void Render(RenderPack rp);
}
