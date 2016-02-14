using System;
using System.Threading.Tasks;

namespace Pr.Core.Storage
{
    public interface IStorage
    {
        Task Move(string from, string to);
        Task RemoveFile(Uri uri);
    }
}