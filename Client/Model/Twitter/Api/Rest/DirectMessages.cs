using System;
using System.Collections.Generic;
using System.Xml;
using Client.Library.OAuth;
using Client.Model.Twitter.Entities;
using Client.Components.ViewModel;

namespace Client.Model.Twitter.Api.Rest {

	public static class DirectMessages {

		/// <summary>
		/// Document: http://dev.twitter.com/doc/get/direct_messages
		/// Supported formats: json, xml, rss, atom
		/// Supported request methods: GET
		/// Requires Authentication: true
		/// Rate Limited: true
		/// Required Parameters:
		/// </summary>
		/// <param name="extension">xmlに対応</param>
		/// <param name="options">Documentを参照してください</param>
		/// <returns></returns>
		public static IEnumerable<Status> Execute(Format extension, params string[] options) {
			string postData = string.Empty;
			string query = TwitterUtility.GetQuery(oAuthTwitter.Method.GET, ref postData, ApiSelector.DirectMesesages, extension, options);
			string response = ModelUtility.DownloadContext(query);

			switch (extension) {
				case Format.Xml:
					var xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(response);
					XmlNodeList xmlNodes = xmlDoc.SelectNodes("//direct-messages/direct_message");
					foreach (XmlNode node in xmlNodes) {
						yield return new Status(node, UserType.DirectMessage);
					}
					break;
				case Format.Atom:
				case Format.Json:
				case Format.Rss:
				default:
					throw new NotImplementedException();
			}
		}

	}

}
