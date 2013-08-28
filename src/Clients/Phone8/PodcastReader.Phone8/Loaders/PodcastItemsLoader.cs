using System;
using System.Linq;
using System.ServiceModel.Syndication;
using Microsoft.Phone.Reactive;
using PodcastReader.Infrastructure;
using PodcastReader.Phone8.Interfaces.Loaders;
using PodcastReader.Phone8.Interfaces.Models;
using PodcastReader.Phone8.ViewModels;

namespace PodcastReader.Phone8.Loaders
{
    public class PodcastItemsLoader : IPodcastItemsLoader
    {
        readonly ISubject<IPodcastItem> _subject = new ReplaySubject<IPodcastItem>();

        public PodcastItemsLoader(SyndicationFeed feed)
        {
            foreach (var syndicationItem in feed.Items.Where(item => item.IsPodcast()))
            {
                _subject.OnNext(new PodcastItemViewModel(syndicationItem));
            }
        }

        public IDisposable Subscribe(IObserver<IPodcastItem> observer)
        {
            return _subject.Subscribe(observer);
        }
    }
}
