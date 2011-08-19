using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Client.Model.Twitter.Entities;

namespace Client.Model {

	/// <summary>
	/// key/value形式のデータベース
	/// NoSQL:http://ja.wikipedia.org/wiki/NoSQL
	/// </summary>
	public static class Cache {

		#region Field
		// url/image
		public static readonly Dictionary<string, BitmapImage> Images;

		// 短縮URL/展開URL
		public static readonly Dictionary<string, string> ShortenUrl;

		// Status.Id/Status
		public static readonly Dictionary<string, Status> Statuses;

		public static readonly HashSet<string> Hyperlinks;
		public static readonly HashSet<string> ScreenNames;
		public static readonly HashSet<string> Hashtages;
		#endregion

		#region Constructor
		static Cache() {
			Images = new Dictionary<string, BitmapImage>();
			ShortenUrl = new Dictionary<string, string>();
			Statuses = new Dictionary<string, Status>();

			Hyperlinks = new HashSet<string>();
			ScreenNames = new HashSet<string>();
			Hashtages = new HashSet<string>();
		}
		#endregion

	}

}
