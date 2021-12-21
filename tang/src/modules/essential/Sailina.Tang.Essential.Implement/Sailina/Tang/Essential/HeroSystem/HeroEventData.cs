using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailina.Tang.Essential.HeroSystem
{
    /// <summary>
    /// 
    /// </summary>
    public record HeroEventData(int HeroId,string Reason);

    /// <summary>
    /// 获得名士事件数据
    /// </summary>
    public record HeroAddEventData(int HeroId,int AddValue, string Reason) : HeroEventData(HeroId,Reason);

    /// <summary>
    /// 名士升级事件数据
    /// </summary>
    public record HeroUpgradeEventData(int HeroId, int AddValue, int Level, string Reason) : HeroEventData(HeroId, Reason);
    /// <summary>
    /// 名士研修事件数据
    /// </summary>
    public record HeroStudyUpgradeEventData(int HeroId, int AddValue, int Level, string Reason) : HeroEventData(HeroId, Reason);
    /// <summary>
    /// 名士升星事件数据
    /// </summary>
    public record HeroStarUpgradeEventData(int HeroId, int AddValue, int Level, string Reason) : HeroEventData(HeroId, Reason);
    /// <summary>
    /// 名士升级技能事件数据
    /// </summary>
    public record HeroSkillUpgradeEventData(int HeroId,int SkillId, int AddValue, int Level, string Reason) : HeroEventData(HeroId, Reason);
}
