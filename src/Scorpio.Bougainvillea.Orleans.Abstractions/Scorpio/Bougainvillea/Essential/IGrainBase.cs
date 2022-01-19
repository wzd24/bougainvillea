using System.Threading.Tasks;

using Orleans;
using Orleans.Concurrency;

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
        [OneWay]
        ValueTask ReloadAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        ValueTask<object> GetStateDataAsync(string stateName);

    }
}