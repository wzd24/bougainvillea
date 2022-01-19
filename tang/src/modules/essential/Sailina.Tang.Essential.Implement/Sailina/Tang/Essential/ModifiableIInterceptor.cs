
using Castle.DynamicProxy;

using Scorpio.Bougainvillea.Data;

namespace Sailina.Tang.Essential
{
    internal class ModifiableIInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            if (methodName.StartsWith("set_"))
            {
                var propertyName = methodName.Substring(4);
                var property = invocation.TargetType.GetProperty(propertyName);
                if (property != null && !property.GetValue(invocation.Proxy, null).Equals( invocation.Arguments.First()))
                {
                    (invocation.Proxy as IModifiable).SetModifyState();
                }
            }
            invocation.Proceed();
        }

    }

}
