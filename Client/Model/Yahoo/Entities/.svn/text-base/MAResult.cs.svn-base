using System.Collections.Generic;
using System.Xml;

namespace Client.Model.Yahoo.Entities {

	public class MAResult {

		public int TotalCount {
			get;
			private set;
		}

		public int FilteredCount {
			get;
			private set;
		}

		public List<Word> WordList {
			get;
			private set;
		}

		public MAResult(XmlNode node, XmlNamespaceManager ns) {
			if (node != null) {
				int p;
				if (int.TryParse(node["total_count"].InnerText, out p))
					TotalCount = p;
				if (int.TryParse(node["filtered_count"].InnerText, out p))
					FilteredCount = p;

				XmlNodeList nodeList = node.SelectNodes("y:word_list/y:word", ns);
				if (nodeList.Count > 0) {
					WordList = new List<Word>();
					foreach (XmlNode wordNode in nodeList) {
						WordList.Add(new Word(wordNode));
					}
				}
			}
		}

	}

}
