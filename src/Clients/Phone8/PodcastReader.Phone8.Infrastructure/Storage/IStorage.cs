using System;
using System.Threading.Tasks;

namespace PodcastReader.Infrastructure.Storage
{
    public interface IStorage
    {
        Task Move(Uri from, Uri to);
        Task RemoveFile(Uri uri);
    }
}