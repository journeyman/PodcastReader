using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audio.Core
{
    public static class DebugOutputLogger
    {
        public static void Log(this object This, string logMessage)
        {
            Debug.WriteLine("{0}: {1}", This.GetType().Name, logMessage);
        }
    }
}
