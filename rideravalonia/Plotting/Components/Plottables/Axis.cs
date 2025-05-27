// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using rideravalonia.Plotting.Interfaces;
using rideravalonia.Plotting.Rendering;
using SkiaSharp;

namespace rideravalonia.Plotting.Components.Plottables;

public class Axis : IPlottable
{
    private const float LineOffset = 5f;
    private const float Padding = 2f;
    private const float VerticalTextOffset = 8f;
    private const float HorizontalTextOffset = 5f;

    private readonly SKPaint _axisPaint = new() { Color = SKColors.Black, StrokeWidth = 2, IsAntialias = true };

    private readonly SKPaint _backgroundPaint = new()
    {
        Color = SKColors.White, IsAntialias = true, Style = SKPaintStyle.Fill
    };

    private readonly SKPaint _gridPaint = new() { Color = SKColors.LightGray, StrokeWidth = 1, IsAntialias = true };

    private readonly List<Label> _labels = new(200);

    private readonly SKPaint _majorPaint = new() { Color = SKColors.LightGray, StrokeWidth = 2, IsAntialias = true };
    private double _scale;

    private double Scale
    {
        get => _scale;
        set => _scale = SpaceTicks(value / 5d);
    }

    private double SubTicks
    {
        get => Scale / 5d;
    }

    public bool IsVisible { get; set; } = true;

    public void Render(RenderPack rp)
    {
        var cm = rp.Plot.CoordinateManager;
        _labels.Clear();

        Scale = cm.CoordinateScale;

        var left = cm.GetScreenSpacePoint(new SKPoint(cm.Left, 0));
        var right = cm.GetScreenSpacePoint(new SKPoint(cm.Right, 0));
        var bottom = cm.GetScreenSpacePoint(new SKPoint(0, cm.Bottom));
        var top = cm.GetScreenSpacePoint(new SKPoint(0, cm.Top));

        for (double i = RoundToMultiple(cm.Bottom, SubTicks); i <= cm.Top; i += SubTicks)
        {
            if (Math.Abs(i) < 1e-6)
            {
                continue;
            }

            var pos = cm.GetScreenSpacePoint(new SKPoint(0, (float)i));
            var isMajor = IsMajorTick(i);
            if (isMajor)
            {
                _labels.Add(new Label(pos, i, true));
            }

            rp.Canvas.DrawLine(new SKPoint(left.X, pos.Y), new SKPoint(right.X, pos.Y),
                isMajor ? _majorPaint : _gridPaint);
        }

        for (double i = RoundToMultiple(cm.Left, SubTicks); i <= cm.Right; i += SubTicks)
        {
            if (Math.Abs(i) < 1e-6)
            {
                continue;
            }

            var pos = cm.GetScreenSpacePoint(new SKPoint((float)i, 0));
            var isMajor = IsMajorTick(i);
            if (isMajor)
            {
                _labels.Add(new Label(pos, i));
            }

            rp.Canvas.DrawLine(new SKPoint(pos.X, bottom.Y), new SKPoint(pos.X, top.Y),
                isMajor ? _majorPaint : _gridPaint);
        }

        //labels

        for (var i = 0; i < _labels.Count; i++)
        {
            DrawLabel(rp.Canvas, _labels[i].Pos, _labels[i].LabelText,
                _labels[i].IsVertical);
        }

        //main axes
        rp.Canvas.DrawLine(left, right, _axisPaint);
        rp.Canvas.DrawLine(bottom, top, _axisPaint);
    }

    public void Dispose()
    {
        _axisPaint.Dispose();
        _majorPaint.Dispose();
        _gridPaint.Dispose();
        _backgroundPaint.Dispose();
    }

    private void DrawLabel(SKCanvas canvas, SKPoint pos, double labelVal, bool isVertical)
    {
        var label = FormatLabel(labelVal);

        var labelWidth = _axisPaint.MeasureText(label);
        var labelHeight = _axisPaint.TextSize;

        var textX = isVertical ? -labelWidth - VerticalTextOffset : -labelWidth + HorizontalTextOffset;
        var textY = isVertical ? labelHeight / 2 - 2 : labelHeight + HorizontalTextOffset;

        var textStart = new SKPoint(pos.X + textX, pos.Y + textY);
        var background = new SKRect(textStart.X - Padding, textStart.Y - labelHeight,
            textStart.X + labelWidth + Padding, textStart.Y + Padding);

        canvas.DrawRect(background, _backgroundPaint);

        if (isVertical)
        {
            canvas.DrawLine(new SKPoint(pos.X - LineOffset, pos.Y), new SKPoint(pos.X + LineOffset, pos.Y), _axisPaint);
        }
        else
        {
            canvas.DrawLine(new SKPoint(pos.X, pos.Y - LineOffset), new SKPoint(pos.X, pos.Y + LineOffset), _axisPaint);
        }

        canvas.DrawText(label, textStart, _axisPaint);
    }

    private static float RoundToMultiple(double value, double multiple)
    {
        return (float)(Math.Round(value / multiple) * multiple);
    }

    private bool IsMajorTick(double value)
    {
        var tolerance = Math.Max(Scale * 1e-10, SubTicks * 0.1);
        var nearestMajor = Math.Round(value / Scale) * Scale;
        return Math.Abs(value - nearestMajor) <= tolerance;
    }

    private static double SpaceTicks(double scale)
    {
        var magnitude = Math.Pow(10, Math.Floor(Math.Log10(scale)));
        var normalized = scale / magnitude;
        return normalized switch
        {
            <= 1f => magnitude,
            <= 2f => magnitude * 2f,
            <= 5f => magnitude * 5f,
            _ => magnitude * 10f
        };
    }

    private string FormatLabel(double value)
    {
        var decimals = Math.Max(0, -(int)Math.Floor(Math.Log10(Scale)));
        return Math.Round(value, decimals).ToString($"F{decimals}", CultureInfo.InvariantCulture);
    }
}

public readonly struct Label(SKPoint pos, double labelText, bool isVertical = false)
{
    public SKPoint Pos { get; } = pos;
    public double LabelText { get; } = labelText;
    public bool IsVertical { get; } = isVertical;
}
