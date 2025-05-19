// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.



namespace rideravalonia.Plotting.Rendering.RenderActions;
/// <summary>
/// Responsible for rendering the background of the <see cref="Components.PlotContainer"/>
/// </summary>
public class RenderBackground : IRenderAction
{
    public void Render(RenderPack rp)
    {
        rp.Plot.BackgroundStyle.Render(rp.Canvas,rp.Bounds);
        
    }
}
