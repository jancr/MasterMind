using System;
using System.IO;
// using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
// using Microsoft.AspNetCore.Hosting;

using MasterMind.MMStartup;

namespace MasterMind {
    public class Program {
        public static void Main(string[] args) {
            // if (args.Length < 1) {
                // args = new[] {"5000"};
            // }
            if ((args.Length >= 1) && (args[0] == "console")) {
                MasterMindConsle.Run();
            } else {
                if (args.Length == 1) {
                    BuildWebHost(args[0]).Run();
                } else {
                    BuildWebHost().Run();
                }
            }
        }

        public static IWebHost BuildWebHost() {
            IWebHost host = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();
            return host;
        }

        public static IWebHost BuildWebHost(string port) {
            IWebHost host = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseUrls($"http://*:{port}") 
                .Build();
            return host;
        }
    }
}
