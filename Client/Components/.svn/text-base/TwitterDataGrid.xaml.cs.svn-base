using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Client.Components.Commands;
using Client.Components.ViewModel;
using Client.Model.Twitter.Entities;

namespace Client.Components {

	/// <summary>
	/// TwitterDataGrid.xaml の相互作用ロジック
	/// </summary>
	public partial class TwitterDataGrid : UserControl {

		#region Property
		public static TwitterDataGridViewModel StaticViewModel {
			get;
			private set;
		}
		#endregion

		#region Constructor
		static TwitterDataGrid() {
			StaticViewModel = new TwitterDataGridViewModel();
		}

		public TwitterDataGrid() {
			InitializeComponent();

			this.dataGrid.SelectionChanged += (s, e) => {
				StaticViewModel.SelectedStatus = this.dataGrid.SelectedItem as Status;
			};
		}
		#endregion

		#region Method
		protected override void OnPreviewMouseWheel(MouseWheelEventArgs e) {
			if (e.RightButton == MouseButtonState.Pressed) {
				// フォントサイズの変更
				double fontSize = this.dataGrid.FontSize;
				if (e.Delta > 0) {
					fontSize *= 1.2;
				} else {
					fontSize *= 0.8;
				}
				if (8 < fontSize && fontSize < 100) {
					StaticViewModel.FontSize = fontSize;
				}
			} else {
				int n;

				if (this.dataGrid.SelectedIndex == -1) {
					this.dataGrid.SelectedIndex = 0;
				}

				// 選択行の変更
				if (e.Delta > 0) {
					n = this.dataGrid.SelectedIndex - 1;
				} else {
					n = this.dataGrid.SelectedIndex + 1;
				}
				if (0 <= n && n < this.dataGrid.Items.Count) {
					this.dataGrid.SelectedIndex = n;
				}

				// 更新方法の決定
				UpdateType? type = null;
				if (n == 0) {
					type = UpdateType.Forward;
				} else if (n == this.dataGrid.Items.Count - 1) {
					type = UpdateType.Prev;
				}

				if (this.dataGrid.SelectedItem != null) {
					if (type != null) {
						var kv = new Dictionary<UpdateType, Status>();
						kv.Add((UpdateType)type, this.dataGrid.SelectedItem as Status);
						TwitterCommand.Refresh.Execute(kv);
					}
					this.dataGrid.ScrollIntoView(this.dataGrid.SelectedItem);
					this.dataGrid.Focus();
				}
			}

			e.Handled = true;
		}
		#endregion

	}

}
