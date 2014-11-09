namespace PodcastReader.Infrastructure.Http
{
    public struct ProgressValue
    {
        private readonly ulong _current;
        private readonly ulong _total;

        public ProgressValue(ulong current, ulong total)
        {
            _current = current;
            _total = total;
        }

        public ulong Current { get { return _current; } }
        public ulong Total { get { return _total; } }
    }

}