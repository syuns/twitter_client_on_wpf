using System.Windows.Input;
using Client.Components;
using Client.Components.ViewModel;
using Client.Model.Twitter;
using Client.Model.Twitter.Api.Rest;
using Client.Model.Twitter.Entities;
using Client.Model.Yahoo.Api.Rest;
using Client.Model.Yahoo.Entities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Text;
using Client.Model.Twitter.Api;
using System.Collections.Generic;

namespace Client.ViewModel {

	/// <summary>
	/// This class contains properties that the main View can data bind to.
	/// <para>
	/// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
	/// </para>
	/// <para>
	/// You can also use Blend to data bind with the tool's support.
	/// </para>
	/// <para>
	/// See http://www.galasoft.ch/mvvm/getstarted
	/// </para>
	/// </summary>
	public class MainViewModel : ViewModelBase {

		#region Field
		private StreamViewModel stream;
		private ConfigurationViewModel configuration;
		private SearchViewModel searchViewModel;
		private HomeViewModel home;
		#endregion

		#region Property
		public MainWindow Main {
			get;
			set;
		}

		public ConfigurationViewModel Configuration {
			get {
				return configuration;
			}
			set {
				configuration = value;
				this.RaisePropertyChanged("Configuration");
			}
		}

		public HomeViewModel Home {
			get {
				return home;
			}
			set {
				home = value;
				this.RaisePropertyChanged("Home");
			}
		}

		public SearchViewModel SearchViewModel {
			get {
				return searchViewModel;
			}
			set {
				searchViewModel = value;
				this.RaisePropertyChanged("SearchViewModel");
			}
		}

		public StreamViewModel Stream {
			get {
				return stream;
			}
			set {
				stream = value;
				this.RaisePropertyChanged("Stream");
			}
		}

		public ICommand FavoritesChangeCommand {
			get;
			private set;
		}

		public ICommand RetweetCommand {
			get;
			private set;
		}

		public ICommand DestroyCommand {
			get;
			set;
		}

		public ICommand UpdateCommand {
			get;
			private set;
		}

		public ICommand ReplyCommand {
			get;
			private set;
		}

		public ICommand ShowReplyCommand {
			get;
			private set;
		}

		public ICommand MASearchZoomInCommand {
			get;
			private set;
		}

		public ICommand MASearchZoomOutCommand {
			get;
			private set;
		}
		#endregion

		#region Constructor
		public MainViewModel() {
			FavoritesChangeCommand = new RelayCommand(FavoritesChange, CanFavoritesChange);
			RetweetCommand = new RelayCommand(Retweet, CanRetweet);
			DestroyCommand = new RelayCommand(Destroy, CanDestroy);
			UpdateCommand = new RelayCommand<string>(Update, CanUpdate);
			ReplyCommand = new RelayCommand<string>(Reply, CanReply);
			ShowReplyCommand = new RelayCommand(ShowReply, CanShowReply);
			MASearchZoomInCommand = new RelayCommand(MASearchZoomIn, CanMASearch);
			MASearchZoomOutCommand = new RelayCommand(MASearchZoomOut, CanMASearch);
		}
		#endregion

		#region Method
		private void FavoritesCreate(string id) {
			Main.WorkerFactory((s, e) => {
				Status status = Favorites.Create(Format.Xml, id, "id=" + id);
				if (status != null) {

				}
			}, true).RunWorkerAsync();
		}

		private void FavoritesDestroy(string id) {
			Main.WorkerFactory((s, e) => {
				Status status = Favorites.Destroy(Format.Xml, id, "id=" + id);
				if (status != null) {

				}
			}, true).RunWorkerAsync();
		}
		#endregion

		#region Command
		private bool CanFavoritesChange() {
			return TwitterDataGrid.StaticViewModel.SelectedStatus != null && ConfigurationViewModel.OAuth.Authorized();
		}

		private void FavoritesChange() {
			Status status = TwitterDataGrid.StaticViewModel.SelectedStatus;
			if (status.Favorited) {
				FavoritesCreate(status.Id);
			} else {
				FavoritesDestroy(status.Id);
			}
		}

		private bool CanRetweet() {
			bool canRetweet = false;

			Status status = TwitterDataGrid.StaticViewModel.SelectedStatus;
			if (status != null) {
				canRetweet = ConfigurationViewModel.OAuth.Authorized() &&
					status.User.ScreenName != ConfigurationViewModel.OAuth.ScreenName;
			}

			return canRetweet;
		}

		private void Retweet() {
			Main.WorkerFactory((s, e) => {
				Status status = TwitterDataGrid.StaticViewModel.SelectedStatus;
				Status retweetStatus = Statuses.Retweet(Format.Xml, status.Id, "id=" + status.Id);
				if (retweetStatus != null) {

				}
			}, true).RunWorkerAsync();
		}

		private bool CanDestroy() {
			bool canDestroy = false;

			Status status = TwitterDataGrid.StaticViewModel.SelectedStatus;
			if (status != null) {
				canDestroy = ConfigurationViewModel.OAuth.Authorized() &&
					status.User.ScreenName == ConfigurationViewModel.OAuth.ScreenName;
			}

			return canDestroy;
		}

		private void Destroy() {
			Main.WorkerFactory((s, e) => {
				Status status = TwitterDataGrid.StaticViewModel.SelectedStatus;
				Status result = Statuses.Destroy(Format.Xml, status.Id, "id=" + status.Id);
				if (result != null) {

				}
			}, true).RunWorkerAsync();
		}

		private bool CanUpdate(string text) {
			bool validTweet = false;
			if (!string.IsNullOrEmpty(text)) {
				int length = text.Length;
				validTweet = length <= 140;
			}

			return ConfigurationViewModel.OAuth.Authorized() && validTweet;
		}

		private void Update(string text) {
			Main.WorkerFactory((s, e) => {
				Status result = Statuses.Update(Format.Xml, text);
				if (result != null) {

				}
			}, true).RunWorkerAsync();
		}

		private bool CanReply(string text) {
			return CanUpdate(text) &&
				TwitterDataGrid.StaticViewModel.SelectedStatus != null;
		}

		private void Reply(string text) {
			Main.WorkerFactory((s, e) => {
				Status status = TwitterDataGrid.StaticViewModel.SelectedStatus;
				Status result = Statuses.Update(Format.Xml, text, "in_reply_to_status_id=" + status.Id);
				if (result != null) {

				}
			}, true).RunWorkerAsync();
		}

		public bool CanShowReply() {
			Status status = TwitterDataGrid.StaticViewModel.SelectedStatus;

			bool canShow = status != null;
			if (canShow) {
				canShow = !string.IsNullOrEmpty(status.InReplyToStatusId);
			}
			return canShow;
		}

		private void ShowReply() {
			Status status = TwitterDataGrid.StaticViewModel.SelectedStatus;
			status.ShownTree = !status.ShownTree;
		}

		private bool CanMASearch() {
			return TwitterDataGrid.StaticViewModel.SelectedStatus != null;
		}

		private void MASearchZoomIn() {
			MASearch(false);
		}

		private void MASearchZoomOut() {
			MASearch(true);
		}

		private void MASearch(bool isZoomIn) {
			try {
				SearchViewModel.RefreshMASearch(isZoomIn, TwitterDataGrid.StaticViewModel.SelectedStatus, "lang=ja");
			} catch {
				// do nothing
			}
		}
		#endregion

	}

}
