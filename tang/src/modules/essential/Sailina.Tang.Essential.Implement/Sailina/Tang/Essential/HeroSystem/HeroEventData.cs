using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailina.Tang.Essential.HeroSystem
{
    /// <summary>
    /// 
    /// </summary>
    public record HeroEventData(int HeroId,string Reason);

    /// <summary>
    /// 获得名士事件数据
    /// </summary>
    public record HeroAddEventData(int HeroId,int AddValue, string Reason) : HeroEventData(HeroId,Reason);
}
