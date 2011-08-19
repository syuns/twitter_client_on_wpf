using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Client.Extension {

	public class OrderedHashSet<T> : ObservableCollection<T> {

		#region Property
		public Func<T, T, bool> Order {
			get;
			set;
		}
		#endregion

		#region Constructor
		public OrderedHashSet(Func<T, T, bool> order) {
			Order = order;
		}
		#endregion

		#region Method
		public void Insert(T item) {
			Dispatch.Method(() => {
				int n = base.Count;
				if (n == 0) {
					base.Add(item);
				} else if (!base.Contains(item)) {
					int i;
					for (i = 0 ; i < n ; i++) {
						if (Order(item, base[i])) {
							base.Insert(i, item);
							break;
						}
					}
					if (i == n) {
						base.Add(item);
					}
				}
			});
		}

		public void InsertRange(IEnumerable<T> enumerator) {
			foreach (T e in enumerator) {
				Insert(e);
			}
		}

		public new bool Remove(T item) {
			bool removed = false;
			Dispatch.Method(() => removed = base.Remove(item));
			return removed;
		}

		public new void RemoveAt(int index) {
			Dispatch.Method(() => base.RemoveAt(index));
		}

		public void RemoveRange(IEnumerable<T> enumerator) {
			foreach (T e in enumerator) {
				this.Remove(e);
			}
		}
		#endregion

	}

}
