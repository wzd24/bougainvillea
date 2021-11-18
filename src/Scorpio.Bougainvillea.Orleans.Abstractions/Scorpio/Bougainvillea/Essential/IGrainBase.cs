using System.Threading.Tasks;

using Orleans;

namespace Scorpio.Bougainvillea.Essential
{

    /// <summary>
    /// 
    /// </summary>
    public interface IGrainBase:IGrain
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task ReloadAsync();
    }
}