using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Skills
{
    /// <summary>
    /// 技能加成目标对象接口
    /// </summary>
    public interface ISkillTarget
    {
        /// <summary>
        /// 添加技能加成值
        /// </summary>
        /// <param name="value"></param>
        void AddSkillBuffValue(SkillBuffValue value);
    }
}
