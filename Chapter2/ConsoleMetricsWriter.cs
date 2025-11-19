namespace Chapter2;

using System.Globalization;

/// <summary>
/// Provides helper methods to write consistently formatted console output for metrics.
/// Keeps alignment and decimal formatting consistent across listeners while preserving
/// the existing output shape (including optional leading blank lines).
/// </summary>
internal static class ConsoleMetricsWriter
{
    /// <summary>
    /// Writes an integer metric right-aligned to a specific width, followed by a label.
    /// </summary>
    /// <param name="value">The metric value.</param>
    /// <param name="label">The text that follows the value (include punctuation).</param>
    /// <param name="width">The field width for right-alignment. Default is 20.</param>
    /// <param name="leadingBlankLine">Whether to write a leading blank line before the metric.</param>
    public static void WriteMetric(long value, string label, int width = 20, bool leadingBlankLine = false)
    {
        if (leadingBlankLine)
        {
            Console.WriteLine();
        }

        var valueText = value.ToString(CultureInfo.CurrentCulture);
        if (width > 0)
        {
            valueText = valueText.PadLeft(width);
        }

        Console.WriteLine($"{valueText} {label}");
    }

    /// <summary>
    /// Writes a time in seconds right-aligned to a specific width with two decimal places,
    /// followed by a label. Uses current culture as requested.
    /// </summary>
    /// <param name="seconds">The time value in seconds.</param>
    /// <param name="label">The text that follows the value (include punctuation).</param>
    /// <param name="width">The field width for right-alignment. Default is 20.</param>
    /// <param name="leadingBlankLine">Whether to write a leading blank line before the value.</param>
    public static void WriteSeconds(double seconds, string label, int width = 20, bool leadingBlankLine = false)
    {
        if (leadingBlankLine)
        {
            Console.WriteLine();
        }

        var valueText = seconds.ToString("F2", CultureInfo.CurrentCulture);
        if (width > 0)
        {
            valueText = valueText.PadLeft(width);
        }

        Console.WriteLine($"{valueText} {label}");
    }
}
