// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using rideravalonia.Plotting.Components;

namespace rideravalonia.Services;

public class PlotService :  IPlotService
{
    private PlotContainer? _container;

    public void Register(PlotContainer container)
    {
        _container = container;
    }
    public void UpdateFunction(object data)
    {
        _container?.UpdateFunction(data as Func<double,double>);
    }

    public void ClearPlot()
    {
        _container?.ClearPlot();
    }
}
