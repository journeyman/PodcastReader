using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Akavache;
using JetBrains.Annotations;
using PodcastReader.Infrastructure.Entities.Podcasts;

namespace PodcastReader.Infrastructure.Caching
{
    public class FileModel
    {
        public FileModel(PodcastId id, [CanBeNull]CacheInfo cacheInfo)
        {
            Id = id;
            //State = new CachingState();
			UpdateCachingState(cacheInfo);
		}

        public PodcastId Id { get; set; }
        public CachingState State { get; }

        public void UpdateCachingState(CacheInfo entry)
        {
            //State
        }
    }

    public class FileCache
    {
        readonly ReplaySubject<FileModel> _cachedFiles = new ReplaySubject<FileModel>(); 
        readonly Dictionary<PodcastId, FileModel> _memCache = new Dictionary<PodcastId, FileModel>(); 
        readonly TaskCompletionSource<object> _initTask = new TaskCompletionSource<object>(); 

        public static readonly FileCache Instance = new FileCache();

        private FileCache() {}

        public async Task Init()
        {
            var dataFromCache = Cache.Local.GetAllObjects<CacheInfo>()
                 .SelectMany(x => x)
                 .Select(x => new FileModel(new PodcastId(x.FileUri.OriginalString), x));

            dataFromCache.Subscribe(x => { }, () => _initTask.SetResult(null));
            dataFromCache.Subscribe(_cachedFiles);

            await WaitInit().ConfigureAwait(false);
        }

        public IObservable<FileModel> CachedFiles => _cachedFiles;

        public async Task UpdateOrCreateCacheEntry(PodcastId id, CacheInfo entry)
        {
            FileModel existingEnry;
            if (!_memCache.TryGetValue(id, out existingEnry))
            {
                var fileModel = new FileModel(id, entry);
                _memCache.Add(id, fileModel);
                _cachedFiles.OnNext(fileModel);
            }
            else
            {
                existingEnry.UpdateCachingState(entry);
            }

            await Cache.Local.InsertObject(id.Url, entry);
        }

        public async Task WaitInit()
        {
            await _initTask.Task.ConfigureAwait(false);
        }
    }
}