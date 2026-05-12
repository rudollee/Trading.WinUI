using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;

namespace Trading.WinUI.Extensions;

public class ObservableCollectionEx<T> : ObservableCollection<T>
{
	private int _suppressCount;

	public void PauseUpdate() => Interlocked.Increment(ref _suppressCount);

	public void ResumeUpdate()
	{
		if (Interlocked.Decrement(ref _suppressCount) == 0)
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}

	public void AddRange(IEnumerable<T> collection)
	{
		CheckReentrancy();

		if (!collection.Any()) return;

		var oldIndex = Items.Count;
		var itemsList = (List<T>)Items;
		itemsList.AddRange(collection);
		var newIndex = Items.Count;

		var items = collection as IList ?? collection.ToList();

		if (collection.Count() > 1)
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
		else
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
				changedItems: items,
				startingIndex: oldIndex));
		}
	}

	public void InsertRange(int index, IEnumerable<T> collection)
	{
		CheckReentrancy();

		if (!collection.Any()) return;

		var itemsList = (List<T>)Items;
		itemsList.InsertRange(index, collection);

		var items = collection as IList ?? collection.ToList();

		OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
			changedItems: items,
			startingIndex: index));
	}

	public void RemoveRange(int index, int count)
	{
		if (count <= 0) return;
		CheckReentrancy();

		var itemsList = (List<T>)Items;
		if (index < 0 || index + count > itemsList.Count) return;

		var removedItems = itemsList.GetRange(index, count);

		itemsList.RemoveRange(index, count);

		if (_suppressCount <= 0)
		{
			try
			{
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(
					NotifyCollectionChangedAction.Remove,
					changedItems: removedItems,
					startingIndex: index));
			}
			catch (NotSupportedException)
			{
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}
	}

	protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
		if (_suppressCount > 0) return;
        base.OnCollectionChanged(e);
    }
}
