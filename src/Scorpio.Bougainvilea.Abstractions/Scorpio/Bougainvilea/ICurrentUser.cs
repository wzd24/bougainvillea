﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvilea
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICurrentUser
    {

        /// <summary>
        /// 
        /// </summary>
        string Token { get; }

        /// <summary>
        /// 
        /// </summary>
        int UserId { get; }

        /// <summary>
        /// 
        /// </summary>
        int Id { get; }

        /// <summary>
        /// 
        /// </summary>
        int ServerId { get; }
    }
}
