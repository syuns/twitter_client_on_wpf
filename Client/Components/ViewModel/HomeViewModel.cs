using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Client.Extension;
using Client.Model.Twitter;
using Client.Model.Twitter.Api.Rest;
using Client.Model.Twitter.Entities;
using GalaSoft.MvvmLight;

namespace Client.Components.ViewModel {

	public class HomeViewModel : ViewModelBase {

		#region Property
		public MainWindow Main {
			get;
			set;
		}

		public ConfigurationViewModel Configuration {
			get;
			set;
		}

		public TabItem SelectedTabItem {
			private get;
			set;
		}

		private Func<Status, Status, bool> OrderPred {
			get;
			set;
		}

		public OrderedHashSet<Status> Timeline {
			get;
			set;
		}

		public OrderedHashSet<Status> Mentions {
			get;
			set;
		}

		public OrderedHashSet<Status> Favorites {
			get;
			set;
		}

		public OrderedHashSet<Status> DM {
			get;
			set;
		}
		#endregion

		#region Constructor
		public HomeViewModel(MainWindow main, ConfigurationViewModel configuration) {
			Main = main;
			Configuration = configuration;

			OrderPred = (entry, item) => {
				return entry.CreatedAt.CompareTo(item.CreatedAt) >= 0;
			};

			Timeline = new OrderedHashSet<Status>(OrderPred);
			Mentions = new OrderedHashSet<Status>(OrderPred);
			Favorites = new OrderedHashSet<Status>(OrderPred);
			DM = new OrderedHashSet<Status>(OrderPred);
		}
		#endregion

		#region Method
		public void InsertToMentions(Status status) {
			string screenName = Properties.Settings.Default.ScreenName;
			if (!string.IsNullOrEmpty(screenName) && status.Text.StartsWith("@" + screenName)) {
				Mentions.Insert(status);
			}
		}

		public void InsertToFavorites(Status status) {
			if (status.Favorited) {
				Favorites.Insert(status);
			}
		}

		public void InsertToTimeline(Status status) {
			Timeline.Insert(status);
			InsertToMentions(status);
			InsertToFavorites(status);
		}

		public void Remove(Status status) {
			Timeline.Remove(status);
			Mentions.Remove(status);
			Favorites.Remove(status);
		}

		private void RefreshTimeline(params string[] options) {
			Main.WorkerFactory((s, e) => {
				foreach (Status status in Statuses.HomeTimeline(Format.Xml, options)) {
					InsertToTimeline(status);
				}
			}, true).RunWorkerAsync();
		}

		private void RefreshMentions(params string[] options) {
			Main.WorkerFactory((s, e) => {
				foreach (Status status in Statuses.Mentions(Format.Xml, options)) {
					Mentions.Insert(status);
				}
			}, true).RunWorkerAsync();
		}

		private void RefreshFavorites(params string[] options) {
			Main.WorkerFactory((s, e) => {
				foreach (Status status in Client.Model.Twitter.Api.Rest.Favorites.Execute(Format.Xml, options)) {
					Favorites.Insert(status);
				}
			}, true).RunWorkerAsync();
		}

		private void RefreshDM(params string[] options) {
			Main.WorkerFactory((s, e) => {
				foreach (Status status in DirectMessages.Execute(Format.Xml, options)) {
					DM.Insert(status);
				}
			}, true).RunWorkerAsync();
		}
		#endregion

		#region Command
		public bool CanRefresh(object sender, CanExecuteRoutedEventArgs e) {
			return ConfigurationViewModel.OAuth.Authorized();
		}

		public void Refresh(object sender, ExecutedRoutedEventArgs e) {
			if (SelectedTabItem == null) {
				return;
			}

			var statuses = SelectedTabItem.DataContext as OrderedHashSet<Status>;

			var options = new List<string>();
			options.Add("count=100");
			var kv = e.Parameter as Dictionary<UpdateType, Status>;
			if (kv == null) {
				statuses.Clear();
			} else {
				if (kv.ContainsKey(UpdateType.Forward)) {
					options.Add("since_id=" + kv[UpdateType.Forward].Id);
				} else if (kv.ContainsKey(UpdateType.Prev)) {
					options.Add("max_id=" + kv[UpdateType.Prev].Id);
				}
			}

			if (statuses == Timeline) {
				RefreshTimeline(options.ToArray());
			} else if (statuses == Mentions) {
				RefreshMentions(options.ToArray());
			} else if (statuses == Favorites) {
				RefreshFavorites(options.ToArray());
			} else if (statuses == DM) {
				RefreshDM(options.ToArray());
			}
		}
		#endregion

	}

}
