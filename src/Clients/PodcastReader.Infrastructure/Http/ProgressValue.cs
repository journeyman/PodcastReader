namespace PodcastReader.Infrastructure.Http
{
    public struct ProgressValue
    {
        private readonly long _current;
        private readonly long _total;

        public ProgressValue(long current, long total)
        {
            _current = current;
            _total = total;
        }

        public long Current { get { return _current; } }
        public long Total { get { return _total; } }
    }

}