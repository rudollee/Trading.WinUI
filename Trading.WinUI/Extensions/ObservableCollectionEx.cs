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

		var itemsList = (List<T>)Items;
		itemsList.AddRange(collection);

        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

	public void InsertRange(int index, IEnumerable<T> collection)
	{
		CheckReentrancy();

		if (!collection.Any()) return;

		var itemsList = (List<T>)Items;
		itemsList.InsertRange(index, collection);

		OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
	}

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
		if (_suppressCount > 0) return;
        base.OnCollectionChanged(e);
    }
}
