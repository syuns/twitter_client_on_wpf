using System.Xml;

namespace Client.Model.Yahoo.Entities {

	public class Word {

		public string Surface {
			get;
			private set;
		}

		public string Reading {
			get;
			private set;
		}

		public string Pos {
			get;
			private set;
		}

		public string BaseForm {
			get;
			private set;
		}

		public Word(XmlNode node) {
			if (node["surface"] != null)
				Surface = node["surface"].InnerText;
			if (node["reading"] != null)
				Reading = node["reading"].InnerText;
			if (node["pos"] != null)
				Pos = node["pos"].InnerText;
			if (node["baseform"] != null)
				BaseForm = node["baseform"].InnerText;
		}

		public override string ToString() {
			return string.Format("Surface: {0}\tReading: {1}\tPos: {2}\tBaseForm: {3}",
					Surface, Reading, Pos, BaseForm);
		}

	}

}
