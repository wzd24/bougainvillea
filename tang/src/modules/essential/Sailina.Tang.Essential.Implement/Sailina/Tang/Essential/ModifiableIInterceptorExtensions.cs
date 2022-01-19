
using Castle.DynamicProxy;

namespace Sailina.Tang.Essential
{
    internal static class ModifiableIInterceptorExtensions
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        private static readonly ModifiableProxyGenerationHook _hook = new ModifiableProxyGenerationHook();

        private static readonly IInterceptor _interceptor = new ModifiableIInterceptor();

        public static T GenerateProxy<T>(this T value) where T : class
        {
            var _options = new ProxyGenerationOptions { Hook = _hook, }.Action(o => o.AddMixinInstance(new Modifiable()));
            return _generator.CreateClassProxyWithTarget(value, _options, _interceptor);
        }
    }

}
