using System;

namespace Client.Extension {

	public static class Dispatch {

		public static void Method(Action action) {
			App.Current.Dispatcher.BeginInvoke(
				action,
				null
			);
		}

	}

}
