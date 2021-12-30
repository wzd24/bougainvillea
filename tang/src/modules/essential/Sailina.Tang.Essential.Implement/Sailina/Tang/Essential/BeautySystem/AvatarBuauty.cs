using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using Dapper.Extensions;

using EasyMigrator;

using Sailina.Tang.Essential.BeautySystem;
using Sailina.Tang.Essential.HeroSystem;
using Sailina.Tang.Essential.Settings;

using Scorpio.Bougainvillea.Data;

using static Dapper.SqlMapper;

namespace Sailina.Tang.Essential
{
    internal partial class Avatar : IAvatarBeauty
    {
        private BeautySubSystem BeautySubSystem => SubSystems.GetOrDefault(nameof(BeautySubSystem)) as BeautySubSystem;

    }

    internal partial class AvatarState
    {
        public BeautyState Beauties { get; set; } = new BeautyState();

    }

    internal class BeautyState : Dictionary<int, Beauty>
    {
        public BeautyState()
        {
        }

        public BeautyState(IDictionary<int, Beauty> dictionary) : base(dictionary)
        {
        }

        public BeautyState(IEnumerable<KeyValuePair<int, Beauty>> collection) : base(collection)
        {
        }

        public BeautyState(IEqualityComparer<int> comparer) : base(comparer)
        {
        }


        public BeautyState(int capacity) : base(capacity)
        {
        }

        public BeautyState(IDictionary<int, Beauty> dictionary, IEqualityComparer<int> comparer) : base(dictionary, comparer)
        {
        }

        public BeautyState(IEnumerable<KeyValuePair<int, Beauty>> collection, IEqualityComparer<int> comparer) : base(collection, comparer)
        {
        }

        public BeautyState(int capacity, IEqualityComparer<int> comparer) : base(capacity, comparer)
        {
        }

        protected BeautyState(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }


        public BeautyMisc Misc { get; init; } = new BeautyMisc();

        internal static async ValueTask<BeautyState> InitializeAsync(GridReader r)
        {
            return new BeautyState((await r.ReadAsync<Beauty>()).Select(b => b.GenerateProxy()).ToDictionary(m => m.Id, m => m))
            {
                Misc = (await r.ReadSingleOrDefaultAsync<BeautyMisc>()).GenerateProxy()
            };
        }

        internal async ValueTask WriteAsync(IDbConnection connection)
        {
            var items = Values.Where(p => !(p is IModifiable { Modified: false })).ToArray();
            await connection.InsertOrUpdateAsync<Beauty>(items);
            if (!(Misc is IModifiable { Modified: false }))
            {
                await connection.InsertOrUpdateAsync<BeautyMisc>(Misc);
            }
        }
    }


    internal class BeautyMisc
    {
        /// <summary>
        /// 所属玩家Id
        /// </summary>
        [ExplicitKey]
        [Pk]
        public virtual int AvatarId { get; set; }
        /// <summary>
        /// 传唤赴约生子嗣次数
        /// </summary>
        public virtual int ChuanHuanKidTimes { get; set; }
        /// <summary>
        /// 剩余必得双胞胎次数
        /// </summary>
        public virtual int MustTwinsNum { get; set; }
        /// <summary>
        /// 玩家精力值
        /// </summary>
        public virtual int EnergyNum { get; set; }
        /// <summary>
        /// 美女倒计时开始时间
        /// </summary>
        public virtual long EnergyStartTime { get; set; }
        /// <summary>
        /// 美女好感度
        /// </summary>
        [DbType(DbType.String), Max, Default(null)]
        public virtual Dictionary<int, int> FavorDic { get; set; }
        /// <summary>
        /// 必定传唤美女列表
        /// </summary>
        [DbType(DbType.String), Max, Default(null)]
        public virtual List<int> AssignCHBeautyIds { get; set; }

    }

    internal class Beauty
    {
        public Beauty()
        {
            FateSkills = new Dictionary<int, int>();
            Skins = new Dictionary<int, int>();
            ShopSkills = new Dictionary<int, int>();
        }

        public Beauty(long avatarId, BeautySetting beautySetting)
        {
            Id = beautySetting.Id;
            AvatarId = avatarId;
            FateSkills = beautySetting.FateSkills.ToDictionary(i => i, i => 1);
            ShopSkills = beautySetting.ShopSkills.ToDictionary(i => i, i => 1);
            Skins.AddOrUpdate(beautySetting.DefaultSkin, k => 1);
            WearSkinId = beautySetting.DefaultSkin;
            TitleLv = 1;
        }
        /// <summary>
        /// 情缘ID
        /// </summary>
        [ExplicitKey]
        [Pk]
        public virtual int Id { get; set; }

        /// <summary>
        /// 所属玩家ID
        /// </summary>
        [ExplicitKey]
        [Pk]
        [Index]
        public virtual long AvatarId { get; set; }

        /// <summary>
        /// 美名等级
        /// </summary>
        public virtual int TitleLv { get; set; }
        /// <summary>
        /// 升星等级
        /// </summary>
        public virtual int StarLv { get; set; }
        /// <summary>
        /// 通过计算加成后的亲密度，包括道具或奖励的部分
        /// </summary>
        public virtual int TotalQinMi { get; set; }
        /// <summary>
        /// 通过计算加成后的魅力值，包括道具或奖励的部分
        /// </summary>
        public virtual int TotalMeiLi { get; set; }

        /// <summary>
        /// 添加的情缘属性 Dictionary(来源类型, Dictionary(属性Id, BigInteger))
        /// </summary>
        [DbType(DbType.String), Max, Default(null)]
        public virtual Dictionary<FromType, Dictionary<int, long>> RewardPool { get; set; }

        /// <summary>
        /// 缘分值
        /// </summary>
        public virtual int Fate { get; set; }
        /// <summary>
        /// 已出游次数
        /// </summary>
        public virtual int ChuYouTimes { get; set; }
        /// <summary>
        /// 已沐浴次数
        /// </summary>
        public virtual int MuYuTimes { get; set; }
        /// <summary>
        /// 已穿戴服装Id
        /// </summary>
        public virtual int WearSkinId { get; set; }
        /// <summary>
        /// 子嗣数量
        /// </summary>
        public virtual int TotalKids { get; set; }
        /// <summary>
        /// 获得迎娶美女时间(秒级)
        /// </summary>
        public virtual long MarryTime { get; set; }

        /// <summary>
        /// 服装
        /// </summary>
        [DbType(DbType.String), Max, Default(null)]
        public virtual Dictionary<int, int> Skins { get; set; }
        /// <summary>
        /// 门客缘分技能集合
        /// </summary>
        [DbType(DbType.String), Max, Default(null)]
        public virtual Dictionary<int, int> FateSkills { get; set; }
        /// <summary>
        /// 商铺技能
        /// </summary>
        [DbType(DbType.String), Max, Default(null)]
        public virtual Dictionary<int, int> ShopSkills { get; set; }

    }
}
