namespace Scorpio.Bougainvillea.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public interface IResponseMessage
    {
        /// <summary>
        /// 
        /// </summary>
        int Code { get; }

        /// <summary>
        /// 
        /// </summary>
        string Message { get; }

        /// <summary>
        /// 
        /// </summary>
        object Data { get; }
    }
}
