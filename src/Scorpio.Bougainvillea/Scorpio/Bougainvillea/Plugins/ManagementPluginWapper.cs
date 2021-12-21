using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Scorpio;

namespace Scorpio.Bougainvillea.Plugins
{
    internal class ManagementPluginWapper : IManagementPlugin
    {
        private bool _disposedValue;
        private readonly IManagementPlugin _plugin;
        private readonly DisposeAction _disposeAction;

        public ManagementPluginWapper(IManagementPlugin plugin,DisposeAction disposeAction)
        {
            _plugin = plugin;
            _disposeAction = disposeAction;
        }
        Task<object> IManagementPlugin.ExecuteAsync(ManagementPluginExecutionContext context) => throw new NotImplementedException();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _disposeAction.Dispose();
                }

                _disposedValue = true;
            }
        }


        void IDisposable.Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
