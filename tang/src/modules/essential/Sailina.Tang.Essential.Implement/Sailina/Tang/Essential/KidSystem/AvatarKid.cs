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

using Sailina.Tang.Essential.KidSystem;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Data;

using static Dapper.SqlMapper;

namespace Sailina.Tang.Essential
{
    internal partial class Avatar : IAvatarKid
    {
        private KidSubSystem KidSubSystem => SubSystems.GetOrDefault(nameof(KidSubSystem)) as KidSubSystem;

    }

    internal partial class AvatarState
    {
        public KidState Kids { get; set; } = new KidState();

    }

    internal class KidState : Dictionary<int, Kid>
    {
        public KidState()
        {
        }

        public KidState(IDictionary<int, Kid> dictionary) : base(dictionary)
        {
        }

        public KidState(IEnumerable<KeyValuePair<int, Kid>> collection) : base(collection)
        {
        }

        public KidState(IEqualityComparer<int> comparer) : base(comparer)
        {
        }


        public KidState(int capacity) : base(capacity)
        {
        }

        public KidState(IDictionary<int, Kid> dictionary, IEqualityComparer<int> comparer) : base(dictionary, comparer)
        {
        }

        public KidState(IEnumerable<KeyValuePair<int, Kid>> collection, IEqualityComparer<int> comparer) : base(collection, comparer)
        {
        }

        public KidState(int capacity, IEqualityComparer<int> comparer) : base(capacity, comparer)
        {
        }

        protected KidState(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }


        public KidMisc Misc { get; init; } = new KidMisc();

        internal static async ValueTask<KidState> InitializeAsync(GridReader r)
        {
            return new KidState((await r.ReadAsync<Kid>()).Select(b => b.GenerateProxy()).ToDictionary(m => m.Id, m => m))
            {
                Misc = (await r.ReadSingleOrDefaultAsync<KidMisc>()).GenerateProxy()
            };
        }

        internal async ValueTask WriteAsync(IDbConnection connection)
        {
            var items = Values.Where(p => !(p is IModifiable { Modified: false })).ToArray();
            await connection.InsertOrUpdateAsync<Kid>(items);
            if (!(Misc is IModifiable { Modified: false }))
            {
                await connection.InsertOrUpdateAsync<KidMisc>(Misc);
            }
        }
    }


    internal class KidMisc
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
        public virtual List<int> AssignCHKidIds { get; set; }

    }

    internal class Kid
    {
        public Kid()
        {
            FateSkills = new Dictionary<int, int>();
            Skins = new Dictionary<int, int>();
            ShopSkills = new Dictionary<int, int>();
        }

        //public Kid(long avatarId, KidSetting KidSetting)
        //{
        //    Id = KidSetting.Id;
        //    AvatarId = avatarId;
        //    FateSkills = KidSetting.FateSkills.ToDictionary(i => i, i => 1);
        //    ShopSkills = KidSetting.ShopSkills.ToDictionary(i => i, i => 1);
        //    Skins.AddOrUpdate(KidSetting.DefaultSkin, k => 1);
        //    WearSkinId = KidSetting.DefaultSkin;
        //    TitleLv = 1;
        //}
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
