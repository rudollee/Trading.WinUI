namespace Trading.WinUI.Converters;

public partial class DecimalToPercentageStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null) return string.Empty;

        if (!decimal.TryParse(value.ToString(), out decimal decimalNumber)) return string.Empty;

        decimalNumber *= 100;

        var parameterString = parameter?.ToString() ?? "0";

        if (string.IsNullOrWhiteSpace(parameterString) || !int.TryParse(parameterString[..1], out int precision)) precision = 0;

        var mark = parameterString.Length > 1 ? parameterString[1..] : "";

        return decimalNumber.ToString($"N{precision}") + mark;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (decimal.TryParse(value.ToString(), out decimal decimalNumber)) return decimalNumber / 100;
        if (decimal.TryParse(value.ToString()[..1], out decimalNumber)) return decimalNumber / 100;

        return 0;
    }
}
