namespace Trading.WinUI.Converters;
public partial class TimeToStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		var time = (TimeOnly)value;
		if (time == TimeOnly.MinValue) return string.Empty;

		var timeFormat = parameter is null ? "HH:mm:ss.ff" : parameter.ToString();

		return time.ToString(timeFormat);
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) =>
		throw new NotImplementedException();
}