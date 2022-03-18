using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Castle.DynamicProxy;

using Scorpio.DynamicProxy;

using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace Scorpio.Bougainvillea.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModifiableIInterceptorExtensions
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        private static readonly ModifiableProxyGenerationHook _hook = new ModifiableProxyGenerationHook();

        private static readonly Castle.DynamicProxy.IInterceptor _interceptor = new ModifiableIInterceptor();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GenerateProxy<T>(this T value) where T : class
        {
            var _options = new ProxyGenerationOptions { Hook = _hook, }.Action(o => o.AddMixinInstance(new Modifiable()));
            return _generator.CreateClassProxyWithTarget(value, _options, _interceptor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GenerateProxy(this object value, Type type)
        {
            var _options = new ProxyGenerationOptions { Hook = _hook, }.Action(o => o.AddMixinInstance(new Modifiable()));
            return _generator.CreateClassProxyWithTarget(type, value, _options, _interceptor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T SetModified<T>(this T value)
        {
            (value as IModifiable)?.SetModifyState();
            return value;
        }

    }

    internal class ModifiableProxyGenerationHook : IProxyGenerationHook
    {
        public void MethodsInspected()
        {
        }
        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return methodInfo.IsSpecialName && !methodInfo.Name.Contains(nameof(IModifiable.Modified)) && (methodInfo.Name.StartsWith("set_") || methodInfo.Name.StartsWith("get_"));
        }
    }

    internal class ModifiableIInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            if (methodName.StartsWith("set_"))
            {
                var propertyName = methodName.Substring(4);
                var property = invocation.TargetType.GetProperty(propertyName);
                if (property != null && !((property.GetValue(invocation.Proxy, null)?.Equals(invocation.Arguments.First())) ?? false))
                {
                    (invocation.Proxy as IModifiable).SetModifyState();
                }
            }
            invocation.Proceed();
        }

    }
}
