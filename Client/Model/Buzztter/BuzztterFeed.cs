using Client.Model.Twitter;
using Client.Model.Twitter.Api;
using System.Collections.Generic;

namespace Client.Model.Buzztter {

	/// <summary>
	/// http://buzztter.com/ja
	/// http://twitter.com/buzztter
	/// </summary>
	public static class BuzztterFeed {

		#region Field
		private static readonly string buzztterUrl;
		private static readonly string hotPrefix;
		#endregion

		#region Constructor
		static BuzztterFeed() {
			buzztterUrl = "http://buzztter.com/ja";
			hotPrefix = "HOT:";
		}
		#endregion

		#region Method
		/// <summary>
		/// Buzztterが配信するHOT群を取得します。
		/// </summary>
		/// <returns>Buzztterが配信するHOT群</returns>
		public static string[] GetHotWords() {
			List<string> list = new List<string>();

			int count = 0;
			foreach (var entry in Search.Execute(Format.Atom, "q=from:buzztter", "rpp=30")) {
				if (entry.Text.StartsWith(hotPrefix)) {
					list.Add(entry.Text.Split(' ')[1]);
					if (++count > 10) {
						break;
					}
				}
			}

			return list.ToArray();
		}

		/// <summary>
		/// Buzztterが配信するキーワード群を取得します。
		/// </summary>
		/// <returns>Buzztterが配信するキーワード群</returns>
		public static string[] GetFeed() {
			string[] words = null;

			string feed = null;
			foreach (var entry in Search.Execute(Format.Atom, "q=from:buzztter", "rpp=30")) {
				if (entry.Text.StartsWith(buzztterUrl)) {
					feed = entry.Text;
					break;
				}
			}
			if (feed != null) {
				words = feed.Remove(0, buzztterUrl.Length).Replace(" ", "").Split(',');
			}

			return words;
		}
		#endregion

	}

}
