using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Client.Model.Twitter.Entities;

namespace Client.Model.Twitter.Api.Rest {

	public static class Trends {

		/// <summary>
		/// Document: http://dev.twitter.com/doc/get/trends/:woeid
		/// Supported formats: json, xml
		/// Supported request methods: GET
		/// Requires Authentication: false
		/// Rate Limited: true
		/// Required Parameters: woeid
		/// </summary>
		/// <param name="extension">jsonに対応</param>
		/// <param name="woeid">woeid(ex.1:世界,23424856:日本)</param>
		/// <returns></returns>
		public static MatchingTrends Woeid(Format extension, string woeid) {
			string postData = string.Empty;
			string query = TwitterUtility.GetQuery(Client.Library.OAuth.oAuthTwitter.Method.GET, ref postData, ApiSelector.TrendsWoeid, extension, null);
			query = query.Replace("woeid", woeid);

			MatchingTrends trends = null;
			switch (extension) {
				case Format.Json:
					string line = ModelUtility.DownloadContext(query);
					var serializer = new JavaScriptSerializer();
					var a = serializer.DeserializeObject(line);
					var b = a as object[];
					var trendHash = b[0] as Dictionary<string, object>;
					trends = new MatchingTrends(trendHash);
					break;
				case Format.Xml:
				default:
					throw new NotImplementedException();
			}
			return trends;
		}

	}

}
