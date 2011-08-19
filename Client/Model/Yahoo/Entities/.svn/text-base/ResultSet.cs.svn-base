using System.Xml;

namespace Client.Model.Yahoo.Entities {

	public class ResultSet {

		public MAResult MAResult {
			get;
			private set;
		}

		public UniqResult UniqResult {
			get;
			private set;
		}

		public ResultSet(XmlNode node, XmlNamespaceManager ns) {
			MAResult = new MAResult(node.SelectSingleNode("y:ma_result", ns), ns);
			UniqResult = new UniqResult(node.SelectSingleNode("y:uniq_result", ns), ns);
		}

	}

}
