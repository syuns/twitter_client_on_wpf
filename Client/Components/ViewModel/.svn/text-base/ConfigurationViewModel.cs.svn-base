using System;
using System.Collections.Generic;
using System.Windows.Input;
using Client.Extension;
using Client.Library.OAuth;
using Client.Model;
using Client.Model.Twitter;
using Client.Model.Twitter.Api;
using Client.Model.Twitter.Api.Rest;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Client.Components.ViewModel {

	public class ConfigurationViewModel : ViewModelBase {

		#region Field
		public static oAuthTwitter OAuth;

		private string timelineScreenName;
		private string searchKeyword;
		private string streamScreenName;
		private string streamQuery;
		private bool isStreamChecked;
		#endregion

		#region Property
		public MainWindow Main {
			get;
			set;
		}

		public string TimelineScreenName {
			get {
				return timelineScreenName;
			}
			set {
				OnTimelineScreenNameChanged(TimelineScreenName, value);
				timelineScreenName = value;
				this.RaisePropertyChanged("TimelineScreenName");
			}
		}

		public string SearchQuery {
			get {
				return searchKeyword;
			}
			set {
				OnSearchQueryChanged(SearchQuery, value);
				searchKeyword = value;
				this.RaisePropertyChanged("SearchQuery");
			}
		}

		public string StreamScreenName {
			get {
				return streamScreenName;
			}
			set {
				streamScreenName = value;
				this.RaisePropertyChanged("StreamScreenName");
			}
		}

		public string StreamKeyword {
			get {
				return streamQuery;
			}
			set {
				streamQuery = value;
				this.RaisePropertyChanged("StreamKeyword");
			}
		}

		public bool IsStreamChecked {
			get {
				return isStreamChecked;
			}
			set {
				isStreamChecked = value;
				this.RaisePropertyChanged("IsStreamChecked");
			}
		}

		public Action<string, string> OnTimelineScreenNameChanged {
			get;
			set;
		}

		public Action<string, string> OnSearchQueryChanged {
			get;
			set;
		}

		public ICommand ShowOAuthView {
			get;
			private set;
		}

		public ICommand OpenAuthorizePageCommand {
			get;
			private set;
		}

		public ICommand AOuthSaveCommand {
			get;
			private set;
		}

		public ICommand AOuthResetCommand {
			get;
			private set;
		}
		#endregion

		#region Constructor & Destructor
		static ConfigurationViewModel() {
			OAuth = new oAuthTwitter();
			OAuth.ConsumerKey = Properties.Settings.Default.ComsumerKey;
			OAuth.ConsumerSecret = Properties.Settings.Default.ComsumerSecret;
			OAuth.Token = Properties.Settings.Default.AccessToken;
			OAuth.TokenSecret = Properties.Settings.Default.AccessTokenSecret;
			OAuth.ScreenName = Properties.Settings.Default.ScreenName;
			OAuth.UserId = Properties.Settings.Default.UserId;
		}

		~ConfigurationViewModel() {
			Save();
		}

		public ConfigurationViewModel(MainWindow main) {
			Main = main;

			OnTimelineScreenNameChanged = (e1, e2) => {
			};
			OnSearchQueryChanged = (e1, e2) => {
			};

			ShowOAuthView = new RelayCommand(() => {
				new OAuthView() {
					DataContext = this,
				}.ShowDialog();
			});
			OpenAuthorizePageCommand = new RelayCommand(OpenAuthorizePage);
			AOuthSaveCommand = new RelayCommand<string>(OAuthSave, CanOAuthSave);
			AOuthResetCommand = new RelayCommand(OAuthReset, CanOAuthReset);
		}
		#endregion

		#region Method
		public void AOuthReset() {
			OAuth.Reset();
			OAuth.ConsumerKey = Properties.Settings.Default.ComsumerKey;
			OAuth.ConsumerSecret = Properties.Settings.Default.ComsumerSecret;

			if (Main != null) {
				Main.Title = "Client";
			}
		}

		public void OAuthSave() {
			OAuth.AccessTokenGet(OAuth.OAuthToken, OAuth.PIN);
			Properties.Settings.Default.AccessToken = OAuth.Token;
			Properties.Settings.Default.AccessTokenSecret = OAuth.TokenSecret;
			Properties.Settings.Default.ScreenName = OAuth.ScreenName;
			Properties.Settings.Default.UserId = OAuth.UserId;
			Properties.Settings.Default.Save();

			SetTitle();
		}

		public void SetTitle() {
			if (Main != null && OAuth.Authorized()) {
				Main.Title = string.Format("{0} - {1}", "@" + ConfigurationViewModel.OAuth.ScreenName, "Client");
			}
		}

		public void Load() {
			TimelineScreenName = Properties.Settings.Default.TimelineScreenName;
			SearchQuery = Properties.Settings.Default.SearchKeyword;
			StreamScreenName = Properties.Settings.Default.StreamScreenName;
			StreamKeyword = Properties.Settings.Default.StreamKeyword;
		}

		private void Save() {
			Properties.Settings.Default.TimelineScreenName = TimelineScreenName;
			Properties.Settings.Default.SearchKeyword = SearchQuery;
			Properties.Settings.Default.StreamScreenName = StreamScreenName;
			Properties.Settings.Default.StreamKeyword = StreamKeyword;

			Properties.Settings.Default.Save();
		}

		public string GetStreamFilterOption(bool includeFollow) {
			string option = null;

			var options = new List<string>();
			if (includeFollow) {
				var idList = new List<string>();
				foreach (string screenName in StreamScreenName.SplitByLineOrComma()) {
					try {
						idList.Add(Users.Show(Format.Xml, "id=" + screenName).Id);
					} catch {
						// do nothing
					}
				}
				if (idList.Count > 0) {
					options.Add("follow=" + Streaming.FormatFollowOrTrack(idList.ToArray()));
				}
			}
			string[] streamKeywords = StreamKeyword.SplitByLineOrComma();
			if (streamKeywords.Length > 0) {
				options.Add("track=" + Streaming.FormatFollowOrTrack(streamKeywords));
			}
			if (options.Count > 0) {
				option = ModelUtility.GetOption(options.ToArray());
			} else {
				IsStreamChecked = false;
			}

			return option;
		}
		#endregion

		#region Command
		private void OpenAuthorizePage() {
			AOuthReset();
			System.Diagnostics.Process.Start(OAuth.AuthorizationLinkGet());
		}

		private bool CanOAuthSave(string pin) {
			return !string.IsNullOrEmpty(pin) && !OAuth.Authorized();
		}

		private void OAuthSave(string pin) {
			OAuth.PIN = pin;
			try {
				this.OAuthSave();
			} catch {

			}
		}

		private bool CanOAuthReset() {
			return OAuth.Authorized();
		}

		private void OAuthReset() {
			AOuthReset();
		}
		#endregion

	}

}
