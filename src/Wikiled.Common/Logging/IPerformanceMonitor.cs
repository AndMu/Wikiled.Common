namespace Wikiled.Common.Logging
{
    public interface IPerformanceMonitor
    {
        void SetOwner<T>(T owner);

        void Increment<T>(T instance);

        void ManualyCount();

        void Increment();

        string ToString();
    }
}