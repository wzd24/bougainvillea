using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Rewards
{
    internal class AttributeRewardHandlerOptions
    {

        private readonly IDictionary<int,RewardHandlerTypeNode> _nodes = new Dictionary<int,RewardHandlerTypeNode>();


        public Type GetHandlerType(int[] reward)
        {
            return GetHandlerTypes(_nodes, reward.AsSpan());
        }

        public void AddHandlerType<T>()
            where T:IRewardHandler
        {
            AddHandlerType(typeof(T));
        }

        public void AddHandlerType(Type type)
        {
            var attrs = type.GetAttributes<RewardHandlerAttribute>(true);
            if (attrs.IsNullOrEmpty())
            {
                return;
            }
            foreach (var item in attrs)
            {
                AddHandlerType(item.Reward, type);
            }
        }

        public void AddHandlerType(int[] reward,Type type)
        {
            if (!type.IsAssignableTo<IRewardHandler>())
            {
                throw new InvalidOperationException($"Type {type.FullName} not implement interface {nameof(IRewardHandler)}.");
            }
            var nodes = _nodes;
            var node = default(RewardHandlerTypeNode);
            foreach (var item in reward)
            {
                node = nodes.GetOrAdd(item,k=>new RewardHandlerTypeNode { Key = k });
                nodes = node.Children;
            }
            node.Type = type;
        }

        private Type GetHandlerTypes(IDictionary<int,RewardHandlerTypeNode> nodes, Span<int> reward)
        {
            var key = reward[0];
            var node = nodes.GetOrDefault(key);
            if (node == null)
            {
                return null;
            }
            return GetHandlerTypes(node.Children, reward.Slice(1)) ?? node.Type;
        }

        private class RewardHandlerTypeNode
        {
            public int Key { get; set; }

            public IDictionary<int, RewardHandlerTypeNode> Children { get; } = new Dictionary<int, RewardHandlerTypeNode>();

            public Type Type { get; set; }
        }
    }
}
