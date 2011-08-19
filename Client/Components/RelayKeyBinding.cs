using System.Windows;
using System.Windows.Input;

namespace Client.Components {

	public class RelayKeyBinding : KeyBinding {

		public static readonly DependencyProperty CommandBindingProperty = DependencyProperty.Register("CommandBinding", typeof(ICommand), typeof(RelayKeyBinding), new FrameworkPropertyMetadata(OnCommandBindingChanged));

		public ICommand CommandBinding {
			get {
				return (ICommand)GetValue(CommandBindingProperty);
			}
			set {
				SetValue(CommandBindingProperty, value);
			}
		}

		private static void OnCommandBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			var keyBinding = (RelayKeyBinding)d;
			keyBinding.Command = e.NewValue as ICommand;
		}

	}

}
