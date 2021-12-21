using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Sailina.Tang.Essential.Dtos.Heros;
using Sailina.Tang.Essential.HeroSystem;
using Sailina.Tang.Essential.Settings;

using Scorpio.Bougainvillea.Essential;

namespace Sailina.Tang.Essential
{
    internal partial class Avatar : IAvatarHero
    {
        private HeroSubSystem HeroSubSystem => SubSystems.GetOrDefault(nameof(HeroSubSystem)) as HeroSubSystem;
    }

    internal partial class AvatarState
    {
        public HeroState Heros { get; set; } = new HeroState();

    }

    /// <summary>
    /// 
    /// </summary>
    public class HeroState : Dictionary<int, Hero>
    {
        /// <summary>
        /// 
        /// </summary>
        public HeroState()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        public HeroState(IDictionary<int, Hero> dictionary) : base(dictionary)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public HeroState(IEnumerable<KeyValuePair<int, Hero>> collection) : base(collection)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        public HeroState(IEqualityComparer<int> comparer) : base(comparer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        public HeroState(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="comparer"></param>
        public HeroState(IDictionary<int, Hero> dictionary, IEqualityComparer<int> comparer) : base(dictionary, comparer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comparer"></param>
        public HeroState(IEnumerable<KeyValuePair<int, Hero>> collection, IEqualityComparer<int> comparer) : base(collection, comparer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        public HeroState(int capacity, IEqualityComparer<int> comparer) : base(capacity, comparer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected HeroState(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Hero
    {
        /// <summary>
        /// 
        /// </summary>
        public Hero()
        {
            Skills = new Dictionary<int, int>();
            Skins = new Dictionary<int, int>();
            RewardPool = new Dictionary<FromType, Dictionary<int, long>>();
            Lv = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatarId"></param>
        /// <param name="heroSetting"></param>
        public Hero(long avatarId, HeroSetting heroSetting) : this()
        {
            AvatarId = avatarId;
            Id = heroSetting.Id;
            WearSkinId = heroSetting.DefaultSkin;
            Ability = heroSetting.Ability;
            Skills = heroSetting.Skills.ToDictionary(i => i, i => 1);
        }

        /// <summary>
        /// 名士ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 所属玩家ID
        /// </summary>
        public long AvatarId { get; set; }

        /// <summary>
        /// 当前等级
        /// </summary>
        public int Lv { get; set; }
        /// <summary>
        /// 研修等级
        /// </summary>
        public int StudyLv { get; set; }
        /// <summary>
        /// 升星等级
        /// </summary>
        public int StarLv { get; set; }
        /// <summary>
        /// 名士使用中的宠物（0代表善未添加使用中的宠物）
        /// </summary>
        public int PetId { get; set; }
        /// <summary>
        /// 穿戴中的服装Id，0为默认穿戴
        /// </summary>
        public int WearSkinId { get; set; }

        /// <summary>
        /// 技能
        /// </summary>
        public Dictionary<int, int> Skills { get; set; }

        /// <summary>
        /// 服装
        /// </summary>
        public Dictionary<int, int> Skins { get; set; }

        /// <summary>
        /// 添加的名士属性 Dictionary(来源类型, Dictionary(属性Id, BigInteger))
        /// </summary>
        public Dictionary<FromType, Dictionary<int, long>> RewardPool { get; set; }

        /// <summary>
        /// 资质
        /// </summary>
        public int Ability { get; set; }
        /// <summary>
        /// 财力
        /// </summary>
        public double Power { get; set; }
        /// <summary>
        /// 实力
        /// </summary>
        public double Hp { get; set; }
        /// <summary>
        /// 谋略
        /// </summary>
        public double Atk { get; set; }
        /// <summary>
        /// 经营
        /// </summary>
        public double Bias { get; set; }
        /// <summary>
        /// 名气
        /// </summary>
        public double Fame { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HeroInfo ToHeroInfo(HeroSetting setting)
        {
            var info = new HeroInfo(Id, Lv, StudyLv, StarLv)
            {
                PetId = PetId,
                WearSkinId = WearSkinId,
                Skills = Skills.Select(t => new SkillInfo() { Id = t.Key, Lv = t.Value }).ToList(),
                Skins = Skins.Select(t => new SkinInfo() { Id = t.Key, Lv = t.Value }).ToList(),
                Profession = setting.Profession,
                Quality = setting.Quality,
                ItemAddAttrs = RewardPool.GetOrDefault(FromType.Prop)?.Select(t => ((int)t.Key, (double)t.Value)).ToList() ?? new List<(int, double)>(),
                OtherAddAttrs = RewardPool.Where(t => t.Key != FromType.Prop).SelectMany(t => (t.Value)).GroupBy(t => t.Key).Select(g => ((int)g.Key, g.Sum(v => (double)v.Value))).ToList(),
            };
            return info;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HeroBaseInfo ToHeroBaseInfo(HeroSetting setting) => new HeroBaseInfo(Id, Lv, StudyLv, StarLv) { Profession = setting.Profession, Quality = setting.Quality };
    }

    /// <summary>
    /// 属性增量来源类型
    /// </summary>
    public enum FromType
    {
        /// <summary>
        /// 道具
        /// </summary>
        Prop = 1101,
        /// <summary>
        /// 奖励
        /// </summary>
        Reward = 1102,
        /// <summary>
        /// 子嗣授学
        /// </summary>
        KidTeaching = 1201,

        /// <summary>
        /// 游历事件
        /// </summary>
        Travel = 1801,

    }
}
