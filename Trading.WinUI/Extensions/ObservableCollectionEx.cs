using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Trading.WinUI.Extensions;

public class ObservableCollectionEx<T> : ObservableCollection<T>
{
	private int _suppressCount;

	public void PauseUpdate() => Interlocked.Increment(ref _suppressCount);

	public void ResumeUpdate()
	{
		if (Interlocked.Decrement(ref _suppressCount) != 0) return;

		RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
	}

	public void AddRange(IReadOnlyList<T> collection)
	{
		if (collection is null) return;
		CheckReentrancy();
		if (collection.Count == 0) return;

		var oldIndex = Count;
		for (int i = 0; i < collection.Count; i++)
		{
			Items.Add(collection[i]);
		}

		if (Volatile.Read(ref _suppressCount) > 0) return;

		if (collection.Count > 1)
		{
			RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			return;
		}

		RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItem: collection[0], index: oldIndex));
	}

	public void InsertRange(int index, IReadOnlyList<T> collection)
	{
		if (collection is null) return;
		CheckReentrancy();
		if (collection.Count == 0) return;

		if (index < 0 || index > Count) return;

		int currentIndex = index;
		for (int i = 0; i < collection.Count; i++)
		{
			Items.Insert(currentIndex++, collection[i]);
		}

		if (Volatile.Read(ref _suppressCount) > 0) return;

		if (collection.Count > 1)
		{
			RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			return;
		}

		RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItem: collection[0], index: index));
	}

	public void RemoveRange(int index, int count)
	{
		if (count <= 0) return;
		CheckReentrancy();

		if (index < 0 || index + count > Count) return;

		var removedItem = Items[index];
		for (int i = 0; i < count; i++)
		{
			Items.RemoveAt(index);
		}

		if (Volatile.Read(ref _suppressCount) > 0) return;

		if (count > 1)
		{
			RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			return;
		}

		RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(
			NotifyCollectionChangedAction.Remove,
			changedItem: removedItem,
			index: index));
	}

	protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
		if (Volatile.Read(ref _suppressCount) > 0) return;
        base.OnCollectionChanged(e);
    }

	private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
	{
		OnCollectionChanged(e);
		OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
		OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
	}
}