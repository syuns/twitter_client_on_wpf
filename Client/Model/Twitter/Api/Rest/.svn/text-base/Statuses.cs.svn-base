using System;
using System.Collections.Generic;
using System.Xml;
using Client.Components.ViewModel;
using Client.Library.OAuth;
using Client.Model.Twitter.Entities;

namespace Client.Model.Twitter.Api.Rest {

	public static class Statuses {

		/// <summary>
		/// Document: http://dev.twitter.com/doc/get/statuses/show/:id
		/// Supported formats: json, xml
		/// Supported request methods: GET
		/// Requires Authentication: false
		/// Rate Limited: true
		/// Required Parameters: id
		/// </summary>
		/// <param name="extension">xmlに対応</param>
		/// <param name="options">Documentを参照してください</param>
		/// <returns></returns>
		public static Status Show(Format extension, string id, params string[] options) {
			string postData = string.Empty;
			string query = TwitterUtility.GetQuery(oAuthTwitter.Method.GET, ref postData, ApiSelector.StatusesShow, extension, options);
			query = query.Replace("id.", id + ".");
			string response = ModelUtility.DownloadContext(query);

			Status status = null;
			switch (extension) {
				case Format.Xml:
					var xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(response);
					XmlNode node = xmlDoc.SelectSingleNode("status");
					status = new Status(node, UserType.Others);
					break;
				case Format.Atom:
				case Format.Json:
				case Format.Rss:
				default:
					throw new NotImplementedException();
			}
			return status;
		}

		private static IEnumerable<Status> BaseTimeline(ApiSelector selector, Format extension, params string[] options) {
			string postData = string.Empty;
			string query = TwitterUtility.GetQuery(oAuthTwitter.Method.GET, ref postData, selector, extension, options);
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
		/// Document: http://dev.twitter.com/doc/get/statuses/user_timeline
		/// Supported formats: json, xml, rss, atom
		/// Supported request methods: GET
		/// Requires Authentication: false
		/// Rate Limited: true
		/// Required Parameters:
		/// </summary>
		/// <param name="extension">xmlに対応</param>
		/// <param name="options">Documentを参照してください</param>
		/// <returns></returns>
		public static IEnumerable<Status> UserTimeline(Format extension, params string[] options) {
			foreach (Status status in BaseTimeline(ApiSelector.StatusesUserTimeLine, extension, options)) {
				yield return status;
			}
		}

		/// <summary>
		/// Document: http://dev.twitter.com/doc/get/statuses/public_timeline
		/// Supported formats: json, xml, rss, atom
		/// Supported request methods: GET
		/// Requires Authentication: false
		/// Rate Limited: true
		/// Required Parameters:
		/// </summary>
		/// <param name="extension">xmlに対応</param>
		/// <param name="options">Documentを参照してください</param>
		/// <returns></returns>
		public static IEnumerable<Status> PublicTimeline(Format extension, params string[] options) {
			foreach (Status status in BaseTimeline(ApiSelector.StatusesPublicTimeLine, extension, options)) {
				yield return status;
			}
		}

		/// <summary>
		/// Document: http://dev.twitter.com/doc/get/statuses/home_timeline
		/// Supported formats: json, xml, rss, atom
		/// Supported request methods: GET
		/// Requires Authentication: true
		/// Rate Limited: true
		/// Required Parameters:
		/// </summary>
		/// <param name="extension">xmlに対応</param>
		/// <param name="options">Documentを参照してください</param>
		/// <returns></returns>
		public static IEnumerable<Status> HomeTimeline(Format extension, params string[] options) {
			foreach (Status status in BaseTimeline(ApiSelector.StatusesHomeTimeline, extension, options)) {
				yield return status;
			}
		}

		/// <summary>
		/// Document: http://dev.twitter.com/doc/get/statuses/mentions
		/// Supported formats: json, xml, rss, atom
		/// Supported request methods: GET
		/// Requires Authentication: true
		/// Rate Limited: true
		/// Required Parameters:
		/// </summary>
		/// <param name="extension">xmlに対応</param>
		/// <param name="options">Documentを参照してください</param>
		/// <returns></returns>
		public static IEnumerable<Status> Mentions(Format extension, params string[] options) {
			string postData = "";
			string query = TwitterUtility.GetQuery(oAuthTwitter.Method.GET, ref postData, ApiSelector.StatusesMentions, extension);
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
		/// Document: http://dev.twitter.com/doc/post/statuses/retweet/:id
		/// Supported formats: xml, json
		/// Supported request methods: POST
		/// Requires Authentication: true
		/// Rate Limited: false
		/// Required Parameters: id
		/// </summary>
		/// <param name="extension">xmlに対応</param>
		/// <param name="id">Status.Id</param>
		/// <param name="options">Documentを参照してください</param>
		/// <returns></returns>
		public static Status Retweet(Format extension, string id, params string[] options) {
			string query = TwitterUtility.GetQuery(ApiSelector.StatusesRetweet, extension, options);
			query = query.Replace("id.", id + ".");
			string response = ConfigurationViewModel.OAuth.oAuthWebRequest(oAuthTwitter.Method.POST, query, ModelUtility.GetOption(options));

			Status status = null;
			switch (extension) {
				case Format.Xml:
					var xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(response);
					XmlNode node = xmlDoc.SelectSingleNode("status");
					status = new Status(node, UserType.Others);
					break;
				case Format.Json:
				default:
					throw new NotImplementedException();
			}
			return status;
		}

		/// <summary>
		/// Document: http://dev.twitter.com/doc/post/statuses/destroy/:id
		/// Supported formats: xml, json
		/// Supported request methods: POST
		/// Requires Authentication: true
		/// Rate Limited: false
		/// Required Parameters: id
		/// </summary>
		/// <param name="extensio"></param>
		/// <param name="id"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static Status Destroy(Format extension, string id, params string[] options) {
			string query = TwitterUtility.GetQuery(ApiSelector.StatusesDestroy, extension, options);
			query = query.Replace("id.", id + ".");
			string response = ConfigurationViewModel.OAuth.oAuthWebRequest(oAuthTwitter.Method.POST, query, ModelUtility.GetOption(options));

			Status status = null;
			switch (extension) {
				case Format.Xml:
					var xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(response);
					XmlNode node = xmlDoc.SelectSingleNode("status");
					status = new Status(node, UserType.Others);
					break;
				case Format.Json:
				default:
					throw new NotImplementedException();
			}
			return status;
		}

		/// <summary>
		/// Document: http://dev.twitter.com/doc/post/statuses/update
		/// Supported formats: xml, json
		/// Supported request methods: POST
		/// Requires Authentication: true
		/// Rate Limited: false
		/// Required Parameters: status
		/// </summary>
		/// <param name="extension"></param>
		/// <param name="tweet"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static Status Update(Format extension, string tweet, params string[] options) {
			string postData = "status=" + tweet;
			string query = TwitterUtility.GetQuery(oAuthTwitter.Method.POST, ref postData, ApiSelector.StatusesUpdate, extension, options);
			string response = oAuthTwitter.WebRequest(oAuthTwitter.Method.POST, query, postData);

			Status status = null;
			switch (extension) {
				case Format.Xml:
					var xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(response);
					XmlNode node = xmlDoc.SelectSingleNode("status");
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
