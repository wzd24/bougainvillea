using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EasyMigrator.Parsing.Model;


namespace EasyMigrator.Extensions
{

    /// <summary>
    /// 
    /// </summary>
    static public class ModelExtensions
    {

      /// <summary>
      /// 
      /// </summary>
      /// <param name="columns"></param>
      /// <returns></returns>
        static public IEnumerable<Column> PrimaryKey(this IEnumerable<Column> columns) => columns.Where(c => c.IsPrimaryKey);

 
    }
}
