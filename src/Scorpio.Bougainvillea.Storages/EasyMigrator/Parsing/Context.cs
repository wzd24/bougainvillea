using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EasyMigrator.Parsing.Model;


namespace EasyMigrator.Parsing
{

    /// <summary>
    /// 
    /// </summary>
    public class Context
    {

        /// <summary>
        /// 
        /// </summary>
        public Conventions Conventions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Table Table { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Type ModelType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<PropertyInfo> ColumnFields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<PropertyInfo, Column> Columns { get; set; } = new Dictionary<PropertyInfo, Column>();

        /// <summary>
        /// 
        /// </summary>
        public object Model { get; set; }
    }
}
