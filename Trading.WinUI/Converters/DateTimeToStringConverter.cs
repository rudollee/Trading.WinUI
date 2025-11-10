namespace Trading.WinUI.Converters;
public partial class DateTimeToStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is null) return "";

		var datetime = (DateTime)value;
		if (datetime == DateTime.MinValue) return string.Empty;

		var datetimeFormat = parameter is null ? "yyyy-MM-dd HH:mm:ss" : parameter.ToString();

		return datetime.ToString(datetimeFormat);
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}