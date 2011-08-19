using System;
using System.Collections.Generic;
using System.Xml;
using Client.Components.ViewModel;
using Client.Library.OAuth;
using Client.Model.Twitter.Entities;

namespace Client.Model.Twitter.Api.Rest {

	public static class Favorites {

		/// <summary>
		/// Document: http://dev.twitter.com/doc/get/favorites
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
			string query = TwitterUtility.GetQuery(oAuthTwitter.Method.GET, ref postData, ApiSelector.Favorites, extension, options);
			string response = ModelUtility.DownloadContext(query);

			switch (extension) {
				case Format.Xml:
					var xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(response);
					XmlNodeList xmlNodes = xmlDoc.SelectNodes("//statuses/status");
					foreach (XmlNode node in xmlNodes) {
						yield return new Status(node, UserType.Others);
					}
					break;
				case Format.Atom:
				case Format.Json:
				case Format.Rss:
				default:
					throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Document: http://dev.twitter.com/doc/post/favorites/create/:id
		/// Supported formats: json, xml
		/// Supported request methods: POST
		/// Requires Authentication: true
		/// Rate Limited: false
		/// Required Parameters: id
		/// </summary>
		/// <param name="extension">xmlに対応</param>
		/// <param name="options">Documentを参照してください</param>
		/// <returns></returns>
		public static Status Create(Format extension, string id, params string[] options) {
			string query = TwitterUtility.GetQuery(ApiSelector.FavoritesCreate, extension, options);
			query = query.Replace("id.", id + ".");
			string response = ConfigurationViewModel.OAuth.oAuthWebRequest(oAuthTwitter.Method.POST, query, ModelUtility.GetOption(options));

			Status status = null;
			switch (extension) {
				case Format.Xml:
					var xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(response);
					XmlNode node = xmlDoc.SelectSingleNode("/status");
					status = new Status(node, UserType.Others);
					break;
				case Format.Json:
				default:
					throw new NotImplementedException();
			}
			return status;
		}

		/// <summary>
		/// Document: http://dev.twitter.com/doc/post/favorites/destroy/:id
		/// Supported formats: json, xml
		/// Supported request methods: POST, DELETE
		/// Requires Authentication: true
		/// Rate Limited: false
		/// Required Parameters: id
		/// </summary>
		/// <param name="extension">xmlに対応</param>
		/// <param name="options">Documentを参照してください</param>
		/// <returns></returns>
		public static Status Destroy(Format extension, string id, params string[] options) {
			string query = TwitterUtility.GetQuery(ApiSelector.FavoritesDestroy, extension, options);
			query = query.Replace("id.", id + ".");
			string response = ConfigurationViewModel.OAuth.oAuthWebRequest(oAuthTwitter.Method.POST, query, ModelUtility.GetOption(options));

			Status status = null;
			switch (extension) {
				case Format.Xml:
					var xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(response);
					XmlNode node = xmlDoc.SelectSingleNode("/status");
					status = new Status(node, UserType.Others);
					break;
				case Format.Json:
				default:
					throw new NotImplementedException();
			}
			return status;
		}

	}

}
