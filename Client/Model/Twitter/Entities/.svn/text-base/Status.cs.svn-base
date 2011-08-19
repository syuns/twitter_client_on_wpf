using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using Client.Extension;
using Client.Model.Twitter.Api.Rest;
using GalaSoft.MvvmLight;

namespace Client.Model.Twitter.Entities {

	public enum UserType {
		DirectMessage,
		Others,
	}

	public enum CreatedAtFormatType {
		Search,
		Streaming,
	}

	/// <summary>
	/// エントリを表します。
	/// </summary>
	public class Status : ViewModelBase {

		#region Field
		private static readonly string dateFormat;
		private static readonly int tweetMaxLength;

		private HashSet<string> thumburls = new HashSet<string>();
		private string id;
		private string text;
		private TextParser textParser;
		private bool shownTree;
		#endregion

		#region Property
		/// <summary>
		/// つぶやきの文字数上限を取得します。
		/// </summary>
		public static int TweetMaxLength {
			get {
				return tweetMaxLength;
			}
		}

		/// <summary>
		/// ユーザを取得します。
		/// </summary>
		public User User {
			get;
			private set;
		}

		/// <summary>
		/// 投稿日時を取得します。
		/// </summary>
		public string CreatedAt {
			get;
			private set;
		}

		/// <summary>
		/// 投稿IDを取得します。
		/// </summary>
		public string Id {
			get {
				return id;
			}
			private set {
				string[] splited = value.Split(':');
				if (splited.Length == 3) {
					id = splited[2];
				} else {
					id = value;
				}
				if (!Cache.Statuses.ContainsKey(id)) {
					Cache.Statuses.Add(id, this);
				}
				this.RaisePropertyChanged("Id");
			}
		}

		/// <summary>
		/// クライアント(例：web, twipple)を取得します。
		/// </summary>
		public string Source {
			get;
			private set;
		}

		/// <summary>
		/// クライアントのURLを取得します。
		/// </summary>
		public string SourceUrl {
			get;
			private set;
		}

		/// <summary>
		/// 発言を取得します。
		/// </summary>
		public string Text {
			get {
				return text;
			}
			private set {
				text =
					value.
					Replace("nbsp;", " ").
					Replace("&lt;", "<").
					Replace("&gt;", ">").
					Replace("&amp;", "&").
					Replace("&quot;", "\"");
			}
		}

		/// <summary>
		/// Userが付けたお気に入りを取得または設定します。
		/// </summary>
		public bool Favorited {
			get;
			set;
		}

		/// <summary>
		/// 返信先のStatus.Idを取得します。
		/// </summary>
		public string InReplyToStatusId {
			get;
			private set;
		}

		/// <summary>
		/// Textを意味分割したTextParserを取得します。
		/// </summary>
		public TextParser TextParser {
			get {
				return textParser ??
					(textParser = new TextParser(Text));
			}
		}

		/// <summary>
		/// 整形済みの発言を取得します。
		/// </summary>
		public StackPanel TextContainer {
			get {
				return ShownTree ?
					GetTree() : GetTextConteiner();
			}
		}

		public bool ShownTree {
			get {
				return shownTree;
			}
			set {
				shownTree = value;
				this.RaisePropertyChanged("ShownTree");
				this.RaisePropertyChanged("TextContainer");
			}
		}
		#endregion

		#region Constructor & Destructor
		static Status() {
			dateFormat = "yyyy/MM/dd HH:mm:ss";
			tweetMaxLength = 140;
		}

		public Status(Dictionary<string, object> status, CreatedAtFormatType formatType) {
			if (status.ContainsKey("user")) {
				var user = status["user"] as Dictionary<string, object>;
				if (user != null) {
					User = new User(user);
				}
			}
			if (status.ContainsKey("created_at")) {
				string createdAtFormt = null;
				if (formatType == CreatedAtFormatType.Search) {
					createdAtFormt = "ddd',' dd MMM yyyy HH':'mm':'ss zzzz";
				} else if (formatType == CreatedAtFormatType.Streaming) {
					createdAtFormt = "ddd MMM dd HH':'mm':'ss zzzz yyyy";
				}

				CreatedAt = DateTime.ParseExact(
					status["created_at"] as string,
					createdAtFormt,
					System.Globalization.DateTimeFormatInfo.InvariantInfo,
					System.Globalization.DateTimeStyles.None
				).ToString(dateFormat);
			}
			if (status.ContainsKey("id_str")) {
				Id = status["id_str"] as string;
			}
			if (status.ContainsKey("source")) {
				Source = FormatSource(status["source"] as string);
			}
			if (status.ContainsKey("text")) {
				Text = status["text"] as string;
			}
			if (status.ContainsKey("in_reply_to_status_id")) {
				var id = status["in_reply_to_status_id"];
				if (id != null) {
					InReplyToStatusId = id.ToString();
				}
			}
		}

		public Status(XmlNode node, UserType userType) {
			string localUserName = null;
			if (userType == UserType.DirectMessage) {
				localUserName = "sender";
			} else if (userType == UserType.Others) {
				localUserName = "user";
			}

			User = new User(node.SelectSingleNode(localUserName));
			if (node["created_at"] != null) {
				CreatedAt = DateTime.ParseExact(
					node["created_at"].InnerText,
					"ddd MMM dd HH':'mm':'ss zzzz yyyy",
					System.Globalization.DateTimeFormatInfo.InvariantInfo,
					System.Globalization.DateTimeStyles.None
				).ToString(dateFormat);
			}
			if (node["id"] != null) {
				Id = node["id"].InnerText;
			}
			if (node["source"] != null) {
				Source = FormatSource(node["source"].InnerText);
			}
			if (node["text"] != null) {
				Text = node["text"].InnerText;
			}
			if (node["favorited"] != null) {
				Favorited = node["favorited"].InnerText == "true";
			}
			if (node["in_reply_to_status_id"] != null) {
				InReplyToStatusId = node["in_reply_to_status_id"].InnerText;
			}
		}

		public Status(SyndicationItem item) {
			string[] names = item.Authors[0].Name.Replace(" ", "").Replace('(', '/').TrimEnd(')').Split('/');
			User = new User {
				ScreenName = names[0],
				Name = names[1],
				ProfileImageUrl = item.Links[1].Uri.ToString(),
			};

			CreatedAt = item.PublishDate.LocalDateTime.ToString(dateFormat);
			Id = item.Id;
			Source = FormatSource(item.ElementExtensions.ReadElementExtensions<string>("source", "http://api.twitter.com/")[0]);
			Text = item.Title.Text;
		}
		#endregion

		#region Method
		public override string ToString() {
			return User.Name + " : " + Text;
		}

		public override bool Equals(object other) {
			bool isEquals = false;

			if (other != null) {
				var s = other as Status;
				if (s != null) {
					isEquals = this.Id == s.Id;
				}
			}

			return isEquals;
		}

		public override int GetHashCode() {
			int hashCode = 0;

			if (Id != null) {
				hashCode = Id.GetHashCode();
			}

			return hashCode;
		}

		private string FormatSource(string source) {
			string formated = source;
			const string innerTextPattern = @">([^<]*)<";
			const string hrefPettern = @"href=""([^""]*)""";

			try {
				if (Regex.IsMatch(source, innerTextPattern)) {
					formated = Regex.Match(source, innerTextPattern).Result("$1");
					if (Regex.IsMatch(source, hrefPettern)) {
						SourceUrl = Regex.Match(source, hrefPettern).Result("$1");
					}
				}
			} catch {
				// do nothing
			}

			return formated;
		}

		private void SetThumb(string thumbUrl, Image image, int count) {
			if (Cache.Images.ContainsKey(thumbUrl)) {
				image.Source = Cache.Images[thumbUrl];
			} else {
				var thumb = new BitmapImage();
				thumb.BeginInit();
				thumb.CacheOption = BitmapCacheOption.OnDemand;
				thumb.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
				thumb.UriSource = new Uri(thumbUrl);
				thumb.EndInit();

				image.Source = thumb;

				thumb.DownloadCompleted += (s, e) => {
					if (!Cache.Images.ContainsKey(thumbUrl)) {
						Cache.Images.Add(thumbUrl, thumb);
						thumburls.Add(thumbUrl);
					}
				};

				thumb.DownloadFailed += (s, e) => {
					if (++count <= 5) {
						SetThumb(thumbUrl, image, count);
					}
				};
			}
		}

		private Hyperlink GetThumbHyperlink(string thumbUrl, string linkUrl, int width, int height) {
			var hyperlink = new Hyperlink();

			var image = new Image {
				Width = width,
				Height = height,
				Margin = new Thickness(3, 0, 0, 0),
				HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
			};
			SetThumb(thumbUrl, image, 0);
			hyperlink.Inlines.Add(image);
			hyperlink.Click += (s, e) => Process.Start(linkUrl);
			hyperlink.ToolTip = linkUrl;

			return hyperlink;
		}

		private StackPanel GetTextConteiner() {
			var textContainer = new StackPanel();

			var textblockDetails = new TextBlock {
				FontSize = 10,
				Foreground = new SolidColorBrush(Colors.Gray),
			};
			var textblockPlain = new TextBlock {
				TextWrapping = TextWrapping.Wrap,
			};
			var imageContainer = new StackPanel {
				Orientation = Orientation.Horizontal,
			};

			textContainer.Children.Add(textblockDetails);
			textContainer.Children.Add(textblockPlain);
			textContainer.Children.Add(imageContainer);

			// textblockDetails
			textblockDetails.Inlines.Add("says at " + CreatedAt);
			if (!string.IsNullOrEmpty(Source)) {
				textblockDetails.Inlines.Add(" via ");
				if (SourceUrl == null) {
					textblockDetails.Inlines.Add(Source);
				} else {
					var hyperlinkSource = new Hyperlink();
					hyperlinkSource.Inlines.Add(Source);
					hyperlinkSource.Click += (s, e) => Process.Start(SourceUrl);
					textblockDetails.Inlines.Add(hyperlinkSource);
				}
			}
			textblockDetails.Inlines.Add(".");

			// textblockPlain
			foreach (var word in TextParser.ParsedText) {
				string linkUrl = null;

				if (word.Type == StatusTextType.Normal) {
					textblockPlain.Inlines.Add(word.Text);
				} else if (word.Type == StatusTextType.NormalHyperLink) {
					linkUrl = word.Text;
				} else if (word.Type == StatusTextType.ThumbHyperLink) {
					linkUrl = word.Text;

					var textblockThumbImage = new TextBlock {
						Margin = new Thickness(3.0, 0, 0, 0),
					};
					textblockThumbImage.Inlines.Add(GetThumbHyperlink(
						TextParser.GetThumbUrl(word.Text),
						linkUrl,
						100, 100
					));
					imageContainer.Children.Add(textblockThumbImage);
				} else if (word.Type == StatusTextType.Username) {
					linkUrl = @"http://twitter.com/" + word.Text.Replace("@", "");
				} else if (word.Type == StatusTextType.Hashtag) {
					linkUrl = @"http://search.twitter.com/search?" + ModelUtility.GetOption("q=" + word.Text);
				}

				if (linkUrl != null) {
					var hyperlink = new Hyperlink();
					hyperlink.Click += (s, e) => Process.Start(linkUrl);
					if (word.Type == StatusTextType.NormalHyperLink) {
						hyperlink.MouseEnter += (s, e) => {
							hyperlink.ToolTip = "...";
						};
						hyperlink.ToolTipOpening += (s, e) => {
							if (Cache.ShortenUrl.ContainsKey(linkUrl)) {
								Dispatch.Method(() => hyperlink.ToolTip = Cache.ShortenUrl[linkUrl]);
							} else {
								ModelUtility.DownloadContextAsync("http://search.twitter.com/hugeurl?url=" + linkUrl, (s2, e2) => {
									string expandUrl = null;
									try {
										expandUrl = Encoding.UTF8.GetString(e2.Result);
									} catch {
										expandUrl = "unknown";
									}
									if (expandUrl != null) {
										if (!Cache.ShortenUrl.ContainsKey(linkUrl)) {
											Cache.ShortenUrl.Add(linkUrl, expandUrl == string.Empty ? linkUrl : expandUrl);
										}
										hyperlink.ToolTip = Cache.ShortenUrl[linkUrl];
									}
								});
							}
						};
					}
					hyperlink.Inlines.Add(word.Text);
					textblockPlain.Inlines.Add(hyperlink);
				}
			}

			return textContainer;
		}

		private StackPanel GetTree() {
			var textContainer = GetTextConteiner();

			if (!string.IsNullOrEmpty(InReplyToStatusId)) {
				var worker = new BackgroundWorker();
				worker.DoWork += (s, e) => {
					try {
						var status = Cache.Statuses.ContainsKey(InReplyToStatusId) ?
						Cache.Statuses[InReplyToStatusId] : Statuses.Show(Format.Xml, InReplyToStatusId);
						Dispatch.Method(() => textContainer.Children.Add(status.GetThumbAndText()));
						if (!string.IsNullOrEmpty(status.InReplyToStatusId)) {
							status.ShownTree = true;
						}
					} catch {
						InReplyToStatusId = string.Empty;
					}
				};
				worker.RunWorkerAsync();
			}

			return textContainer;
		}

		private DockPanel GetThumbAndText() {
			var dockPanel = new DockPanel();

			var image = User.ProfileImageHyperlink;
			var text = TextContainer;
			DockPanel.SetDock(image, Dock.Left);
			dockPanel.Children.Add(image);
			dockPanel.Children.Add(text);

			return dockPanel;
		}
		#endregion

	}

}
