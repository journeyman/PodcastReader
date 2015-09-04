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
			UpdateCachingState(cacheInfo);
		}

        public PodcastId Id { get; set; }
        public ICachingState State { get; }

        public void UpdateCachingState(CacheInfo entry)
        {
            
        }
    }

    public class FileCache
    {
        readonly ReplaySubject<FileModel> _cachedFiles = new ReplaySubject<FileModel>(); 
        readonly Dictionary<PodcastId, FileModel> _memCache = new Dictionary<PodcastId, FileModel>(); 

        public static readonly FileCache Instance = new FileCache();

        private FileCache() {}

        public void Init()
        {
            var cachedInfoSource = Cache.Local.GetAllObjects<CacheInfo>()
                .SelectMany(x => x);
			
            cachedInfoSource.Select(x => new FileModel(new PodcastId(x.FileUri.OriginalString), x))
							.Subscribe(_cachedFiles);

            cachedInfoSource.Subscribe(x => UpdateOrCreateCacheEntry(new PodcastId(x.FileUri.OriginalString), x));
        }

        public IObservable<FileModel> CachedFiles => _cachedFiles;

        public async Task UpdateOrCreateCacheEntry(PodcastId id, CacheInfo entry)
        {
            FileModel existingEnry;
            if (!_memCache.TryGetValue(id, out existingEnry))
            {
                _memCache.Add(id, new FileModel(id, entry));
            }
            else
            {
                existingEnry.UpdateCachingState(entry);
            }
            await Cache.Local.InsertObject(id.Url, entry);
        }
    }
}