using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Client.Library.OAuth;
using Client.Model.Twitter.Entities;
using System.Collections;

namespace Client.Model.Twitter.Api {

	/// <summary>
	/// http://dev.twitter.com/pages/streaming_api
	/// http://dev.twitter.com/pages/streaming_api_methods
	/// </summary>
	public static class Streaming {

		#region Field
		private static HttpWebRequest request;
		#endregion

		#region Constructor
		static Streaming() {
			request = null;
		}
		#endregion

		#region Method
		private static void Stop() {
			if (request != null) {
				request.Abort();
				request = null;
			}
		}

		/// <summary>
		/// StreamFilterのfollowまたはtrackオプション用にフォーマットします。
		/// </summary>
		/// <param name="keywords">キーワード群</param>
		/// <returns>keywordsをカンマ区切りで結合した文字列</returns>
		public static string FormatFollowOrTrack(params string[] keywords) {
			var sb = new StringBuilder();

			int n = keywords.Length;
			for (int i = 0 ; i < n ; i++) {
				string comma = (i == n - 1) ? string.Empty : ",";
				sb.Append(keywords[i] + comma);
			}

			return sb.ToString();
		}

		/// <summary>
		/// StreamSampleで取得したデータを各デリゲートに委譲します。
		/// Document: http://dev.twitter.com/pages/streaming_api_methods#statuses-sample
		/// Supported formats: json
		/// Supported request methods: GET
		/// Requires Authentication: false
		/// Rate Limited: false
		/// Required Parameters:
		/// </summary>
		/// <param name="user">認証ユーザ</param>
		/// <param name="onAdded">追加デリゲート：Statusが生成されるときに発生します。</param>
		/// <param name="onDeleted">削除デリゲート：Statusが削除されるときに発生します。</param>
		/// <param name="condition">実行条件デリゲート：データを取得するときに発生します。/param>
		public static void RunStreamSample(Action<Status> onAdded, Action<Status> onDeleted, Func<bool> condition) {
			if (request != null) {
				Stop();
				return;
			}

			string postData = string.Empty;
			string query = TwitterUtility.GetQuery(oAuthTwitter.Method.GET, ref postData, ApiSelector.StreamSample, Format.Json, null);

			try {
				request = WebRequest.Create(query) as HttpWebRequest;
				request.ServicePoint.Expect100Continue = false;
				using (var response = request.GetResponse() as HttpWebResponse)
				using (var stream = response.GetResponseStream())
				using (var reader = new StreamReader(stream)) {
					while (condition()) {
						string line = reader.ReadLine();
						if (line == null)
							break;
						if (line == "")
							continue;

						var serializer = new JavaScriptSerializer();
						var status = serializer.Deserialize<Dictionary<string, object>>(line);

						if (status.ContainsKey("text")) {
							onAdded(new Status(status, CreatedAtFormatType.Streaming));
						} else if (status.ContainsKey("delete")) {
							var a = status["delete"] as Dictionary<string, object>;
							var b = a["status"] as Dictionary<string, object>;
							onDeleted(new Status(b, CreatedAtFormatType.Streaming));
						} else {
							Console.WriteLine(line);
						}
					}
				}
			} finally {
				Stop();
			}
		}

		/// <summary>
		/// StreamFilterで取得したデータを各デリゲートに委譲します。
		/// Document: http://dev.twitter.com/pages/streaming_api_methods#statuses-filter
		/// Supported formats: json
		/// Supported request methods: GET
		/// Requires Authentication: false
		/// Rate Limited: false
		/// Required Parameters:
		/// </summary>
		/// <param name="user">認証ユーザ</param>
		/// <param name="onAdded">追加デリゲート：Statusが生成されるときに発生します。</param>
		/// <param name="onDeleted">削除デリゲート：Statusが削除されるときに発生します。</param>
		/// <param name="condition">実行条件デリゲート：データを取得するときに発生します。</param>
		/// <param name="post">Twitter API Docを参照してください</param>
		public static void RunStreamFilter(Action<Status> onAdded, Action<Status> onDeleted, Func<bool> condition, string option) {
			if (request != null) {
				Stop();
				return;
			}

			string postData = option;
			string query = TwitterUtility.GetQuery(oAuthTwitter.Method.POST, ref postData, ApiSelector.StreamFilter, Format.Json, null);
			byte[] bpostData = Encoding.UTF8.GetBytes(postData);

			try {
				request = WebRequest.Create(query) as HttpWebRequest;
				request.ServicePoint.Expect100Continue = false;
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = bpostData.Length;
				using (Stream requestStream = request.GetRequestStream()) {
					requestStream.Write(bpostData, 0, bpostData.Length);
					using (var response = request.GetResponse() as HttpWebResponse)
					using (var stream = response.GetResponseStream())
					using (var reader = new StreamReader(stream)) {
						while (condition()) {
							string line = reader.ReadLine();
							if (line == null)
								break;
							if (line == "")
								continue;

							var serializer = new JavaScriptSerializer();
							var status = serializer.Deserialize<Dictionary<string, object>>(line);

							if (status.ContainsKey("text")) {
								onAdded(new Status(status, CreatedAtFormatType.Streaming));
							} else if (status.ContainsKey("delete")) {
								var a = status["delete"] as Dictionary<string, object>;
								var b = a["status"] as Dictionary<string, object>;
								onDeleted(new Status(b, CreatedAtFormatType.Streaming));
							} else {
								Console.WriteLine(line);
							}
						}
					}
				}
			} finally {
				Stop();
			}
		}

		/// <summary>
		/// UserStreamsで取得したデータを各デリゲートに委譲します。（未実装部分多し）
		/// Document: http://dev.twitter.com/pages/user_streams_suggestions
		/// Supported formats: json
		/// Supported request methods: POST
		/// Requires Authentication: true
		/// Rate Limited: false
		/// Required Parameters:
		/// </summary>
		/// <param name="onAdded">追加デリゲート：Statusが生成されるときに発生します。</param>
		/// <param name="onDeleted">削除デリゲート：Statusが削除されるときに発生します。</param>
		/// <param name="condition">実行条件デリゲート：データを取得するときに発生します。</param>
		/// <param name="post">Twitter API Docを参照してください</param>
		public static void RunUserStreams(Action<Status> onAdded, Action<Status> onDeleted, Action<Status> onFavorited, Action<Status> onUnFavorited, Func<bool> condition, string option, List<string> friends) {
			if (request != null) {
				Stop();
				return;
			}

			string postData = option == null ? string.Empty : option;
			string query = TwitterUtility.GetQuery(oAuthTwitter.Method.POST, ref postData, ApiSelector.UserStreams, Format.Json, null);
			byte[] bpostData = Encoding.UTF8.GetBytes(postData);

			try {
				request = WebRequest.Create(query) as HttpWebRequest;
				request.ServicePoint.Expect100Continue = false;
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = bpostData.Length;
				using (Stream requestStream = request.GetRequestStream()) {
					requestStream.Write(bpostData, 0, bpostData.Length);
					using (var response = request.GetResponse() as HttpWebResponse)
					using (var stream = response.GetResponseStream())
					using (var reader = new StreamReader(stream)) {
						while (condition()) {
							string line = reader.ReadLine();
							if (line == null)
								break;
							if (line == "")
								continue;

							var serializer = new JavaScriptSerializer();
							var source = serializer.Deserialize<Dictionary<string, object>>(line);

							if (source.ContainsKey("text")) {
								onAdded(new Status(source, CreatedAtFormatType.Streaming));
							} else if (source.ContainsKey("delete")) {
								var a = source["delete"] as Dictionary<string, object>;
								var b = a["status"] as Dictionary<string, object>;
								onDeleted(new Status(b, CreatedAtFormatType.Streaming));
							} else if (source.ContainsKey("friends")) {
								var ids = source["friends"] as ArrayList;
								foreach (object id in ids) {
									string sid = id.ToString();
									if (sid != null) {
										friends.Add(sid);
									}
								}
							} else if (source.ContainsKey("event")) {
								string eventName = source["event"] as string;
								if (eventName == "favorite") {
									var status = source["target_object"] as Dictionary<string, object>;
									var entry = new Status(status, CreatedAtFormatType.Streaming);
									entry.Favorited = true;
									onFavorited(entry);
								} else if (eventName == "unfavorite") {
									var status = source["target_object"] as Dictionary<string, object>;
									var entry = new Status(status, CreatedAtFormatType.Streaming);
									entry.Favorited = false;
									onUnFavorited(entry);
								}
							} else {
								Console.WriteLine(line);
								Console.WriteLine(source);
							}
						}
					}
				}
			} finally {
				Stop();
			}
		}
		#endregion

	}

}
