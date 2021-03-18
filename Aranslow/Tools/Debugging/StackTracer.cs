using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Aranslow.Tools.Debugging
{
    internal class StackTracer
    {
        internal static List<StackFrame> GetCurrentStackCalls()
        {
            return new StackTrace(new StackFrame(1, true)).GetFrames().ToList();
        }

        //Convert function into attribute embeddable
        internal static CallerFrame GetCurrentStackCaller()
        {
            return CallerFrame.ConstructCallerFrame(new StackTrace().GetFrame(2));
        }

        internal class CallerFrame
        {
            internal string MethodName { get; set; }

            internal static CallerFrame ConstructCallerFrame(StackFrame sFrameToConstruct)
            {
                var callerMethod = sFrameToConstruct.GetMethod();

                return new CallerFrame() { MethodName = $"{callerMethod.ReflectedType.FullName}.{callerMethod.Name}()" };
            }
        }
    }
}
