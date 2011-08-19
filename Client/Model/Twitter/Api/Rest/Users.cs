using System;
using System.Xml;
using Client.Library.OAuth;
using Client.Model.Twitter.Entities;

namespace Client.Model.Twitter.Api.Rest {

	public static class Users {

		/// <summary>
		/// Document: http://dev.twitter.com/doc/get/users/show
		/// Supported formats: json, xml
		/// Supported request methods: GET
		/// Requires Authentication: false
		/// Rate Limited: true
		/// Required Parameters: user_id, screen_name
		/// </summary>
		/// <param name="extension">jsonに対応</param>
		/// <param name="options">Documentを参照してください</param>
		/// <returns></returns>
		public static User Show(Format extension, params string[] options) {
			string postData = string.Empty;
			string query = TwitterUtility.GetQuery(oAuthTwitter.Method.GET, ref postData, ApiSelector.UsersShow, extension, options);
			string response = ModelUtility.DownloadContext(query);

			User user;
			switch (extension) {
				case Format.Xml:
					var xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(response);
					XmlNode node = xmlDoc.SelectSingleNode("user");
					user = new User(node);
					break;
				case Format.Json:
				default:
					throw new NotImplementedException();
			}
			return user;
		}

	}

}
