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
        /// 基础错误代码
        /// </summary>
        public static int BaseCode { get; set; } = 180000;
        /// <summary>
        /// 错误编码(描述)：180001(道具数量不足)
        /// </summary>
        public static int NotEnough = BaseCode+1;

        /// <summary>
        /// 错误编码(描述)：180002(道具不存在)
        /// </summary>
        public static int NotExist = BaseCode+2;

        /// <summary>
        /// 错误编码(描述)：180003(您还没有这个道具)
        /// </summary>
        public static int NotHave = BaseCode+3;

        /// <summary>
        /// 错误编码(描述)：180004(道具不可使用)
        /// </summary>
        public static int NotCanUse = BaseCode+4;

        /// <summary>
        /// 错误编码(描述)：180005(道具参数错误)
        /// </summary>
        public static int ExceptionParameter = BaseCode+5;

        /// <summary>
        /// 错误编码(描述)：180006(使用失败，或未实现该道具实现)
        /// </summary>
        public static int UseFail = BaseCode + 6;

        #region 合成相关
        /// <summary>
        /// 错误编码(描述)：181001(合成异常,不存在该合成)
        /// </summary>
        public static int ExceptionCompound = BaseCode + 101;
        #endregion
    }
}
