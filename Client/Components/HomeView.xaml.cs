using System.Windows.Controls;
using System.Windows.Input;
using Client.Components.ViewModel;

namespace Client.Components {

	/// <summary>
	/// HomeView.xaml の相互作用ロジック
	/// </summary>
	public partial class HomeView : UserControl {

		public HomeView() {
			InitializeComponent();
		}

		private void Reflesh(object sender, ExecutedRoutedEventArgs e) {
			var home = this.DataContext as HomeViewModel;
			if (home != null) {
				home.Refresh(sender, e);
			}
		}

		private void CanRefresh(object sender, CanExecuteRoutedEventArgs e) {
			var home = this.DataContext as HomeViewModel;
			if (home != null) {
				e.CanExecute = home.CanRefresh(sender, e);
			}
		}

	}

}
