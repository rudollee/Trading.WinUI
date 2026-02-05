using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Trading.WinUI.Extensions;

public class ObservableCollectionEx<T> : ObservableCollection<T>
{
	private int _suppressCount;

	public void PauseUpdate() => _suppressCount++;

	public void ResumeUpdate()
	{
		if (--_suppressCount == 0)
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

		OnCollectionChanged(new NotifyCollectionChangedEventArgs(
			action: NotifyCollectionChangedAction.Add,
			changedItem: itemsList.First(),
			index: index));
	}

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
		if (_suppressCount > 0) return;
        base.OnCollectionChanged(e);
    }
}
