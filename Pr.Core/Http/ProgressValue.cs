namespace PodcastReader.Infrastructure.Http
{
    public struct ProgressValue
    {
        public ProgressValue(ulong current, ulong total)
        {
            Current = current;
            Total = total;
        }

        public ulong Current { get; }
        public ulong Total { get; }
    }

}