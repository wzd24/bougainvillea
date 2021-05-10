using System;

namespace Scorpio.Bougainvillea.Handler
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HandlerAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public HandlerAttribute(string code)
        {
            Code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
