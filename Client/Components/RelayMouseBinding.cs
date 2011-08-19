using System.Windows;
using System.Windows.Input;

namespace Client.Components {

	public class RelayMouseBinding : MouseBinding {

		public static readonly DependencyProperty CommandBindingProperty = DependencyProperty.Register("CommandBinding", typeof(ICommand), typeof(RelayMouseBinding), new FrameworkPropertyMetadata(OnCommandBindingChanged));

		public ICommand CommandBinding {
			get {
				return (ICommand)GetValue(CommandBindingProperty);
			}
			set {
				SetValue(CommandBindingProperty, value);
			}
		}

		private static void OnCommandBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			var mouseBinding = (RelayMouseBinding)d;
			mouseBinding.Command = e.NewValue as ICommand;
		}

	}

}
