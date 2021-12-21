using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans.CodeGeneration;

using Scorpio.Bougainvillea.Essential;

namespace Sailina.Tang.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public class Game : GameBase<Game>, IGame
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="baseGrainProvider"></param>
        public Game(IServiceProvider serviceProvider, IBaseGrainProvider baseGrainProvider) : base(serviceProvider, baseGrainProvider)
        {
        }
    }
}
