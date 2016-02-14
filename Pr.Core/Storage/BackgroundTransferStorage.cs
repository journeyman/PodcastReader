using System;
using System.IO;
using System.Threading.Tasks;

namespace Pr.Core.Storage
{
    public class BackgroundTransferStorage : IBackgroundTransferStorage
    {
        private const string TRANSFER_PATH = "/shared/transfers";

        private readonly IStorage _storage;

        public BackgroundTransferStorage(IStorage storage)
        {
            _storage = storage;
        }

        public string GetTransferUrl(string relativeUrl)
        {
            return Path.Combine(TRANSFER_PATH, Path.GetFileName(relativeUrl));
        }

        public async Task RemoveFile(Uri downloadLocation)
        {
            await _storage.RemoveFile(downloadLocation).ConfigureAwait(false);
        }
    }
}