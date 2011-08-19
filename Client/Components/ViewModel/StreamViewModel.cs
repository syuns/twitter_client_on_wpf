using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Client.Extension;
using Client.Model.Twitter.Api;
using Client.Model.Twitter.Entities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Client.Components.ViewModel {

	public class StreamViewModel : ViewModelBase {

		#region Field
		private static readonly int StreamQueueSize;
		#endregion

		#region Property
		public MainWindow Main {
			get;
			set;
		}

		public ConfigurationViewModel Configuration {
			get;
			set;
		}

		public ObservableCollection<Status> Stream {
			get;
			private set;
		}

		public ICommand ConnectStreamSampleCommand {
			get;
			private set;
		}

		public ICommand ConnectStreamFilterCommand {
			get;
			private set;
		}

		public ICommand ConnectUserStreamsCommand {
			get;
			private set;
		}

		public HomeViewModel HomeViewModel {
			get;
			set;
		}
		#endregion

		#region Constructor
		static StreamViewModel() {
			StreamQueueSize = 10;
		}

		public StreamViewModel(MainWindow main, ConfigurationViewModel configuration) {
			Main = main;
			Configuration = configuration;
			Stream = new ObservableCollection<Status>();
			ConnectStreamSampleCommand = new RelayCommand(ConnectStreamSample, CanConnect);
			ConnectStreamFilterCommand = new RelayCommand(ConnectStreamFilter, CanConnect);
			ConnectUserStreamsCommand = new RelayCommand(ConnectUserStreams, CanConnect);
		}
		#endregion

		#region Method
		private bool AnyContains(IEnumerable<string> ids, string targetId) {
			bool contains = false;

			foreach (string id in ids) {
				contains = id == targetId;
				if (contains) {
					break;
				}
			}

			return contains;
		}
		#endregion

		#region Command
		private void ConnectStreamSample() {
			Main.WorkerFactory(
				(s, e) => {
					Configuration.IsStreamChecked = true;
					Streaming.RunStreamSample(
						entry => {
							Dispatch.Method(() => {
								Stream.Insert(0, entry);
								if (Stream.Count > StreamQueueSize) {
									Stream.RemoveAt(StreamQueueSize);
								}
							});
						},
						entry => Dispatch.Method(() => Stream.Remove(entry)),
						() => Configuration.IsStreamChecked
					);
				},
				false,
				(s, e) => Configuration.IsStreamChecked = false
			).RunWorkerAsync();
		}

		private void ConnectStreamFilter() {
			string option = Configuration.GetStreamFilterOption(true);
			if (option == null) {
				return;
			}

			Main.WorkerFactory(
				(s, e) => {
					Configuration.IsStreamChecked = true;
					Streaming.RunStreamFilter(
						entry => {
							Dispatch.Method(() => {
								Stream.Insert(0, entry);
								if (Stream.Count > StreamQueueSize) {
									Stream.RemoveAt(StreamQueueSize);
								}
							});
						},
						entry => Dispatch.Method(() => Stream.Remove(entry)),
						() => Configuration.IsStreamChecked,
						option
					 );
				},
				false,
				(s, e) => Configuration.IsStreamChecked = false
			).RunWorkerAsync();
		}

		private void ConnectUserStreams() {
			string option = Configuration.GetStreamFilterOption(false);

			Main.WorkerFactory(
				(s, e) => {
					var friends = new List<string>();
					friends.Add(ConfigurationViewModel.OAuth.UserId);
					Configuration.IsStreamChecked = true;
					Streaming.RunUserStreams(
						entry => {
							Dispatch.Method(() => {
								if (AnyContains(friends, entry.User.Id)) {
									HomeViewModel.InsertToTimeline(entry);
								}
								Stream.Insert(0, entry);
								if (Stream.Count > StreamQueueSize) {
									Stream.RemoveAt(StreamQueueSize);
								}
							});
						},
						entry => {
							Dispatch.Method(() => {
								HomeViewModel.Remove(entry);
								Stream.Remove(entry);
							});
						},
						entry => {
							Dispatch.Method(() => HomeViewModel.InsertToFavorites(entry));
						},
						entry => {
							Dispatch.Method(() => HomeViewModel.Favorites.Remove(entry));
						},
						() => Configuration.IsStreamChecked,
						option,
						friends
					 );
				},
				false,
				(s, e) => Configuration.IsStreamChecked = false
			).RunWorkerAsync();
		}

		private bool CanConnect() {
			return ConfigurationViewModel.OAuth.Authorized();
		}
		#endregion

	}

}
