using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Data
{
    internal interface IDirtyEntity<TEntity>
    {
        TEntity Original { get; }
        void Flush();

        PropertyInfo[] DirtyProperies { get; }
    }
}
