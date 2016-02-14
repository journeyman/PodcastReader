using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Splat;

namespace Pr.Core.Utils.Logging
{
    public class CompositeLogger : ILogger
    {
        private readonly List<ILogger> _loggers;
        private LogLevel _level;

        public CompositeLogger(params ILogger[] loggers)
        {
            _loggers = loggers.ToList();
        }

        public void Write(string message, LogLevel logLevel)
        {
            foreach (var logger in _loggers)
            {
                logger.Write(message, logLevel);
            }
        }

        public LogLevel Level
        {
            get { return _level; }
            set
            {
                _level = value;
                foreach (var logger in _loggers)
                {
                    logger.Level = value;
                }
            }
        }
    }

    public class PRDebugLogger : ILogger
    {
        public void Write(string message, LogLevel logLevel)
        {
            if (logLevel >= Level)
                Debug.WriteLine(message);
        }

        public LogLevel Level { get; set; }
    }
}
