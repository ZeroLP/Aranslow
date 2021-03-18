using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Reflection;

namespace Aranslow.Tools
{
    internal class Logger
    {
        internal static void CreateLoggerInstance()
        {
            NativeImport.AllocConsole();

            var outFile = NativeImport.CreateFile("CONOUT$", NativeImport.ConsolePropertyModifiers.GENERIC_WRITE
                                                            | NativeImport.ConsolePropertyModifiers.GENERIC_READ,
                                                            NativeImport.ConsolePropertyModifiers.FILE_SHARE_WRITE,
                                                            0, NativeImport.ConsolePropertyModifiers.OPEN_EXISTING, /*FILE_ATTRIBUTE_NORMAL*/0, 0);

            var safeHandle = new SafeFileHandle(outFile, true);

            NativeImport.SetStdHandle(-11, outFile);

            FileStream fs = new FileStream(safeHandle, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fs) { AutoFlush = true };

            Console.SetOut(writer);

            if (NativeImport.GetConsoleMode(outFile, out var cMode))
            {
                NativeImport.SetConsoleMode(outFile, cMode | 0x0200);
            }

            DisableQuickEditMode();

            Console.Title = $"Log Window - {Assembly.GetExecutingAssembly().GetName().Name}";
        }

        internal static void Log(object content) => Console.WriteLine(content);

        private static void DisableQuickEditMode()
        {
            if (!NativeImport.GetConsoleMode(NativeImport.GetConsoleWindow(), out uint mode))
                return;

            mode = mode & ~((uint)NativeImport.ConsolePropertyModifiers.ENABLE_QUICK_EDIT | 128/*ExtendedFlags*/ );

            if (!NativeImport.SetConsoleMode(NativeImport.GetConsoleWindow(), mode)) { }
        }
    }
}
