using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastReader.Phone8.Infrastructure.Storage
{
    public interface IStorage
    {
    }

    public interface IBackgroundTransferStorage
    {
        string GetTransferUrl(string relativeUrl);
    }
}
