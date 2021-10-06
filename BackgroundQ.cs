using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CodeServiceJD.BackgroundQ
{
    public interface IBackgroundQ<T>
    {
        void AddElement(T message);
        Task<T> GetElementAsync(CancellationToken cancellationToken);
    }
    public class BackgroundQ<T> : IBackgroundQ<T>
    {
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);
        public void AddElement(T message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _queue.Enqueue(message);
            _signal.Release();
        }

        public async Task<T> GetElementAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _queue.TryDequeue(out var message);
            return message;
        }
    }
}
