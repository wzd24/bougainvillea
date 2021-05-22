namespace Scorpio.Bougainvillea.Handler
{
    /// <summary>
    /// 
    /// </summary>
    public class HandlerNotFoundException : GameFriendlyException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestCode"></param>
        public HandlerNotFoundException(string requestCode) : base(404, $"Handler {requestCode} not found")
        {
            RequestCode = requestCode;
        }

        /// <summary>
        /// 
        /// </summary>
        public string RequestCode { get; }
    }
}
