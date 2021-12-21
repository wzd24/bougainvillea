using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Depletion
{
    internal class AttributeDepleteHandlerOptions
    {

        private readonly IDictionary<long, DepleteHandlerTypeNode> _nodes = new Dictionary<long, DepleteHandlerTypeNode>();


        public Type GetHandlerType(long[] depletion)
        {
            return GetHandlerTypes(_nodes, depletion.AsSpan());
        }

        public void AddHandlerType<T>()
            where T : IDepleteHandler
        {
            AddHandlerType(typeof(T));
        }

        public void AddHandlerType(Type type)
        {
            var attrs = type.GetAttributes<DepleteHandlerAttribute>(true);
            if (attrs.IsNullOrEmpty())
            {
                return;
            }
            foreach (var item in attrs)
            {
                AddHandlerType(item.Depletion, type);
            }
        }

        public void AddHandlerType(long[] depletion, Type type)
        {
            if (!type.IsAssignableTo<IDepleteHandler>())
            {
                throw new InvalidOperationException($"Type {type.FullName} not implement interface {nameof(IDepleteHandler)}.");
            }
            var nodes = _nodes;
            var node = default(DepleteHandlerTypeNode);
            foreach (var item in depletion)
            {
                node = nodes.GetOrAdd(item, k => new DepleteHandlerTypeNode { Key = k });
                nodes = node.Children;
            }
            node.Type = type;
        }

        private Type GetHandlerTypes(IDictionary<long, DepleteHandlerTypeNode> nodes, Span<long> depletion)
        {
            var key = depletion[0];
            var node = nodes.GetOrDefault(key);
            if (node == null)
            {
                return null;
            }
            return GetHandlerTypes(node.Children, depletion.Slice(1)) ?? node.Type;
        }

        private class DepleteHandlerTypeNode
        {
            public long Key { get; set; }

            public IDictionary<long, DepleteHandlerTypeNode> Children { get; } = new Dictionary<long, DepleteHandlerTypeNode>();

            public Type Type { get; set; }
        }
    }
}
