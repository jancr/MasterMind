// Copyright 2018 Jan Christian Refsgaard (jancrefsgaard@gmail.com)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
            if (args.Length >= 1) {
                if (args[0] == "console") {
                    MasterMindConsle.Run();
                } else if (args[0] == "test") {
                    TestHighScore.Test();
                } else {
                    BuildWebHost(args[0]).Run();
                }
            } else {
                BuildWebHost().Run();
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
