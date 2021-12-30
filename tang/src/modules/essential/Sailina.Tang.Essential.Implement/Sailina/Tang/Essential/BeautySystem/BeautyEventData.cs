using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sailina.Tang.Essential.HeroSystem;

namespace Sailina.Tang.Essential.BeautySystem
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="BeautyId">美女Id</param>
    /// <param name="Reason">事件发生原因</param>
    public record BeautyEventData(int BeautyId, string Reason);

    /// <summary>
    /// 获得美女事件
    /// </summary>
    /// <param name="BeautyId">美女Id</param>
    /// <param name="Reason">事件发生原因</param>
    public record BeautyAddEventData(int BeautyId, string Reason) : BeautyEventData(BeautyId, Reason);

    /// <summary>
    /// 美名提升事件
    /// </summary>
    /// <param name="BeautyId">美女Id</param>
    /// <param name="Reason">事件发生原因</param>
    /// <param name="AddValue">事件添加值</param>
    /// <param name="Lv">事件发生后美名等级</param>
    public record BeautyTitleUpgradeEventData(int BeautyId, string Reason, int AddValue, int Lv) : BeautyEventData(BeautyId, Reason);

    /// <summary>
    /// 美女升星事件
    /// </summary>
    /// <param name="BeautyId">美女Id</param>
    /// <param name="Reason">事件发生原因</param>
    /// <param name="AddValue">事件添加值</param>
    /// <param name="Lv">事件发生后美女星级</param>
    public record BeautyStarUpgradeEventData(int BeautyId, string Reason, int AddValue, int Lv) : BeautyEventData(BeautyId, Reason);

    ///<summary>
    /// 美女皮肤升级事件
    /// </summary>
    /// <param name="BeautyId">美女Id</param>
    /// <param name="Reason">事件发生原因</param>
    /// <param name="SkinId">皮肤Id</param>
    /// <param name="AddValue">事件添加值</param>
    /// <param name="Lv">事件发生后美女星级</param>
    public record BeautySkinUpgradeEventData(int BeautyId, int SkinId, string Reason, int AddValue, int Lv) : BeautyEventData(BeautyId, Reason);

    /// <summary>
    /// 美女更换皮肤事件数据
    /// </summary>
    /// <param name="BeautyId">美女Id</param>
    /// <param name="Reason">事件发生原因</param>
    /// <param name="Before">更换前皮肤Id</param>
    /// <param name="After">更换后皮肤Id</param>

    public record BeautySkinWearEventData(int BeautyId, string Reason, int Before, int After) : BeautyEventData(BeautyId, Reason);

    /// <summary>
    /// 美女升级技能事件数据
    /// </summary>
    /// <param name="BeautyId">美女Id</param>
    /// <param name="Reason">事件发生原因</param>
    /// <param name="SkillId">技能Id</param>
    /// <param name="AddValue">事件添加值</param>
    /// <param name="Level">事件发生后美女技能等级</param>
    public record BeautySkillUpgradeEventData(int BeautyId, string Reason, int SkillId, int AddValue, int Level) : BeautyEventData(BeautyId, Reason);

    /// <summary>
    /// 美女升级技能事件数据
    /// </summary>
    /// <param name="BeautyId">美女Id</param>
    /// <param name="Reason">事件发生原因</param>
    /// <param name="PropId">道具Id</param>
    /// <param name="AddValue">事件添加值</param>
    public record BeautyPresentingEventData(int BeautyId, string Reason, int PropId, int AddValue) : BeautyEventData(BeautyId, Reason);

}
