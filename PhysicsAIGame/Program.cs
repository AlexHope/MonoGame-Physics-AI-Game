using System;

namespace General
{
#if WINDOWS || LINUX
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new PhysicsAIGame())
                game.Run();
        }
    }
#endif
}
