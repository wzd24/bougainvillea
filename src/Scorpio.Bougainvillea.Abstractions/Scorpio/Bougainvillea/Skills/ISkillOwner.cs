using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Skills
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISkillOwner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ICollection<Skill>> GetSkillsAsync();
    }
}
