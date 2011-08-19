using System.Xml;
using Client.Model.Twitter;
using Client.Model.Yahoo.Entities;

namespace Client.Model.Yahoo.Api.Rest {

	public static class MAService {

		/// <summary>
		/// 日本語形態素解析
		/// http://developer.yahoo.co.jp/webapi/jlp/ma/v1/parse.html
		/// </summary>
		public static ResultSet Parse(string appid, string sentence, string results, params string[] options) {
			const string url = "http://jlp.yahooapis.jp/MAService/V1/parse?appid={0}&sentence={1}&results={2}";
			string option = ModelUtility.GetOption(options);
			string query = string.Format(url, appid, sentence, results);
			if (!string.IsNullOrEmpty(option)) {
				query = url + "&" + option;
			}

			string response = ModelUtility.DownloadContext(query);

			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(response);
			var ns = new XmlNamespaceManager(xmlDoc.NameTable);
			ns.AddNamespace("y", "urn:yahoo:jp:jlp");
			return new ResultSet(xmlDoc.SelectSingleNode("/y:ResultSet", ns), ns);
		}

	}

}
