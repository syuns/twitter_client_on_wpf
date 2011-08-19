using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Client.Components.ViewModel;
using Client.Extension;
using Client.ViewModel;

namespace Client {

	/// <summary>
	/// This application's main window.
	/// </summary>
	public partial class MainWindow : Window {

		#region Field
		private int workerCounter;
		#endregion

		#region Property
		private int WorkerCounter {
			get {
				return workerCounter;
			}
			set {
				workerCounter = value;
				Dispatch.Method(() => {
					if (workerCounter == 0) {
						this.Cursor = null;
					} else {
						this.Cursor = Cursors.Wait;
					}
				});
			}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the MainWindow class.
		/// </summary>
		public MainWindow() {
			InitializeComponent();

			Closing += (s, e) => ViewModelLocator.Cleanup();
			WorkerCounter = 0;

			// initialize view model
			var configViewModel = new ConfigurationViewModel(this);
			var homeViewModel = new HomeViewModel(this, configViewModel);
			var searchViewModel = new SearchViewModel(this, configViewModel);
			var streamViewModel = new StreamViewModel(this, configViewModel);

			// initialize view model property
			configViewModel.OnSearchQueryChanged = (e1, e2) => {
				searchViewModel.ArrangeTabItems(e1.SplitByLineOrComma(), e2.SplitByLineOrComma(), searchViewModel.QueryTabItems);
			};
			configViewModel.OnTimelineScreenNameChanged = (e1, e2) => {
				searchViewModel.ArrangeTabItems(e1.SplitByLineOrComma(), e2.SplitByLineOrComma(), searchViewModel.UserTabItems);
			};
			streamViewModel.HomeViewModel = homeViewModel;

			// viewmodel bind to view datacontext
			homeView.DataContext = homeViewModel;
			searchView.DataContext = searchViewModel;
			streamView.DataContext = streamViewModel;
			configView.DataContext = configViewModel;

			//ConfigurationViewModel.SetTitle();
			configViewModel.Load();
			configViewModel.SetTitle();

			var main = this.DataContext as MainViewModel;
			main.Main = this;
			main.Configuration = configViewModel;
			main.Stream = streamViewModel;
			main.SearchViewModel = searchViewModel;
		}
		#endregion

		#region Method
		public BackgroundWorker WorkerFactory(DoWorkEventHandler doWork, bool showWaitCursor) {
			return WorkerFactory(doWork, showWaitCursor, (s, e) => {
			});
		}

		public BackgroundWorker WorkerFactory(DoWorkEventHandler doWork, bool showWaitCursor, RunWorkerCompletedEventHandler runWorkerComplited) {
			var worker = new BackgroundWorker();

			worker.DoWork += (s, e) => {
				if (showWaitCursor) {
					WorkerCounter++;
				}
			};
			worker.DoWork += doWork;
			worker.RunWorkerCompleted += runWorkerComplited;
			worker.RunWorkerCompleted += (s, e) => {
				if (showWaitCursor) {
					WorkerCounter--;
				}
			};

			return worker;
		}
		#endregion

		#region Event Handdler
		private void SelectTab(int index, IInputElement value) {
			tabControl.SelectedIndex = index;
			FocusManager.SetFocusedElement(this, value);
			Keyboard.Focus(value);
		}

		private void SelectHomeViewTab(object sender, RoutedEventArgs e) {
			SelectTab(0, homeView);
		}

		private void SelectSearchViewTab(object sender, RoutedEventArgs e) {
			SelectTab(1, searchView);
		}

		private void SelectStreamViewTab(object sender, RoutedEventArgs e) {
			SelectTab(2, streamView);
		}

		private void SelectConfigViewTab(object sender, RoutedEventArgs e) {
			SelectTab(3, configView);
		}
		#endregion

	}

}
