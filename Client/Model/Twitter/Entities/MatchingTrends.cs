using System.Collections.Generic;

namespace Client.Model.Twitter.Entities {

	/// <summary>
	/// トレンドの集合を表します。
	/// </summary>
	public class MatchingTrends {

		#region Property
		public Trend this[int i] {
			get {
				return TrendList[i];
			}
		}

		public string AsOf {
			get;
			private set;
		}

		public Location Location {
			get;
			private set;
		}

		private List<Trend> TrendList {
			get;
			set;
		}

		/// <summary>
		/// TrendListのTrend.Nameの集合を取得します。
		/// </summary>
		public string[] Hotwords {
			get;
			private set;
		}
		#endregion

		#region Constructor
		public MatchingTrends(Dictionary<string, object> trendsHash) {
			AsOf = trendsHash["as_of"] as string;

			var locations = trendsHash["locations"] as object[];
			var locationHash = locations[0] as Dictionary<string, object>;
			Location = new Location(locationHash);

			TrendList = new List<Trend>();
			var trends = trendsHash["trends"] as object[];
			foreach (var e in trends) {
				var trendHash = e as Dictionary<string, object>;
				TrendList.Add(new Trend(trendHash));
			}

			List<string> hotwordList = new List<string>();
			foreach (Trend value in TrendList) {
				hotwordList.Add(value.Name);
			}
			Hotwords = hotwordList.ToArray();
		}
		#endregion

		#region Method
		public override string ToString() {
			return string.Format("AsOf:{0},Location:{1},TrendList:{2},Hotwords:{3}",
				AsOf, Location, TrendList, Hotwords);
		}
		#endregion

	}

}
