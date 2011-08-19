using System.Windows.Data;

namespace Client.Converters {

	public class TextConvereter : IValueConverter {

		public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			string text = value as string;
			return text.Replace("\r", "");
		}

		public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			return value;
		}

	}
}
