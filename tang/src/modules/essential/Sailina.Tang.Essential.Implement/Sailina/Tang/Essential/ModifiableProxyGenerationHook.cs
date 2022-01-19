using System.Reflection;

using Castle.DynamicProxy;

using Scorpio.Bougainvillea.Data;

namespace Sailina.Tang.Essential
{
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
            return methodInfo.IsSpecialName && !methodInfo.Name.Contains(nameof(IModifiable.Modified)) && (methodInfo.Name.StartsWith("set_")|| methodInfo.Name.StartsWith("get_"));
        }
    }

}
