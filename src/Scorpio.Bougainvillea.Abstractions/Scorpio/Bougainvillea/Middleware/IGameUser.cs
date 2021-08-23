namespace Scorpio.Bougainvillea.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameUser
    {
        /// <summary>
        /// 
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 
        /// </summary>
        string Token { get; }

        /// <summary>
        /// 
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int ServerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool IsValid { get; }
    }
}
