using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Text;

namespace Scorpio.Bougainvillea.Plugins
{
    internal class PluginAssemblyLoadContext : AssemblyLoadContext
    {
        public PluginAssemblyLoadContext() : base(true)
        {
        }
    }
}
