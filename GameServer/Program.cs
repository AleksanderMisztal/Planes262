using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using GameDataStructures.Dtos;
using GameServer.Utils;

namespace GameServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Thread(MainThread).Start();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => 
                    webBuilder.UseStartup<Startup>());

        private static void MainThread()
        {
            Console.WriteLine($"Main thread started. Running at {Constants.ticksPerSec} ticks per second.");
            Console.WriteLine(Json.Serialize(new LevelDto("bg", new BoardDto(16, 10), new CameraDto(1, 2, 3), new TroopDto[]{})));

            DateTime nextLoop = DateTime.Now;
            while (true)
            {
                if (nextLoop < DateTime.Now)
                {
                    ThreadManager.UpdateMain();
                    nextLoop = nextLoop.AddMilliseconds(Constants.msPerTick);
                }
                else
                {
                    int milliseconds = (int)(nextLoop - DateTime.Now).TotalMilliseconds;
                    Thread.Sleep(Math.Max(milliseconds, 0));
                }
            }
        }
    }
}
