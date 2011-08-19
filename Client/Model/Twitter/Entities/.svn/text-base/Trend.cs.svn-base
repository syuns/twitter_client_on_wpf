using System.Collections.Generic;

namespace Client.Model.Twitter.Entities {

	/// <summary>
	/// トレンドを表します。
	/// </summary>
	public class Trend {

		#region Property
		public string Url {
			get;
			private set;
		}

		public string Query {
			get;
			private set;
		}

		public string Name {
			get;
			private set;
		}
		#endregion

		#region Constructor
		public Trend(Dictionary<string, object> trendHash) {
			Url = trendHash["url"] as string;
			Query = trendHash["query"] as string;
			Name = trendHash["name"] as string;
		}
		#endregion

		#region Method
		public override string ToString() {
			return string.Format("Url:{0},Query:{1},Name:{2}", Url, Query, Name);
		}
		#endregion

	}

}
