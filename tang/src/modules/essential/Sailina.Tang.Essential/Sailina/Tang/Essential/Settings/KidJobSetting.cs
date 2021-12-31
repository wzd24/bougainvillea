using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Setting;

namespace Sailina.Tang.Essential.Settings
{
    /// <summary>
    /// 子嗣职业
    /// </summary>
    [Serializable]
    public class KidJobSetting:GameSettingBase
    {
        /// <summary>
        /// 职业限定
        /// </summary>
        public Profession Job { get; set; }

        /// <summary>
        /// 性别限定
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 工作加成
        /// </summary>

        public List<int> JobAddtion{get; set; }
        /// <summary>
        /// 是否孤独终老
        /// </summary>
        public bool DieAlone { get; set; }
        /// <summary>
        /// 职业随机权重
        /// </summary>
        public int AppearanceWeight { get; set; }

        /// <summary>
        /// 是否有状元贴
        /// </summary>

        public int TopPrize { get; set; }
    }
}
