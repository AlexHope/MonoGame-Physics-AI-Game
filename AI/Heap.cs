using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    // This is more of a utility class, although it is currently unused
    class Heap<Item> where Item : IHeapItem<Item>
    {
        private Item[] _items;
        private int _currentItemCount;

        public Heap(int maximumSize)
        {
            _items = new Item[maximumSize];
        }

        // Adds an item to the heap
        public void Add(Item item)
        {
            item.HeapIndex = _currentItemCount;
            _items[_currentItemCount] = item;
            SortItemUp(item);

            _currentItemCount++;
        }

        // Removes the first item from the heap
        public Item RemoveFirstItem()
        {
            Item firstItem = _items[0];
            _currentItemCount--;

            _items[0] = _items[_currentItemCount];
            _items[0].HeapIndex = 0;
            SortItemDown(_items[0]);

            return firstItem;
        }

        // Returns if the given item is already in the heap
        public bool ContainsItem(Item item)
        {
            return Equals(_items[item.HeapIndex], item);
        }

        // Sorts an item up in the heap
        public void SortItemUp(Item item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                Item parentItem = _items[parentIndex];

                if (item.CompareTo(parentItem) > 0)
                {
                    SwapItems(item, parentItem);
                }
                else
                {
                    break;
                }

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        // Sorts an item down in the heap
        public void SortItemDown(Item item)
        {
            while (true)
            {
                int childIndexLeft = (item.HeapIndex * 2) + 1;
                int childIndexRight = (item.HeapIndex * 2) + 2;

                int swapIndex = 0;

                if (childIndexLeft < _currentItemCount)
                {
                    swapIndex = childIndexLeft;

                    if (childIndexRight < _currentItemCount)
                    {
                        if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(_items[swapIndex]) < 0)
                    {
                        SwapItems(item, _items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        // Swaps 2 items positions in the heap
        void SwapItems(Item itemA, Item itemB)
        {
            _items[itemA.HeapIndex] = itemB;
            _items[itemB.HeapIndex] = itemA;

            int itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }

        public int CurrentItemCount
        {
            get
            {
                return _currentItemCount;
            }
        }
    }

    public interface IHeapItem<Item> : IComparable<Item>
    {
        int HeapIndex { get; set; }
    }
}
