using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pr.Core.Utils
{
    public class AsyncOperationQueue
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly SynchronizationContext _syncContext;

        public AsyncOperationQueue(SynchronizationContext syncContext)
        {
            _syncContext = syncContext;
            _semaphore = new SemaphoreSlim(1);
        }

        public async Task<T> Execute<T>(Func<Task<T>> func)
        {
            await _semaphore.WaitAsync();

            var tcs = new TaskCompletionSource<T>();

            ExecuteInternal(async () =>
            {
                try
                {
                    var result = await func();
                    tcs.SetResult(result);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
                finally
                {
                    _semaphore.Release();
                }
            });

            return await tcs.Task;
        }

        public async Task Execute(Func<Task> func)
        {
            await _semaphore.WaitAsync();

            var tcs = new TaskCompletionSource<object>();

            ExecuteInternal(async () =>
            {
                try
                {
                    await func();
                    tcs.SetResult(null);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
                finally
                {
                    _semaphore.Release();
                }
            });

            await tcs.Task;
        }

        public async Task Execute(Action action)
        {
            await _semaphore.WaitAsync();

            var tcs = new TaskCompletionSource<object>();

            ExecuteInternal(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(null);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
                finally
                {
                    _semaphore.Release();
                }
            });

            await tcs.Task;
        }

        public Task Execute<T>(Func<T> func)
        {
            return Execute(() => Task.FromResult(func()));
        }

        private void ExecuteInternal(Action action)
        {
            if (_syncContext == null)
            {
                action();
            }
            else
            {
                _syncContext.Post(_ => action(), null);
            }
        }
    }
}
