
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;

using Castle.DynamicProxy;

using Dapper;
using Dapper.Extensions;

using EasyMigrator;

using Scorpio.Bougainvillea.Data;
using Scorpio.Bougainvillea.Essential;

using static Dapper.SqlMapper;

namespace Sailina.Tang.Essential
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    internal partial class AvatarState : AvatarStateBase<AvatarBaseInfo>
    {
        public static async ValueTask<AvatarState> InitializeAsync(GridReader dataReader)
        {
            var avatar = new AvatarState
            {
                Base = await AvatarBaseInfo.InitializeAsync(dataReader),
                HotData = await AvatarHotData.InitializeAsync(dataReader),
                ColdData = await AvatarColdData.InitializeAsync(dataReader),
                Currency = await AvatarCurrency.InitializeAsync(dataReader),
                Props = await PropsState.InitializeAsync(dataReader),
                Heros = await HeroState.InitializeAsync(dataReader),
                Beauties = await BeautyState.InitializeAsync(dataReader)
            };
            return avatar.Base == null ? null : avatar;
        }

        internal async ValueTask WriteAsync(IDbConnection conn)
        {
            await Base.WriteAsync(conn);
            await HotData.WriteAsync(conn);
            await ColdData.WriteAsync(conn);
            await Currency.WriteAsync(conn);
            await Props.WriteAsync(conn);
            await Heros.WriteAsync(conn);
            await Beauties.WriteAsync(conn);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [Name("AvatarBase")]
    internal class AvatarBaseInfo : AvatarBaseEntityBase
    {
        public AvatarBaseInfo()
        {
        }
        /// <summary>
        /// 已解锁头像ID列表
        /// </summary>
        [DbType(DbType.String),Max, Default(null)]
        public virtual List<int> HeadIds { get; set; } = new List<int>();

        /// <summary>
        /// 已解锁时装 key 时装ID value 等级
        /// </summary>
        [DbType(DbType.String), Max, Default(null)]
        public virtual Dictionary<int, int> FashionIds { get; set; } = new Dictionary<int, int>();

        /// <summary>
        /// 已解锁头像框ID列表 key 头像框ID value 过期时间戳
        /// </summary>
        [DbType(DbType.String), Max, Default(null)]
        public virtual Dictionary<int, long> HeadFrameIds { get; set; } = new Dictionary<int, long>();


        /// <summary>
        /// 当前穿戴称号ID
        /// </summary>
        public virtual int TitleId { get; set; }

        /// <summary>
        /// 当前穿戴时装ID
        /// </summary>
        public virtual int FashionId { get; set; }

        /// <summary>
        /// 地址ID 默认配置地址取杂项配置1100000
        /// </summary>
        public virtual int AddressId { get; set; }

        /// <summary>
        /// 剧情关卡
        /// </summary>
        public virtual int PlotLevel { get; set; }

        /// <summary>
        /// 地址修改状态
        /// </summary>
        public virtual bool AddressChangeState { get; set; }

        public static async ValueTask<AvatarBaseInfo> InitializeAsync(GridReader dataReader)
        {
            return (await dataReader.ReadSingleOrDefaultAsync<AvatarBaseInfo>())?.GenerateProxy();
        }

        internal async ValueTask WriteAsync(IDbConnection conn) => await this.Action(!(this is IModifiable { Modified: false }), async a => await conn.InsertOrUpdateAsync<AvatarBaseInfo>(this));
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
            return methodInfo.IsSpecialName && !methodInfo.Name.Contains(nameof(IModifiable.Modified)) && (methodInfo.Name.StartsWith("set_")|| methodInfo.Name.StartsWith("get_"));
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
                if (property != null && !property.GetValue(invocation.Proxy, null).Equals( invocation.Arguments.First()))
                {
                    (invocation.Proxy as IModifiable).SetModifyState();
                }
            }
            invocation.Proceed();
        }

    }

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

    internal class Modifiable : IModifiable
    {
        public bool Modified { get; set; }

        public void ResetModifyState() => Modified = false;

        /// <summary>
        /// 
        /// </summary>
        public void SetModifyState() => Modified = true;

    }

}
