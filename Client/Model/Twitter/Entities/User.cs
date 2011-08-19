using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Client.Model.Twitter.Entities {

	/// <summary>
	/// ユーザを表します。
	/// </summary>
	public class User {

		#region Field
		private string screenName;
		#endregion

		#region Property
		/// <summary>
		/// 名前を取得または設定します。
		/// </summary>
		public string Name {
			get;
			set;
		}

		/// <summary>
		/// ユーザ名を取得または設定します。
		/// </summary>
		public string ScreenName {
			get {
				return screenName;
			}
			set {
				Cache.ScreenNames.Add("@" + value);
				screenName = value;
			}
		}

		/// <summary>
		/// ユーザIDを取得または設定します。
		/// </summary>
		public string Id {
			get;
			set;
		}

		/// <summary>
		/// プロフィール画像のURLを取得または設定します。
		/// </summary>
		public string ProfileImageUrl {
			get;
			set;
		}

		/// <summary>
		/// プロフィール画像を取得します。
		/// </summary>
		public BitmapImage ProfileImage {
			get {
				BitmapImage profileImage;

				if (Cache.Images.ContainsKey(ProfileImageUrl)) {
					profileImage = Cache.Images[ProfileImageUrl];
				} else {
					profileImage = new BitmapImage();
					profileImage.BeginInit();
					profileImage.CacheOption = BitmapCacheOption.OnDemand;
					profileImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
					profileImage.UriSource = new Uri(ProfileImageUrl);
					profileImage.EndInit();
					profileImage.DownloadCompleted += (s, e) => {
						if (!Cache.Images.ContainsKey(ProfileImageUrl)) {
							Cache.Images.Add(ProfileImageUrl, profileImage);
						}
					};
				}

				return profileImage;
			}
		}

		/// <summary>
		/// プロフィール画像のハイパーリンクを取得します。
		/// </summary>
		public TextBlock ProfileImageHyperlink {
			get {
				var hyperlink = new Hyperlink();
				hyperlink.Click += (s, e) => Process.Start("http://twitter.com/" + ScreenName);
				hyperlink.Inlines.Add(new Image {
					Width = 48,
					Height = 48,
					Source = ProfileImage,
					HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
				});

				var profileImageHyperlink = new TextBlock {
					ToolTip = string.Format("{0}({1})", Name, ScreenName),
				};
				profileImageHyperlink.Inlines.Add(hyperlink);
				return profileImageHyperlink;
			}
		}
		#endregion

		#region Constructor
		public User() {

		}

		public User(Dictionary<string, object> user) {
			ScreenName = user["screen_name"] as string;
			Id = user["id_str"] as string;
			Name = user["name"] as string;
			ProfileImageUrl = user["profile_image_url"] as string;
		}

		public User(XmlNode node) {
			ScreenName = node["screen_name"].InnerText;
			Name = node["name"].InnerText;
			Id = node["id"].InnerText;
			ProfileImageUrl = node["profile_image_url"].InnerText;
		}
		#endregion

	}

}
