﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Depletion
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDepleteHandleManager
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depletion"></param>
        /// <param name="num"></param>
        /// <param name="reson"></param>
        /// <returns></returns>
        Task<int> HandleAsync(int[] depletion,int num,string reson);
    }
}
