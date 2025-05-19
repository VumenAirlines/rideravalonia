// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.



namespace rideravalonia.Plotting.Rendering.RenderActions;
/// <summary>
/// Responsible for rendering the background of the <see cref="Interfaces.IPlottable"/>
/// </summary>
public class RenderPlottables : IRenderAction
{
    public void Render(RenderPack rp)
    {
        if (rp.Plot.Plottables.Count == 0)
            return;

        rp.Canvas.Save();
        //rp.Canvas.SetMatrix(rp.Plot.CoordinateManager.TransformedSpace); 

        foreach (var plottable in rp.Plot.Plottables)
            plottable.Render(rp);

        rp.Canvas.Restore();
    }
}

