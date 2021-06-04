using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SkipList<T> : ISkipList<T>
           where T : IComparable<T>, IEquatable<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public const int LEVEL_LIMIT = 16;
        private readonly SkipListNode _head;
        private readonly SkipListNode _end;

        private readonly Dictionary<T, SkipListNode> _nodes = new Dictionary<T, SkipListNode>();
        private readonly Random _random = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minScore"></param>
        /// <param name="maxScore"></param>
        public SkipList(Func<T> minScore, Func<T> maxScore)
        {
            _head = new SkipListNode { Data = maxScore() };
            _end = new SkipListNode { Data = minScore() };
            _head.SetNext(_end, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minScore"></param>
        /// <param name="maxScore"></param>
        /// <param name="maxLength"></param>
        public SkipList(Func<T> minScore, Func<T> maxScore, int maxLength) : this(minScore, maxScore)
        {
            if (maxLength <= 0)
            {
                throw new ArgumentException("最大长度不能小于或等于 0", nameof(maxLength));
            }
            MaxLength = maxLength;

        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxLength { get; } = 10000;

        /// <summary>
        /// 
        /// </summary>
        public int Length { get; private set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public int MaxLevel { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Set(T item)
        {
            if (_nodes.TryGetValue(item, out var node))
            {
                if (node.Data.CompareTo(item) == 0)
                {
                    return IndexOf(item);
                }
                Remove(node);
            }
            else
            {
                if (Length >= MaxLength)
                {
                    if (item.CompareTo(_end[0].Previous.Data) <= 0)
                    {
                        return -1;
                    }
                    node = PopLastNode();
                }
                else
                {
                    node = new SkipListNode();
                    Length++;
                }
                _nodes[item] = node;
            }
            node.Data = item;
            Insert(node);
            return IndexOf(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            if (_nodes.TryGetValue(item, out var node))
            {
                Remove(node);
                _nodes.Remove(item);
                Length--;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            var node = _head[0].Next;
            while (node != _end)
            {
                yield return node.Data;
                node = node[0].Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<T> GetSegment(int begin, int count)
        {
            var node = _head[0].Next;
            for (var i = 0; i < begin; i++)
            {
                if (node == _end)
                {
                    break;
                }
                node = node[0].Next;
            }
            for (var i = 0; i < count; i++)
            {
                if (node == _end)
                {
                    yield break;
                }
                yield return node.Data;
                node = node[0].Next;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            if (!_nodes.ContainsKey(item))
            {
                return -1;
            }
            var node = _head;
            var rank = 0;
            while (node != _end)
            {
                if (node.Data.Equals(item))
                {
                    break;
                }
                rank++;
                node = node[0].Next;
            }
            if (node == _end)
            {
                return -1;
            }
            return rank;
        }



        private void Insert(SkipListNode node)
        {
            var level = GetLevel();
            node.SetMaxLevel(level);
            if (level > MaxLevel)
            {
                _head.SetMaxLevel(level);
                _end.SetMaxLevel(level);
                for (var i = MaxLevel + 1; i <= level; i++)
                {
                    _head.SetNext(_end, i);
                }
            }
            MaxLevel = Math.Max(MaxLevel, level);
            var before = _head;
            for (var i = MaxLevel; i >= 0; i--)
            {
                while (before[i].Next.Data.CompareTo(node.Data) >= 0)
                {
                    before = before[i].Next;
                }
                if (i <= level)
                {
                    AddAfter(before, node, i);
                }
            }
        }

        private int GetLevel()
        {
            var level = 0;
            while (level < LEVEL_LIMIT)
            {
                if (_random.Next(10000) % 2 == 1)
                {
                    break;
                }
                level++;
            }
            return level;
        }

        private void AddAfter(SkipListNode before, SkipListNode node, int level)
        {
            before[level].Next.SetPrevious(node, level);
            node.SetPrevious(before, level);
        }


        private void Remove(SkipListNode node)
        {
            for (var i = 0; i <= node.MaxLevel; i++)
            {
                node[i].Previous.SetNext(node[i].Next, i);
            }
            var maxLevel = MaxLevel;
            for (var i = maxLevel; i >= 0; i--)
            {
                if (_head[i].Next != _end)
                {
                    break;
                }
                MaxLevel--;
            }
        }

        private SkipListNode PopLastNode()
        {
            var node = _end[0].Previous;
            if (node == _head)
            {
                return null;
            }
            Remove(node);
            _nodes.Remove(node.Data);
            return node;
        }
        private class SkipListNode
        {
            private readonly List<SkipListNodePoint> _points = new List<SkipListNodePoint>() { new SkipListNodePoint() };

            public T Data { get; set; }

            internal int MaxLevel { get; set; }

            public SkipListNodePoint this[int index]
            {
                get
                {
                    return _points[index];
                }
            }

            internal void SetMaxLevel(int level)
            {
                for (var i = MaxLevel + 1; i <= level; i++)
                {
                    _points.Add(new SkipListNodePoint());
                }
                MaxLevel = level;
            }

            internal void SetNext(SkipListNode next, int level)
            {
                _points[level].Next = next;
                next._points[level].Previous = this;
            }

            internal void SetPrevious(SkipListNode previous, int level)
            {
                _points[level].Previous = previous;
                previous._points[level].Next = this;
            }
        }

        private class SkipListNodePoint
        {
            public SkipListNode Previous { get; set; }

            public SkipListNode Next { get; set; }
        }

    }
}
