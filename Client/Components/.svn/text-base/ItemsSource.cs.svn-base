using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Model;
using Client.Extension;
using System.Text.RegularExpressions;

namespace Client.Components {

	public class ItemsSource {

		public IEnumerable<string> GetItems(string text) {
			var source = from word in Cache.Hashtages.Union(Cache.ScreenNames).Union(Cache.Hyperlinks)
						 orderby word
						 select word;

			if (string.IsNullOrEmpty(text)) {
				return source;
			} else {
				return from s in source
					   where s.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1
					   select s;
			}
		}

	}

}
