namespace Trading.WinUI.Converters;
public partial class DecimalToStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is null) return string.Empty;

		if (!decimal.TryParse(value.ToString(), out decimal decimalNumber)) return string.Empty;

		if (parameter is null || !Int32.TryParse(parameter.ToString(), out int scale)) scale = decimalNumber.Scale;

		return decimalNumber.ToString($"N{scale}");
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) => 
		value is null ? 0 : decimal.TryParse(value.ToString(), out decimal decimalNumber) ? (object)decimalNumber : 0;
}
