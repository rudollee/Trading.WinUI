namespace Trading.WinUI.Converters;
public partial class DateToStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		var date = (DateOnly)value;
		if (date.Equals(new DateOnly(1, 1, 1))) return string.Empty; // 0001-01-01 case

		var dateFormat = parameter is null ? "yyyy-MM-dd" : parameter.ToString();
		return date.ToString(dateFormat);
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}