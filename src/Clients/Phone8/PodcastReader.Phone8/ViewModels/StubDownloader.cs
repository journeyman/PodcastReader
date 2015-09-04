using System;
using System.Collections.Immutable;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using PodcastReader.Infrastructure.Http;

namespace PodcastReader.Phone8.ViewModels
{
    public class StubDownloader : IBackgroundDownloader
    {
        public IAwaitableTransfer Load(Uri url, IProgress<ProgressValue> progress, CancellationToken cancellation)
        {
		    url = new Uri("https://ia700406.us.archive.org/28/items/GodSaveTheQueen_306/GodSaveTheQueen.ogg");
	        var task = Task.Run(async () =>
	        {
		        var client = new HttpClient();

		        var head = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url), cancellation);
		        var totalLength = (ulong) head.Content.Headers.ContentLength;
		        var source = await client.GetStreamAsync(url);

		        var folder = ApplicationData.Current.LocalFolder;
		        var file = await folder.CreateFileAsync(Path.GetFileName(url.AbsoluteUri), CreationCollisionOption.ReplaceExisting);

		        using (client)
		        using (var target = await file.OpenStreamForWriteAsync())
		        using (source)
		        {
			        var buffer = new byte[8192];
			        int bytesRead = 0;
			        while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellation)) > 0)
			        {
				        await target.WriteAsync(buffer, 0, bytesRead, cancellation);
				        progress.Report(new ProgressValue((ulong) target.Position, totalLength));
			        }
		        }
	        });

			return new StubDownloadAwaitableRequest(task, new Uri("\\" + Path.GetFileName(url.AbsoluteUri), UriKind.RelativeOrAbsolute));
        }

	    public IImmutableDictionary<Uri, IAwaitableTransfer> ActiveRequests => ImmutableDictionary.Create<Uri, IAwaitableTransfer>();

	    public Task<IImmutableDictionary<Uri, IAwaitableTransfer>> Update()
        {
            return Task.FromResult(ActiveRequests);
        }

        public Task ForgetAbout(Uri transferUri)
        {
            return Task.FromResult(0);
        }
    }

	public class StubDownloadAwaitableRequest : IAwaitableTransfer
	{
		public StubDownloadAwaitableRequest(Task transferTask, Uri downloadLocation)
		{
			DownloadLocation = downloadLocation;
			TransferTask = transferTask;
		}

		public Uri DownloadLocation { get; set; }

		public Uri UploadLocation { get; set; }

		public Task TransferTask { get; set; }
	}
}