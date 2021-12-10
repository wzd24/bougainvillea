using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sailina.Tang.Essential.Dtos.Heros
{
    /// <summary>
    /// 
    /// </summary>
    public class HeroInfo : HeroBaseInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lv"></param>
        /// <param name="studyLv"></param>
        /// <param name="starLv"></param>
        /// <param name="profession"></param>
        /// <param name="quality"></param>
        public HeroInfo(int id, int lv, int studyLv, int starLv) : base(id, lv, studyLv, starLv)
        {
            Skills = new List<SkillInfo>();
            Skins = new List<SkinInfo>();
            ItemAddAttrs = new List<(int, double)>();
            OtherAddAttrs = new List<(int, double)>();
        }

        /// <summary>
        /// 装备宠物Id
        /// </summary>
        public int PetId { get; set; }

        /// <summary>
        /// 技能组Id
        /// </summary>
        public List<SkillInfo> Skills { get; set; }
        /// <summary>
        /// 服装
        /// </summary>
        public List<SkinInfo> Skins { get; set; }
        /// <summary>
        /// 道具加成
        /// </summary>
        public List<(int, double)> ItemAddAttrs { get; set; }
        /// <summary>
        /// 其它加成
        /// </summary>
        public List<(int, double)> OtherAddAttrs { get; set; }
        /// <summary>
        /// 当前穿戴
        /// </summary>
        public int WearSkinId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HeroInfo4Manager : HeroBaseInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lv"></param>
        /// <param name="studyLv"></param>
        /// <param name="starLv"></param>
        public HeroInfo4Manager(int id, int lv, int studyLv, int starLv) : base(id, lv, studyLv, starLv)
        {
            Skills = new List<SkillInfo>();
            Skins = new List<SkinInfo>();
        }
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
        /// 技能组Id
        /// </summary>
        public List<SkillInfo> Skills { get; set; }
        /// <summary>
        /// 服装
        /// </summary>
        public List<SkinInfo> Skins { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HeroInfo4Guild
    {
        /// <summary>
        /// 
        /// </summary>
        public HeroInfo4Guild() { }

        /// <summary>
        /// 名士Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 财力
        /// </summary>
        public double Power { get; set; }
    }
}
