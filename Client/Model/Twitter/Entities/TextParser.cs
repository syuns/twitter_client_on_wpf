using System.Collections.Generic;
using System.Text.RegularExpressions;
using Client.Library;

namespace Client.Model.Twitter.Entities {

	public enum StatusTextType {
		Hashtag,
		Normal,
		NormalHyperLink,
		ThumbHyperLink,
		Username,
	}

	public struct StatusText {

		public StatusTextType Type {
			get;
			set;
		}

		public string Text {
			get;
			set;
		}

	}

	public class TextParser {

		#region Field
		public static readonly string urlPattern;
		private static readonly string usernamePattern;
		private static readonly string hashtagPattern;
		private static readonly string unionPattern;
		// 置換前URL - 置換後URL
		private static readonly Dictionary<string, string> thumburlPatterns;
		// text - コンパイル済み正規表現
		public static readonly Dictionary<string, Regex> textRegexDictionary;
		// 置換前URL - コンパイル済み正規表現
		public static readonly Dictionary<string, Regex> thumbRegexDictionary;
		#endregion

		#region Property
		public string Text {
			get;
			set;
		}

		public StatusText[] ParsedText {
			get;
			set;
		}
		#endregion

		#region Constructor
		static TextParser() {
			urlPattern = @"(https?://[-_.!~*'a-zA-Z0-9;/?:@&=+$,%#]+)";
			usernamePattern = @"(@[a-zA-Z0-9_]+)";
			hashtagPattern = @"(#[a-zA-Z0-9_]+)";
			unionPattern = urlPattern + "|" + usernamePattern + "|" + hashtagPattern;

			textRegexDictionary = new Dictionary<string, Regex>();
			textRegexDictionary.Add(urlPattern, new Regex(urlPattern, RegexOptions.Compiled));
			textRegexDictionary.Add(usernamePattern, new Regex(usernamePattern, RegexOptions.Compiled));
			textRegexDictionary.Add(hashtagPattern, new Regex(hashtagPattern, RegexOptions.Compiled));
			textRegexDictionary.Add(unionPattern, new Regex(unionPattern, RegexOptions.Compiled));

			thumburlPatterns = new Dictionary<string, string>();
			// image
			thumburlPatterns.Add(@"http://(.+)[.](jpe?g|png|gif)", @"http://$1.$2");
			// Twitpic(http://twitpic.com/)
			thumburlPatterns.Add(@"http://twitpic[.]com/(\w+)(/full)?", @"http://twitpic.com/show/thumb/$1");
			// 携帯百景(http://movapic.com/)
			thumburlPatterns.Add(@"http://movapic[.]com/pic/(\w+)/?", @"http://image.movapic.com/pic/t_$1.jpeg");
			// はてなフォトライフ(http://f.hatena.ne.jp/)
			thumburlPatterns.Add(@"http://f[.]hatena[.]ne[.]jp/(.)(\w+)/(.{8})(\w+)/?", @"http://img.f.hatena.ne.jp/images/fotolife/$1/$1$2/$3/$3$4_120.jpg");
			// Mobypicture(http://www.mobypicture.com/)
			thumburlPatterns.Add(@"http://moby[.]to/(\w+)/?", @"http://moby.to/$1:square");
			// yFrog(http://yfrog.com/)
			thumburlPatterns.Add(@"http://yfrog[.]com/(\w+)/?", @"http://yfrog.com/$1.th.jpg");
			// PhotoShare 短縮URL(http://www.bcphotoshare.com/)
			thumburlPatterns.Add(@"http://bctiny[.]com/p(\w+)/?", @"http://images.bcphotoshare.com/storages/$1/thumbnail.jpg");
			// PhotoShare
			thumburlPatterns.Add(@"http://www[.]bcphotoshare[.]com/photos/\w+/(\w+)/?", @"http://images.bcphotoshare.com/storages/$1/thumbnail.jpg");
			// img.ly(http://img.ly/)
			thumburlPatterns.Add(@"http://img[.]ly/(\w)/?", @"http://img.ly/show/thumb/$1");
			// Twitgoo(http://twitgoo.com/)
			thumburlPatterns.Add(@"http://twitgoo[.]com/(\w+)/?", @"http://twitgoo.com/show/thumb/$1");
			// YouTube 短縮(http://www.youtube.com/)
			thumburlPatterns.Add(@"http://youtu[.]be/([\w\-]+)/?", @"http://i.ytimg.com/vi/$1/default.jpg");
			// YouTube
			thumburlPatterns.Add(@"http://www[.]youtube[.]com/watch(\?|#!).*v=([\w\-]+)&?.*", @"http://i.ytimg.com/vi/$2/default.jpg");
			// Plixi(http://plixi.com/)
			thumburlPatterns.Add(@"(http://plixi.com/p/\w+|http://tweetphoto.com/\w+)", @"http://api.plixi.com/api/TPAPI.svc/imagefromurl?size=thumbnail&url=$1");
			// Ow.ly(http://ow.ly/url/shorten-url)
			thumburlPatterns.Add(@"http://ow.ly/i/(\w+)/?", @"http://static.ow.ly/photos/thumb/$1.jpg");
			// Instagram
			thumburlPatterns.Add(@"http://instagr.am/p/([\w-]+)/?", @"http://instagr.am/p/$1/media/?size=t");

			thumbRegexDictionary = new Dictionary<string, Regex>();
			foreach (string key in thumburlPatterns.Keys) {
				thumbRegexDictionary.Add(key, new Regex(key, RegexOptions.Compiled));
			}
		}

		public TextParser(string text) {
			Text = text;
			ParsedText = Parse(Text);
		}
		#endregion

		#region Method
		public static string GetThumbUrl(string linkurl) {
			string thumbUrl = null;

			foreach (string key in thumbRegexDictionary.Keys) {
				if (thumbRegexDictionary[key].IsMatch(linkurl)) {
					if (key == @"http://bctiny.com/p(\w+)/?") {
						thumbUrl = thumbRegexDictionary[key].Replace(linkurl, (m) => {
							string base36 = RadixConvert.ToInt32(m.Result("$1"), 36).ToString();
							return thumburlPatterns[key].Replace("$1", base36);
						});
					} else {
						thumbUrl = thumbRegexDictionary[key].Replace(linkurl, thumburlPatterns[key]);
					}
					break;
				}
			}

			return thumbUrl;
		}

		private StatusText[] Parse(string text) {
			var parsedText = new List<StatusText>();

			foreach (var word in textRegexDictionary[unionPattern].Split(text)) {
				StatusTextType type;

				if (textRegexDictionary[urlPattern].IsMatch(word)) {
					Cache.Hyperlinks.Add(word);
					if (GetThumbUrl(word) == null) {
						type = StatusTextType.NormalHyperLink;
					} else {
						type = StatusTextType.ThumbHyperLink;
					}
				} else if (textRegexDictionary[usernamePattern].IsMatch(word)) {
					Cache.ScreenNames.Add(word);
					type = StatusTextType.Username;
				} else if (textRegexDictionary[hashtagPattern].IsMatch(word)) {
					Cache.Hashtages.Add(word);
					type = StatusTextType.Hashtag;
				} else {
					type = StatusTextType.Normal;
				}

				parsedText.Add(new StatusText {
					Type = type,
					Text = word,
				});
			}

			return parsedText.ToArray();
		}
		#endregion

	}

}
