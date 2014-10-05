using System;
using System.Linq;
using System.ServiceModel.Syndication;
using Microsoft.Phone.Reactive;
using PodcastReader.Infrastructure.Models.Loaders;
using PodcastReader.Infrastructure.Utils;
using PodcastReader.Phone8.ViewModels;

namespace PodcastReader.Phone8.Models.Loaders
{
    public class PodcastItemsLoader : IPodcastItemsLoader
    {
        readonly ISubject<IPodcastItem> _subject = new ReplaySubject<IPodcastItem>();

        public PodcastItemsLoader(SyndicationFeed feed)
        {
            feed.Items.Where(item => item.IsPodcast())
                      .ToObservable()
                      .Do(item => item.SourceFeed = feed)
                      .Select(item => new PodcastItemViewModel(item))
                      .Subscribe(_subject);

            //foreach (var syndicationItem in feed.Items.Where(item => item.IsPodcast()))
            //{
            //    syndicationItem.SourceFeed = feed;
            //    _subject.OnNext(new PodcastItemViewModel(syndicationItem));
            //}
        }

        public IDisposable Subscribe(IObserver<IPodcastItem> observer)
        {
            return _subject.Subscribe(observer);
        }
    }
}
