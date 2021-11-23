
using Scorpio.Bougainvillea.Essential;

namespace Sailina.Tang.Essential
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public partial class AvatarState:AvatarStateBase<AvatarBase>
    {

    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AvatarBase : AvatarBaseEntityBase
    {
        /// <summary>
        /// 当前穿戴称号ID
        /// </summary>
        public int TitleId { get; set; }

        /// <summary>
        /// 当前穿戴时装ID
        /// </summary>
        public int FashionId { get; set; }

        /// <summary>
        /// 地址ID 默认配置地址取杂项配置1100000
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// 剧情关卡
        /// </summary>
        public int PlotLevel { get; set; }

        /// <summary>
        /// 地址修改状态
        /// </summary>
        public bool AddressChangeState { get; set; }
    }
}
