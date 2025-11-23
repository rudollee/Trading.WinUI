using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Trading.WinUI.Extensions;

public class ObservableCollectionEx<T> : ObservableCollection<T>
{
	public void AddRange(IEnumerable<T> collection)
	{
		CheckReentrancy();

		if (!collection.Any()) return;

		var itemsList = (List<T>)Items;
		itemsList.AddRange(collection);

		OnCollectionChanged(new NotifyCollectionChangedEventArgs(
			action: NotifyCollectionChangedAction.Add,
			changedItem: itemsList.Last(),
			index: itemsList.Count - 1));
	}

	public void InsertRange(int index, IEnumerable<T> collection)
	{
		CheckReentrancy();

		if (!collection.Any()) return;

		var itemsList = (List<T>)Items;
		itemsList.InsertRange(index, collection);

		OnCollectionChanged(new NotifyCollectionChangedEventArgs(
			action: NotifyCollectionChangedAction.Add,
			changedItem: itemsList.First(),
			index: index));
	}
}
