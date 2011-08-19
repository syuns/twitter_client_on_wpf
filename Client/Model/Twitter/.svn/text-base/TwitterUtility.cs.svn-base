using System;
using System.Net;
using System.Text;
using System.Web;
using Client.Components.ViewModel;
using Client.Library.OAuth;

namespace Client.Model.Twitter {

	/// <summary>
	/// APIの種類
	/// </summary>
	public enum ApiSelector {
		DirectMesesages,
		Favorites,
		FavoritesCreate,
		FavoritesDestroy,
		StatusesShow,
		StatusesPublicTimeLine,
		StatusesUserTimeLine,
		StatusesHomeTimeline,
		StatusesMentions,
		StatusesRetweet,
		StatusesDestroy,
		StatusesUpdate,
		Search,
		StreamSample,
		StreamFilter,
		UserStreams,
		TrendsWoeid,
		UsersShow,
	}

	/// <summary>
	/// 拡張子の種類
	/// </summary>
	public enum Format {
		Atom,
		Json,
		Rss,
		Xml,
	}

	/// <summary>
	/// メモ：
	/// あるユーザのタイムラインをxml形式で取得する例は以下の通り。
	/// http://api.twitter.com/1/statuses/user_timeline.xml?screen_name={screen_name}&count={count}
	/// API: ?より前の部分
	/// オプション: ?より後の部分（{}は具体的な値で、引数を複数持つ場合は&で結合する）
	/// クエリ：API + "?" + オプション
	///
	/// TwitterAPI参考文書：
	/// http://apiwiki.twitter.com/w/page/22554679/Twitter-API-Documentation
	/// http://twitool-box.net/api-viewer/
	/// http://watcher.moe-nifty.com/memo/docs/twitterAPI50.txt
	/// </summary>
	public static class TwitterUtility {

		/// <summary>
		/// クエリ用APIを取得します。
		/// </summary>
		/// <param name="selector">APIの種類</param>
		/// <param name="extention">拡張の種類（個々のAPIが対応する拡張子は異なります）</param>
		/// <returns>selectorに対応したTwitterAPI</returns>
		public static string GetApi(ApiSelector selector, Format extention) {
			string api;
			string ext;

			switch (selector) {
				case ApiSelector.DirectMesesages:
					api = "http://api.twitter.com/1/direct_messages";
					break;
				case ApiSelector.Favorites:
					api = "http://api.twitter.com/1/favorites";
					break;
				case ApiSelector.FavoritesCreate:
					api = "http://api.twitter.com/1/favorites/create/id";
					break;
				case ApiSelector.FavoritesDestroy:
					api = "http://api.twitter.com/1/favorites/destroy/id";
					break;
				case ApiSelector.StatusesShow:
					api = "http://api.twitter.com/1/statuses/show/id";
					break;
				case ApiSelector.StatusesPublicTimeLine:
					api = "http://api.twitter.com/1/statuses/public_timeline";
					break;
				case ApiSelector.StatusesUserTimeLine:
					api = "http://api.twitter.com/1/statuses/user_timeline";
					break;
				case ApiSelector.StatusesHomeTimeline:
					api = "http://api.twitter.com/1/statuses/home_timeline";
					break;
				case ApiSelector.StatusesMentions:
					api = "http://api.twitter.com/1/statuses/mentions";
					break;
				case ApiSelector.StatusesRetweet:
					api = "http://api.twitter.com/1/statuses/retweet/id";
					break;
				case ApiSelector.StatusesDestroy:
					api = "http://api.twitter.com/1/statuses/destroy/id";
					break;
				case ApiSelector.StatusesUpdate:
					api = "http://api.twitter.com/1/statuses/update";
					break;
				case ApiSelector.Search:
					api = "http://search.twitter.com/search";
					break;
				case ApiSelector.StreamSample:
					api = "http://stream.twitter.com/1/statuses/sample";
					break;
				case ApiSelector.StreamFilter:
					api = "http://stream.twitter.com/1/statuses/filter";
					break;
				case ApiSelector.UserStreams:
					api = "https://userstream.twitter.com/2/user";
					break;
				case ApiSelector.TrendsWoeid:
					api = "http://api.twitter.com/1/trends/woeid";
					break;
				case ApiSelector.UsersShow:
					api = "http://api.twitter.com/1/users/show";
					break;
				default:
					api = string.Empty;
					break;
			}

			switch (extention) {
				case Format.Atom:
					ext = ".atom";
					break;
				case Format.Json:
					ext = ".json";
					break;
				case Format.Rss:
					ext = ".rss";
					break;
				case Format.Xml:
					ext = ".xml";
					break;
				default:
					ext = string.Empty;
					break;
			}

			return api + ext;
		}

		/// <summary>
		/// クエリ（API + オプション）を取得します。
		/// </summary>
		/// <param name="selector">API選択子</param>
		/// <param name="extention">拡張の種類（個々のAPIが対応する形式は異なります）</param>
		/// <param name="options">key=value形式のオプション項目</param>
		/// <returns>GetApi(selector, ext) + "?" + GetOption(options)と同等の値</returns>
		public static string GetQuery(ApiSelector selector, Format extension, params string[] options) {
			string option = ModelUtility.GetOption(options);
			string query = GetApi(selector, extension) + (option == string.Empty ? "" : "?" + option);
			return query;
		}

		/// <summary>
		/// 認証クエリを取得します。
		/// </summary>
		/// <param name="method"></param>
		/// <param name="postData"></param>
		/// <param name="selector"></param>
		/// <param name="extension"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static string GetQuery(oAuthTwitter.Method method, ref string postData, ApiSelector selector, Format extension, params string[] options) {
			string query = GetQuery(selector, extension, options);
			if (ConfigurationViewModel.OAuth != null) {
				ConfigurationViewModel.OAuth.ConvertToOAuthUrl(method, ref query, ref postData);
			}
			return query;
		}

	}

}
