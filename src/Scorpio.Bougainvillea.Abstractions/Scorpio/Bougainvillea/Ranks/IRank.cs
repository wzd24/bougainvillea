using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Ranks
{

    /// <summary>
    /// 
    /// </summary>
    public interface IRank
    {
       /// <summary>
       /// 
       /// </summary>
       /// <param name="item"></param>
        void SetRank(RankItem item);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<RankItem> GetRanks(int count);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        (int rank, RankItem data) GetRank(RankItem item);
    }
}
