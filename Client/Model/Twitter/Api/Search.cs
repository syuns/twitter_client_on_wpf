using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using Client.Model.Twitter.Entities;

namespace Client.Model.Twitter.Api {

	public static class Search {

		private static string FormatBase(string insertWord, params string[] keywords) {
			var sb = new StringBuilder();

			int n = keywords.Length;
			for (int i = 0 ; i < n ; i++) {
				string or = (i == n - 1) ? string.Empty : insertWord;
				sb.Append(keywords[i].Replace(" ", "") + or);
			}

			return sb.ToString();
		}

		/// <summary>
		/// qオプション用にフォーマットします。
		/// </summary>
		/// <param name="keywords">検索キーワード</param>
		/// <returns>keywordsをOR結合した文字列</returns>
		public static string FormatQ(params string[] keywords) {
			return FormatBase(" OR ", keywords);
		}

		public static string FormatQByAnd(params string[] keywords) {
			return FormatBase(" ", keywords);
		}

		/// <summary>
		/// qオプションの引数を取得します。
		/// </summary>
		/// <param name="options">qオプションを含むオプション群</param>
		/// <returns>qオプションの引数、見つからない場合はnull</returns>
		public static string GetQWord(params string[] options) {
			string qWord = null;

			foreach (string option in options) {
				int index = option.IndexOf("q=");
				if (index != -1) {
					qWord = option.Substring(index + 2);
					break;
				}
			}

			return qWord;
		}

		/// <summary>
		/// Document: http://dev.twitter.com/doc/get/search
		/// Supported formats: json, atom
		/// Supported request methods: GET
		/// Requires Authentication: false
		/// Rate Limited: true
		/// Required Parameters: q
		/// </summary>
		/// <param name="extension">atomに対応</param>
		/// <param name="options">Documentを参照してください</param>
		/// <returns></returns>
		public static IEnumerable<Status> Execute(Format extension, params string[] options) {
			string query = TwitterUtility.GetQuery(ApiSelector.Search, extension, options);

			switch (extension) {
				case Format.Atom:
					using (XmlReader reader = XmlReader.Create(query)) {
						SyndicationFeed feed = SyndicationFeed.Load(reader);
						foreach (var e in feed.Items) {
							yield return new Status(e);
						}
					}
					break;
				case Format.Json:
					// まだ途中
					string context = ModelUtility.DownloadContext(query);
					var serializer = new JavaScriptSerializer();
					var results = serializer.Deserialize<Dictionary<string, object>>(context)["results"];
					foreach (Dictionary<string, object> status in results as ArrayList) {
						yield return new Status(status, CreatedAtFormatType.Search);
					}
					break;
				default:
					throw new NotImplementedException();
			}
		}

	}

}
