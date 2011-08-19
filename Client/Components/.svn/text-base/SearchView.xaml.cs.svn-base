using System.Windows.Controls;
using System.Windows.Input;
using Client.Components.ViewModel;

namespace Client.Components {

	/// <summary>
	/// SearchView.xaml の相互作用ロジック
	/// </summary>
	public partial class SearchView : UserControl {

		public SearchView() {
			InitializeComponent();
		}

		private void Refresh(object sender, ExecutedRoutedEventArgs e) {
			var search = this.DataContext as SearchViewModel;
			if (search != null) {
				search.Refresh(sender, e);
			}
		}

	}

}
