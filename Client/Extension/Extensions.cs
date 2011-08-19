using System;
using System.Security;
using System.Text.RegularExpressions;

namespace Client.Extension {

	public static class Extensions {

		public static string[] SplitByLineOrComma(this string keyword) {
			string[] splited = null;

			if (keyword != null) {
				splited = keyword.Split(new string[] { "\r\n", "\r", "\n", "," }, StringSplitOptions.RemoveEmptyEntries);
			}

			return splited;
		}

		public static bool Relates(this string text, string value) {
			bool relates = text.IndexOf(value, StringComparison.OrdinalIgnoreCase) != -1;

			if (!relates) {
				string[] words = value.Split(' ');

				if (words.Length != 1) {
					int i;
					for (i = 0 ; i < words.Length ; i++) {
						if (!Regex.IsMatch(text, words[i], RegexOptions.IgnoreCase)) {
							break;
						}
					}
					relates = i == words.Length;
				}
			}

			return relates;
		}

	}

}
