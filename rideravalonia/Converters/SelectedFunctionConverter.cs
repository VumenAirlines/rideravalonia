// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace rideravalonia.Converters;

public class SelectedFunctionConverter : IMultiValueConverter
{
    public static readonly SelectedFunctionConverter Instance = new();
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        
        if (values is [Guid item, Guid current] && item == current)
            return new SolidColorBrush(Colors.White);
        return new SolidColorBrush(Colors.Transparent);
    }
}



