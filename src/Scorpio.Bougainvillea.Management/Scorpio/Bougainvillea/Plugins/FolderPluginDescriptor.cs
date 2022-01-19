using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Plugins
{
    internal class FolderPluginDescriptor : IPluginDescriptor
    {
        private readonly string _path;
        private readonly bool _includeSubFolders;
        private readonly IDictionary<string, FilePluginDescriptor> _descriptors = new Dictionary<string, FilePluginDescriptor>();
        private readonly FileSystemWatcher _watcher;

        public IEnumerable<PluginDescriptor> Descriptors => _descriptors.Values.SelectMany(d => d.Descriptors);

        public FolderPluginDescriptor(string path, bool includeSubFolders)
        {
            _path = path;
            _includeSubFolders = includeSubFolders;
            Initailize();
            _watcher = new FileSystemWatcher(path, "*.dll");
            _watcher.Created += Watcher_Changed;
            _watcher.Changed += Watcher_Changed;
            _watcher.Deleted += Watcher_Changed;
            _watcher.EnableRaisingEvents = true;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    Task.Delay(100).ContinueWith(t => _descriptors.Add(e.FullPath, new FilePluginDescriptor(e.FullPath)));
                    break;
                case WatcherChangeTypes.Deleted:
                    _descriptors.Remove(e.FullPath, out var descriptor);
                    descriptor?.Unload();
                    break;
                case WatcherChangeTypes.Changed:
                    Task.Delay(100).ContinueWith(t => _descriptors.GetOrDefault(e.FullPath)?.Changed());
                    break;
                default:
                    break;
            }
        }

        private void Initailize()
        {
            _descriptors.Clear();
            var files = Directory.EnumerateFiles(_path, "*.dll", _includeSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            foreach (var item in files)
            {
                _descriptors.Add(item, new FilePluginDescriptor(item));
            }
        }

        public IManagementPlugin Generate(string code, IServiceProvider serviceProvider)
        {
            foreach (var item in _descriptors.Values)
            {
                if (item.ShouldBeCode(code))
                {
                    return item.Generate(code, serviceProvider);
                }
            }
            return null;
        }

        public bool ShouldBeCode(string code) => _descriptors.Values.Any(d => d.ShouldBeCode(code));
    }
}
