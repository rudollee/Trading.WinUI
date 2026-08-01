namespace Trading.WinUI.Converters;
public partial class DecimalToShortStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is null) return string.Empty;

		if (!decimal.TryParse(value.ToString(), out decimal decimalNumber)) return string.Empty;

		if (parameter is null || !int.TryParse(parameter.ToString(), out int scale)) scale = decimalNumber.Scale;

		var result = decimalNumber switch
		{
			> 1_000_000_000 => $"{Math.Round(decimalNumber * 0.000_000_001m, scale)}B",
			> 1_000_000 => $"{Math.Round(decimalNumber * 0.000_001m, scale)}M",
			> 1_000 => $"{Math.Round(decimalNumber * 0.001m, scale)}K",
			_ => $"{decimalNumber} "
		};
		return result;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) =>
		value is null ? 0 : decimal.TryParse(value.ToString(), out decimal decimalNumber) ? (object)decimalNumber : 0;
}
