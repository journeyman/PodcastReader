using System;
using System.Linq;
using System.ServiceModel.Syndication;
using Microsoft.Phone.Reactive;
using Pr.Core.Utils;
using Pr.Core.Entities.Podcasts;
using Pr.Core.Models.Loaders;
using Pr.Phone8.ViewModels;

namespace Pr.Phone8.Models.Loaders
{
    public class PodcastItemsLoader : IPodcastItemsLoader
    {
        private readonly SyndicationFeed _feed;
        private IConnectableObservable<PodcastItemViewModel> _observable;

        public PodcastItemsLoader(SyndicationFeed feed)
        {
            _feed = feed;

            _observable = Observable.Defer(() =>
                            feed.Items.Where(item => item.IsPodcast())
                                .ToObservable()
                                .Do(item => item.SourceFeed = feed)
                                .Select(item => new PodcastItemViewModel(item)))
                      .Publish();
        }

        public IDisposable Subscribe(IObserver<IPodcastItem> observer)
        {
            return _observable.RefCount().Subscribe(observer);
        }
    }
}
