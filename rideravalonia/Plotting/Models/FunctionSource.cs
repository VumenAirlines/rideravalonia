// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using rideravalonia.Plotting.Interfaces;

namespace rideravalonia.Plotting.Models;

public class FunctionSource(Func<double,double> function) : IDataSource
{
    public Func<double, double> Function { get; set; } = function;

    public double Get(double x)
    {
        return Function(x);
    }
}
