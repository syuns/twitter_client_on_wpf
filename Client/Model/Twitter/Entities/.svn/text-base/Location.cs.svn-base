using System.Collections.Generic;

namespace Client.Model.Twitter.Entities {

	/// <summary>
	/// woeidと関連付けされた地域を表します。
	/// </summary>
	public class Location {

		#region Property
		/// <summary>
		/// woeid(ex.1)を取得します。
		/// </summary>
		public string Woeid {
			get;
			private set;
		}

		/// <summary>
		/// 地域名(ex.Tokyo)を取得します。
		/// </summary>
		public string Name {
			get;
			private set;
		}
		#endregion

		#region Constructor
		public Location(Dictionary<string, object> locationHash) {
			Woeid = locationHash["woeid"] as string;
			Name = locationHash["name"] as string;
		}
		#endregion

		#region Method
		public override string ToString() {
			return string.Format("Woeid:{0},Name:{1}", Woeid, Name);
		}
		#endregion

	}

}
