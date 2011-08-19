using Client.Model.Twitter.Entities;
using GalaSoft.MvvmLight;

namespace Client.Components.ViewModel {

	public class TwitterDataGridViewModel : ViewModelBase {

		#region Field
		private Status selectedStatus;
		private double fontSize;
		#endregion

		#region Property
		public Status SelectedStatus {
			get {
				return selectedStatus;
			}
			set {
				selectedStatus = value;
				this.RaisePropertyChanged("SelectedStatus");
			}
		}

		public double FontSize {
			get {
				return fontSize;
			}
			set {
				fontSize = value;
				this.RaisePropertyChanged("FontSize");
			}
		}
		#endregion

		public bool CanShowTree() {
			return SelectedStatus != null && !string.IsNullOrEmpty(SelectedStatus.InReplyToStatusId);
		}

	}

}
