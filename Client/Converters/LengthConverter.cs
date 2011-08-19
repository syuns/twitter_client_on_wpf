using System.Windows.Data;
using Client.Model.Twitter.Entities;

namespace Client.Converters {

	public class LengthConverter : IValueConverter {

		public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			int? v = value as int?;
			return Status.TweetMaxLength - v;
		}

		public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			return value;
		}

	}

}
