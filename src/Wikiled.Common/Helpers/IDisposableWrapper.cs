using System;

namespace Wikiled.Common.Helpers
{
    public interface IDisposableWrapper<out T> : IDisposable
        where T : IDisposable, new() 
    {
        T Instance { get; }
    }
}
