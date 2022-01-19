using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using Scorpio;

namespace Scorpio.Bougainvillea.Plugins
{
    /// <summary>
    /// 
    /// </summary>
    public class PluginLoadOptions
    {
        private readonly HashSet<IPluginDescriptor> _plugins;
        
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<IPluginDescriptor> Plugins => _plugins;

        /// <summary>
        /// 
        /// </summary>
        public PluginLoadOptions()
        {
            _plugins = new HashSet<IPluginDescriptor>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void AddType(Type type)
        {
            if (!type.AttributeExists<PluginCodeAttribute>())
            {
                return;
            }
            _plugins.Add(new TypePluginDescriptor(type));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="FileLoadException"></exception>
        public void AddFile(string path)
        {
            if (Path.GetExtension(path).ToLowerInvariant() != "dll")
            {
                throw new FileLoadException("指定的文件不是DLL文件");
            }
            _plugins.Add(new FilePluginDescriptor(path));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="includeSubFolder"></param>
        /// <exception cref="FileLoadException"></exception>
        public void AddFolder(string path,bool includeSubFolder=false)
        {
            if (!Directory.Exists(path))
            {
                throw new FileLoadException("指定的的文件夹不存在");
            }
            _plugins.Add(new FolderPluginDescriptor(path, includeSubFolder));
        }

    }
}
