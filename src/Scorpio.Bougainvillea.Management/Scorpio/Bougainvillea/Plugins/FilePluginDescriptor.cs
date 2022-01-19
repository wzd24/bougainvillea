using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

namespace Scorpio.Bougainvillea.Plugins
{
    internal class FilePluginDescriptor : IPluginDescriptor
    {
        private readonly string _path;
        private readonly Dictionary<string, PluginDescriptor> _types = new Dictionary<string, PluginDescriptor>();
        private PluginAssemblyLoadContext _loadContext;
        private bool _needLoad = true;

        public IReadOnlyDictionary<string, PluginDescriptor> Types => EnsureLoad();

        public IEnumerable<PluginDescriptor> Descriptors => Types.Values;

        public FilePluginDescriptor(string path)
        {
            _path = path;
        }

        public void Unload()
        {
            _loadContext?.Unload();
        }

        private void LoadAssembly()
        {
            _types.Clear();
            _loadContext?.Unload();
            _loadContext = new PluginAssemblyLoadContext();
            using (var stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var symbolsStream = LoadSymbols())
                {
                    var ass = _loadContext.LoadFromStream(stream, symbolsStream);
                    var types = ass.GetExportedTypes();
                    foreach (var item in types)
                    {
                        if (!item.AttributeExists<PluginCodeAttribute>())
                        {
                            continue;
                        }
                        var code = item.GetAttribute<PluginCodeAttribute>().Code;
                        _types[code] = new PluginDescriptor(item);
                    }

                }
            }
        }

        private Stream LoadSymbols()
        {
            var path = Path.ChangeExtension(_path, "pdb");
            if (File.Exists(path))
            {
                return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            return null;
        }

        public void Changed()
        {
            _needLoad = true;
        }

        public IReadOnlyDictionary<string, PluginDescriptor> EnsureLoad()
        {
            if (_needLoad)
            {
                _needLoad = false;
                LoadAssembly();
            }
            return _types;
        }

        public IManagementPlugin Generate(string code, IServiceProvider serviceProvider)
        {
            var descriptor = Types[code];
            return descriptor.Generate(serviceProvider);
        }

        public bool ShouldBeCode(string code) => Types.ContainsKey(code);
    }
}
