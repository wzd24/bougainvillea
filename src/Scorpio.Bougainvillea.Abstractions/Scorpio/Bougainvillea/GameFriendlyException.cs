using System;

namespace Scorpio.Bougainvillea
{

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GameFriendlyException : Exception
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public GameFriendlyException(int code)
        {
            Code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public GameFriendlyException(int code, string message) : base(message)
        {
            Code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public GameFriendlyException(int code, string message, Exception inner) : base(message, inner)
        {
            Code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected GameFriendlyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        /// <summary>
        /// 
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// 
        /// </summary>
        public new object Data { get; set; }
    }
}
