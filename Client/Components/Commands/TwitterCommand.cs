using System.Windows.Input;

namespace Client.Components.Commands {

	public static class TwitterCommand {

		public static readonly ICommand Refresh = new RoutedCommand();

	}

}
