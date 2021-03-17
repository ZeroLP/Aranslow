using System;

namespace Aranslow
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var gameEngine = new Engine())
                gameEngine.Run();
        }
    }
}
