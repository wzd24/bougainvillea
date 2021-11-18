using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Middleware;
using Scorpio.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Reflection;
using Scorpio.Threading;

namespace Scorpio.Bougainvillea.Handler
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GameHandlerBase : IGameHandler, IServiceProviderAccessor
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly static string _methodName = "Execute";

        private readonly static Dictionary<Type, MethodInfo> _cachedMethods = new Dictionary<Type, MethodInfo>();

        private static readonly MethodInfo _getArgValueMethod = typeof(GameHandlerBase).GetMethod(nameof(GetArgValue), BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo _addMethodMethod = typeof(GameHandlerBase).GetMethod(nameof(AddMethodAsync), BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo _addMethodMethodVoid = typeof(GameHandlerBase).GetMethod(nameof(AddMethodVoidAsync), BindingFlags.NonPublic | BindingFlags.Static);

        private delegate Task<object> InvokeDelegate(IGameContext gameContext);

        private delegate Task<TResult> InvokeDelegate<TResult>(IGameContext gameContext);

        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        protected GameHandlerBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected IResponseMessage Response(int code, string message = null, object data = null)
        {
            return new ResponseMessage(code, message, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected IResponseMessage Error(int code, string message = null, object data = null)
        {
            return Response(code, message, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected IResponseMessage Success(object data)
        {
            return Response(0, data: data);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        async Task IGameHandler.ExecuteAsync(IGameContext context)
        {
            var result = await PreExecuteAsync(context);
            if (!result.IsSuccessed())
            {
                await context.Response.WriteAsync(result);
                return;
            }
            var type = GetType();
            var method = GetMethod(type);
            var addMethod = method.ReturnType.UnwrapTask() == typeof(void) ? _addMethodMethodVoid.MakeGenericMethod(type) : _addMethodMethod.MakeGenericMethod(type, method.ReturnType.UnwrapTask());
            await (addMethod.Invoke(null, new object[] { this, method, context }) as Task);
            await PostExecuteAsync(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task<IResponseMessage> PreExecuteAsync(IGameContext context)
        {
            return Task.FromResult<IResponseMessage>(ResponseMessage.Sucess);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task PostExecuteAsync(IGameContext context)
        {
            return Task.CompletedTask;
        }

        private static MethodInfo GetMethod(Type type)
        {
            return _cachedMethods.GetOrAdd(type, t =>
            {
                var method = t.GetMethod(_methodName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (method == null)
                {
                    method = t.GetMethod($"{_methodName}Async", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                if (method == null)
                {
                    throw new GameFriendlyException(StatusCodes.Status501NotImplemented, "ActionHandler not implemented.");
                }
                if (!typeof(Task).IsAssignableFrom(method.ReturnType))
                {
                    throw new InvalidOperationException();
                }
                return method;

            });
        }

        private static async Task AddMethodAsync<T, TResult>(T instance, MethodInfo method, IGameContext context)
        {
            var parameters = method.GetParameters();
            switch (parameters.Length)
            {
                case 0:
                    {
                        var @delegate = (Func<Task<TResult>>)method.CreateDelegate(typeof(Func<Task<TResult>>), instance);
                        var result = await @delegate();
                        await WriteResult(context, result);
                        break;
                    }
                case 1 when parameters[0].ParameterType == typeof(IGameContext):
                    {
                        var @delegate = (InvokeDelegate<TResult>)method.CreateDelegate(typeof(InvokeDelegate<TResult>), instance);
                        var result = await @delegate(context);
                        await WriteResult(context, result);
                        break;
                    }
                default:
                    {
                        await InvokeAsync<T, TResult>(instance, method, parameters, context);
                        break;
                    }
            }
        }

        private static async Task AddMethodVoidAsync<T>(T instance, MethodInfo method, IGameContext context)
        {
            var parameters = method.GetParameters();
            switch (parameters.Length)
            {
                case 0:
                    {
                        var @delegate = (Func<Task>)method.CreateDelegate(typeof(Func<Task>), instance);
                        await @delegate();
                        await context.Response.WriteEmptyAsync();
                        break;
                    }
                case 1 when parameters[0].ParameterType == typeof(IGameContext):
                    {
                        var @delegate = (Func<IGameContext, Task>)method.CreateDelegate(typeof(Func<IGameContext, Task>), instance);
                        await @delegate(context);
                        await context.Response.WriteEmptyAsync();
                        break;
                    }
                default:
                    {
                        await InvokeAsync<T>(instance, method, parameters, context);
                        break;
                    }
            }
        }


        private static async Task InvokeAsync<T, TResult>(T instance, MethodInfo method, ParameterInfo[] parameters, IGameContext context)
        {
            var facory = Compile<T, TResult>(method, parameters);
            var token = JToken.Parse(context.Request.Content);
            var result = await facory(instance, context, token);
            await WriteResult(context, result);
        }

        private static async Task InvokeAsync<T>(T instance, MethodInfo method, ParameterInfo[] parameters, IGameContext context)
        {
            var facory = Compile<T>(method, parameters);
            var token = JToken.Parse(context.Request.Content);
            await facory(instance, context, token);
            await context.Response.WriteEmptyAsync();
        }

        private static Func<T, IGameContext, JToken, Task> Compile<T>(MethodInfo method, ParameterInfo[] parameters)
        {
            var handler = typeof(T);
            var instanceArg = Expression.Parameter(handler, "handler");
            var contextArg = Expression.Parameter(typeof(IGameContext), "context");
            var tokenArg = Expression.Parameter(typeof(JToken), "token");
            var methodArguments = new Expression[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameterType = parameters[i].ParameterType;
                if (parameterType.IsByRef)
                {
                    throw new NotSupportedException();
                }
                if (parameterType == typeof(IGameContext))
                {
                    methodArguments[i] = contextArg;
                    continue;
                }
                var getArgParameterExpression = new Expression[]
                {
                    tokenArg,
                    Expression.Constant(parameters[i].Name,typeof(string)),
                    Expression.Constant(parameterType,typeof(Type))
                };
                var getArgValueCall = Expression.Call(_getArgValueMethod, getArgParameterExpression);
                methodArguments[i] = Expression.Convert(getArgValueCall, parameterType);
            }
            var body = Expression.Call(instanceArg, method, methodArguments);
            var lambda = Expression.Lambda<Func<T, IGameContext, JToken, Task>>(body, instanceArg, contextArg, tokenArg);
            return lambda.Compile();
        }

        private static Func<T, IGameContext, JToken, Task<TResult>> Compile<T, TResult>(MethodInfo method, ParameterInfo[] parameters)
        {
            var handler = typeof(T);
            var instanceArg = Expression.Parameter(handler, "handler");
            var contextArg = Expression.Parameter(typeof(IGameContext), "context");
            var tokenArg = Expression.Parameter(typeof(JToken), "token");
            var methodArguments = new Expression[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameterType = parameters[i].ParameterType;
                if (parameterType.IsByRef)
                {
                    throw new NotSupportedException();
                }
                if (parameterType == typeof(IGameContext))
                {
                    methodArguments[i] = contextArg;
                    continue;
                }
                var getArgParameterExpression = new Expression[]
                {
                    tokenArg,
                    Expression.Constant(parameters[i].Name,typeof(string)),
                    Expression.Constant(parameterType,typeof(Type))
                };
                var getArgValueCall = Expression.Call(_getArgValueMethod, getArgParameterExpression);
                methodArguments[i] = Expression.Convert(getArgValueCall, parameterType);
            }
            var body = Expression.Call(instanceArg, method, methodArguments);
            var lambda = Expression.Lambda<Func<T, IGameContext, JToken, Task<TResult>>>(body, instanceArg, contextArg, tokenArg);
            return lambda.Compile();
        }

        private static object GetArgValue(JToken token, string name, Type parameterType)
        {
            var property = token.As<JObject>()?.Property(name, StringComparison.OrdinalIgnoreCase);
            var s = Newtonsoft.Json.JsonSerializer.CreateDefault(new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            }.Action(s => s.Converters.Add(new UnixDateTimeConverter())));
            if (property != null)
            {
                return property.Value?.ToObject(parameterType, s);
            }
            return token?.ToObject(parameterType, s);
        }

        private static async Task WriteResult(IGameContext context, object result)
        {
            if (result == null)
            {
                await context.Response.WriteEmptyAsync();
            }
            else
            {
                await context.Response.WriteAsync(result);
            }
        }


    }
}
