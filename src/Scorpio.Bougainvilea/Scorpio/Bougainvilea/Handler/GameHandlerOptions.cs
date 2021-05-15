﻿using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json.Linq;

using Scorpio.Bougainvillea.Middleware;

using Scorpio.Threading;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Handler
{
    /// <summary>
    /// 
    /// </summary>
    public class GameHandlerOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string MethodName = "Execute";

        delegate Task<object> InvokeDelegate(IGameContext gameContext);
        delegate Task<TResult> InvokeDelegate<TResult>(IGameContext gameContext);

        private readonly List<Action<IGameHandlerFactory>> _handlers;
        private static MethodInfo _getArgValueMethod = typeof(GameHandlerOptions).GetMethod(nameof(GetArgValue), BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo _addMethodMethod = typeof(GameHandlerOptions).GetMethod(nameof(AddMethod), BindingFlags.NonPublic | BindingFlags.Instance);
        private static MethodInfo _addMethodMethodVoid = typeof(GameHandlerOptions).GetMethod(nameof(AddMethodVoid), BindingFlags.NonPublic | BindingFlags.Instance);
        /// <summary>
        /// 
        /// </summary>
        public GameHandlerOptions()
        {
            _handlers = new List<Action<IGameHandlerFactory>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="code"></param>
        /// <param name="handler"></param>
        public void Add<TResult>(string code, Func<IGameContext, Task<TResult>> handler)
        {
            async Task wrapped(IGameContext context)
            {
                var result = await (handler(context));
                await WriteResult(context, result);
            }
            AddHandler(code, wrapped);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="args"></param>
        public void AddType<THandler>(params object[] args)
        {
            AddType(typeof(THandler), args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        public void AddType(Type type, params object[] args)
        {
            var attribute = type.GetAttribute<HandlerAttribute>();
            if (attribute == null)
            {
                throw new InvalidOperationException();
            }
            AddType(attribute.Code, type, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="code"></param>
        /// <param name="args"></param>
        public void AddType<THandler>(string code, params object[] args)
        {
            AddType(code, typeof(THandler), args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <param name="code"></param>
        public void AddType(string code, Type type, params object[] args)
        {
            var method = GetMethod(type);
            var addMethod = 
                method.ReturnType.UnwrapTask() == typeof(void) ? 
                _addMethodMethodVoid.MakeGenericMethod(type) : 
                _addMethodMethod.MakeGenericMethod(type, method.ReturnType.UnwrapTask());
            addMethod.Invoke(this, new object[] { method, code, args });
        }

        internal void ApplyHanders(DefaultGameHandlerFactory defaultGameHandlerFactory) => _handlers.ForEach(action => action(defaultGameHandlerFactory));

        private void AddMethod<T, TResult>(MethodInfo method, string code, params object[] args)
        {
            _handlers.Add(factory =>
            {
                var parameters = method.GetParameters();
                var instance = factory.ServiceProvider.GetService<T>() ?? ActivatorUtilities.CreateInstance<T>(factory.ServiceProvider, args);
                switch (parameters.Length)
                {
                    case 0:
                        {
                            var @delegate = (Func<Task<TResult>>)method.CreateDelegate(typeof(Func<Task<TResult>>), instance);
                            async Task wrapped(IGameContext context)
                            {
                                var result = await (@delegate());
                                await WriteResult(context, result);
                            }
                            factory.AddHandler(code, wrapped);
                            break;
                        }
                    case 1 when parameters[0].ParameterType == typeof(IGameContext):
                        {
                            var @delegate = (InvokeDelegate<TResult>)method.CreateDelegate(typeof(InvokeDelegate<TResult>), instance);
                            async Task wrapped(IGameContext context)
                            {
                                var result = await (@delegate(context));
                                await WriteResult(context, result);
                            }
                            factory.AddHandler(code, wrapped);

                            break;
                        }
                    default:
                        {
                            GenerateDelegate<T, TResult>(code, factory, method, parameters, instance);
                            break;
                        }
                }
            });
        }

        private void AddMethodVoid<T>(MethodInfo method, string code, params object[] args)
        {
            _handlers.Add(factory =>
            {
                var parameters = method.GetParameters();
                var instance = factory.ServiceProvider.GetService<T>() ?? ActivatorUtilities.CreateInstance<T>(factory.ServiceProvider, args);
                switch (parameters.Length)
                {
                    case 0:
                        {
                            var @delegate = (Func<Task>)method.CreateDelegate(typeof(Func<Task>), instance);
                            async Task wrapped(IGameContext context)
                            {
                                await (@delegate());
                                await context.Response.WriteEmptyAsync();
                            }
                            factory.AddHandler(code, wrapped);
                            break;
                        }
                    case 1 when parameters[0].ParameterType == typeof(IGameContext):
                        {
                            var @delegate = (Func<IGameContext, Task>)method.CreateDelegate(typeof(Func<IGameContext, Task>), instance);
                            async Task wrapped(IGameContext context)
                            {
                                await (@delegate(context));
                                await context.Response.WriteEmptyAsync();
                            }
                            factory.AddHandler(code, wrapped);

                            break;
                        }
                    default:
                        {
                            GenerateDelegate<T>(code, factory, method, parameters, instance);
                            break;
                        }
                }
            });
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

        private MethodInfo GetMethod(Type type)
        {
            var method = type.GetMethod(MethodName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method == null)
            {
                method = type.GetMethod($"{MethodName}Async", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
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
        }


        private static void GenerateDelegate<T, TResult>(string code, IGameHandlerFactory gameHandlerFactory, MethodInfo method, ParameterInfo[] parameters, T instance)
        {
            var facory = Compile<T, TResult>(method, parameters);
            gameHandlerFactory.AddHandler(code, async context =>
            {
                var token = JToken.Parse(context.Request.Content);
                var result = await facory(instance, context, token);
                if (result == null)
                {
                    await context.Response.WriteEmptyAsync();
                }
                else
                {
                    await context.Response.WriteAsync(result);
                }
            });
        }

        private static void GenerateDelegate<T>(string code, IGameHandlerFactory gameHandlerFactory, MethodInfo method, ParameterInfo[] parameters, T instance)
        {
            var facory = Compile<T>(method, parameters);
            gameHandlerFactory.AddHandler(code, async context =>
            {
                var token = JToken.Parse(context.Request.Content);
                await facory(instance, context, token);
                await context.Response.WriteEmptyAsync();
            });
        }

        private static Func<T, IGameContext, JToken, Task> Compile<T>(MethodInfo method, ParameterInfo[] parameters)
        {
            var handler = typeof(T);
            var instanceArg = Expression.Parameter(handler, "handler");
            var contextArg = Expression.Parameter(typeof(IGameContext), "context");
            var tokenArg = Expression.Parameter(typeof(JToken), "token");
            var methodArguments = new Expression[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
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
            for (int i = 0; i < parameters.Length; i++)
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
            if (property != null)
            {
                return property.Value?.ToObject(parameterType);
            }
            return token?.ToObject(parameterType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="handler"></param>
        public void AddHandler(string code, GameHandleDelegate handler)
        {
            _handlers.Add(factory => factory.AddHandler(code, handler));
        }
    }
}
