using System;
using System.Collections.Generic;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ServerInfo : IEqualityComparer<ServerInfo>, IComparable<ServerInfo>
    {
        /// <summary>
        /// 
        /// </summary>
        public int ServerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsVisiable { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public bool CanRegister { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public ServerStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ServerTag Tags { get; set; } = ServerTag.Noarmal;

        /// <summary>
        /// 
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public DateTime OpenTime { get; set; } = DateTime.Now.AddDays(7);

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastMaintenanceDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan  ServerTimeOffset { get; set; }=TimeSpan.Zero;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => HashCode.Combine(ServerId);
        int IComparable<ServerInfo>.CompareTo(ServerInfo other) => ServerId.CompareTo(other.ServerId);
        bool IEqualityComparer<ServerInfo>.Equals(ServerInfo x, ServerInfo y) => x.ServerId.Equals(y.ServerId);
        int IEqualityComparer<ServerInfo>.GetHashCode(ServerInfo obj) => obj.GetHashCode();
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ServerStatus
    {
        /// <summary>
        /// 待开区
        /// </summary>
        AwaitOpen = 0,
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 维护
        /// </summary>
        Maintenance = 2,
        /// <summary>
        /// 停服
        /// </summary>
        Closed = 3,


    }

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum ServerTag
    {
        /// <summary>
        /// 正常
        /// </summary>
        Noarmal = 0,
        /// <summary>
        /// 新服
        /// </summary>
        NewServer = 1,
        /// <summary>
        /// 火爆
        /// </summary>
        HotServer = 2,

        /// <summary>
        /// 繁忙
        /// </summary>
        Busy = 4
    }
}
