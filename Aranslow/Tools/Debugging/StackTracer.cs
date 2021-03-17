using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Aranslow.Tools.Debugging
{
    internal class StackTracer
    {
        internal static List<StackFrame> GetCurrentStackCalls()
        {
            return new StackTrace(new StackFrame(1, true)).GetFrames().ToList();
        }
    }
}
