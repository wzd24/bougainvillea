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
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TScore"></typeparam>
    public class SkipList<TKey, TScore> : ISkipList<TKey, TScore>
        where TScore : IComparable<TScore>
    {
        /// <summary>
        /// 
        /// </summary>
        public const int LEVEL_LIMIT = 16;
        private readonly SkipListNode _head;
        private readonly SkipListNode _end;

        private readonly Dictionary<TKey, SkipListNode> _nodes = new Dictionary<TKey, SkipListNode>();
        private readonly Random _random = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minScore"></param>
        /// <param name="maxScore"></param>
        public SkipList(Func<TScore> minScore, Func<TScore> maxScore)
        {
            _head = new SkipListNode { Score = maxScore() };
            _end = new SkipListNode { Score = minScore() };
            _head.SetNext(_end, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minScore"></param>
        /// <param name="maxScore"></param>
        /// <param name="maxLength"></param>
        public SkipList(Func<TScore> minScore, Func<TScore> maxScore,int maxLength) : this(minScore,maxScore)
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
        /// <param name="key"></param>
        /// <param name="score"></param>
        public void Set(TKey key, TScore score)
        {
            if (_nodes.TryGetValue(key, out var node))
            {
                if (node.Score.CompareTo(score) == 0)
                {
                    return;
                }
                Remove(node);
            }
            else
            {
                if (Length >= MaxLength)
                {
                    if (score.CompareTo(_end[0].Previous.Score) <= 0)
                    {
                        return;
                    }
                    node = PopLastNode();
                }
                else
                {
                    node = new SkipListNode();
                    Length++;
                }
                _nodes[key] = node;
            }
            node.Score = score;
            Insert(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Remove(TKey item) => throw new NotImplementedException();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TKey> GetEnumerator()
        {
            var node = _head[0].Next;
            while (node != _end)
            {
                yield return node.Key;
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
        public IEnumerable<TKey> GetSegment(int begin, int count)
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
                yield return node.Key;
                node = node[0].Next;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int IndexOf(TKey key) => throw new NotImplementedException();

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
                while (before[i].Next.Score.CompareTo(node.Score) >= 0)
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
            _nodes.Remove(node.Key);
            return node;
        }


        private class SkipListNode
        {
            private readonly List<SkipListNodePoint> _points = new List<SkipListNodePoint>() { new SkipListNodePoint() };

            public TKey Key { get; set; }

            public TScore Score { get; set; }

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
