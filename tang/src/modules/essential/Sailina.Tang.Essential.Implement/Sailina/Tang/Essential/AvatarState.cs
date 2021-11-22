
using Scorpio.Bougainvillea.Essential;

namespace Sailina.Tang.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AvatarState:AvatarStateBase<AvatarEntity>
    {

    }


    /// <summary>
    /// 
    /// </summary>
    public class AvatarEntity : AvatarEntityBase
    {
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
