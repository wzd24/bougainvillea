using System;

namespace Scorpio.Bougainvillea.Tokens
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class UserData
    {
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset TimeStamp { get; set; }
    }
}