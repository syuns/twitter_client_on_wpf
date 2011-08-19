using System;
using System.Net;
using System.Text;
using System.Web;

namespace Client.Model {

	public static class ModelUtility {

		/// <summary>
		/// options[0]&amp;options[1]...の形式に変換した文字列を返します。
		/// </summary>
		/// <param name="options">key=value形式のオプション</param>
		/// <returns>options[0]&amp;options[1]...の形式に変換した文字列</returns>
		public static string GetOption(params string[] options) {
			string option;

			if (options == null) {
				option = string.Empty;
			} else {
				var sb = new StringBuilder();
				for (int i = 0 ; i < options.Length ; i++) {
					try {
						int n = options[i].IndexOf('=');
						if (n != -1) {
							// =以降をURLエンコードする
							string kv = options[i].Substring(0, n) + "=" + HttpUtility.UrlEncode(options[i].Substring(n + 1));
							sb.Append((i == options.Length - 1) ? kv : kv + "&");
						}
					} catch {
						// do nothing
					}
				}
				option = sb.ToString();
			}

			return option;
		}

		public static string DownloadContext(string url) {
			string context = "";

			using (var webClient = new WebClient {
				Encoding = Encoding.UTF8
			}) {
				byte[] data = webClient.DownloadData(url);
				context = Encoding.UTF8.GetString(data);
			}

			return context;
		}

		public static void DownloadContextAsync(string url, DownloadDataCompletedEventHandler e) {
			using (var webClient = new WebClient {
				Encoding = Encoding.UTF8
			}) {
				webClient.DownloadDataAsync(new Uri(url));
				webClient.DownloadDataCompleted += e;
			}
		}

	}

}
