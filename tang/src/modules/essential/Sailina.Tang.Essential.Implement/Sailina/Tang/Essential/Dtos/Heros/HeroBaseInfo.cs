using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sailina.Tang.Essential.Settings;

namespace Sailina.Tang.Essential.Dtos.Heros
{
    /// <summary>
    /// 
    /// </summary>
    public class HeroBaseInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lv"></param>
        /// <param name="studyLv"></param>
        /// <param name="starLv"></param>
        public HeroBaseInfo(int id, int lv, int studyLv, int starLv)
        {
            Id = id;
            Lv = lv;
            StudyLv = studyLv;
            StarLv = starLv;
        }
        /// <summary>
        /// 名士Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Lv { get; set; }
        /// <summary>
        /// 研修等级
        /// </summary>
        public int StudyLv { get; set; }
        /// <summary>
        /// 升星等级
        /// </summary>
        public int StarLv { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        public Profession Profession { get; set; }
        /// <summary>
        /// 品质
        /// </summary>
        public Quality Quality { get; set; }
    }
}
