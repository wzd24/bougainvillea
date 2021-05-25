using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Props
{
    /// <summary>
    /// 道具相关错误代码。
    /// </summary>
    public static class PropsErrorCodes
    {
        /// <summary>
        /// 错误编码(描述)：180001(道具数量不足)
        /// </summary>
        public static int NotEnough = 180001;

        /// <summary>
        /// 错误编码(描述)：180002(道具不存在)
        /// </summary>
        public static int NotExist = 180002;

        /// <summary>
        /// 错误编码(描述)：180003(您还没有这个道具)
        /// </summary>
        public static int NotHave = 180003;

        /// <summary>
        /// 错误编码(描述)：180004(道具不可使用)
        /// </summary>
        public static int NotCanUse = 180004;

        /// <summary>
        /// 错误编码(描述)：180005(道具参数错误)
        /// </summary>
        public static int ExceptionParameter = 180005;

        /// <summary>
        /// 错误编码(描述)：180006(使用失败，或未实现该道具实现)
        /// </summary>
        public static int UseFail = 180006;

        #region 合成相关
        /// <summary>
        /// 错误编码(描述)：181001(合成异常,不存在该合成)
        /// </summary>
        public static int ExceptionCompound = 181001;
        #endregion
    }
}
