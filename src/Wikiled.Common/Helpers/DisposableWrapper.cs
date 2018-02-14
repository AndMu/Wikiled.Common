using System;
using Wikiled.Core.Utility.Helpers;

namespace Wikiled.Common.Helpers
{
    public class DisposableWrapper<T> : IDisposableWrapper<T>
        where T : IDisposable, new()
    {
        public DisposableWrapper()
        {
            Instance = new T();
        }

        public T Instance { get; }

        public void Dispose()
        {
            Instance.Dispose();
        }
    }
}
