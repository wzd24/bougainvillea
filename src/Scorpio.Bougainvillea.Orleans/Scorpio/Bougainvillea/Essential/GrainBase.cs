﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Orleans;
using Orleans.Core;
using Orleans.Runtime;
using Orleans.Streams;

using Scorpio.Bougainvillea.Essential.Dtos;
using Scorpio.Bougainvillea.Setting;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GrainBase<TGrain> : Grain
        where TGrain:GrainBase<TGrain>
    {
        private static readonly MethodInfo _getPersistentState = typeof(GrainBase<>).GetMethod(nameof(GetPersistentState), BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// 
        /// </summary>
        protected IGameSettingManager GameSettingManager { get; }

        /// <summary>
        /// 
        /// </summary>
        protected ISettingManager SettingManager { get; }
        /// <summary>
        /// 
        /// </summary>
        protected GrainBase()
        {
            InitPersistentStates();
            GameSettingManager = ServiceProvider.GetService<IGameSettingManager>();
            SettingManager=ServiceProvider.GetService<ISettingManager>();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitPersistentStates()
        {
            var context = ServiceProvider.GetService<IGrainActivationContext>();
            var factory = ServiceProvider.GetService<IPersistentStateFactory>();
            var grainType = typeof(TGrain);
            var properties = grainType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var stateProperties = properties.Where(p => p.AttributeExists<PropertyPersistentStateAttribute>(true) && p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IPersistentState<>)).ToList();
            stateProperties.ForEach(p =>
            {
                var stateType = p.PropertyType.GenericTypeArguments.First();
                var method = _getPersistentState.MakeGenericMethod(stateType);
                var state = method.Invoke(null, new object[] { context, factory, p.GetAttribute<PropertyPersistentStateAttribute>(true) });
                p.SetValue(this, state);
            });
        }

        private static IPersistentState<TState> GetPersistentState<TState>(IGrainActivationContext context, IPersistentStateFactory factory, IPersistentStateConfiguration configuration)
        {
            
            return factory.Create<TState>(context, configuration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal new IStreamProvider GetStreamProvider(string name)
        {
            return base.GetStreamProvider(name);
        }


    }
}