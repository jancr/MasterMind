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
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MasterMind {
    class Users {
        private Dictionary<string, UserStats> users;
        public Users() {
            foreach (string userName in Directory.GetFiles("user_data")) {
                UserStats us = new UserStats(userName);
                us.Load();
                users.Add(userName, us);
            }
        }
    }

    class GameStat {
        // difficulity is defined as the number of colors
        public GameDifficulity Difficulity { get; private set; }
        public int Rounds { get; private set; }
        public bool Win { get; private set; }

        public GameStat(GameDifficulity difficulity, int rounds, bool win) {
            Difficulity = difficulity;
            Rounds = rounds;
            Win = win;
        }
    }

    class UserStats {
        public string Name { get; private set; }
        public List<GameStat> Stats { get; private set; }
        
        public UserStats(string userName) {
            Name = userName;
            string file = getFilePath();
            if (File.Exists(file)) {
                Load();
            } else {
                Stats = new List<GameStat>();
            }
        }

        public void Add(GameDifficulity difficulity, int rounds, bool win) {
            Stats.Add(new GameStat(difficulity, rounds, win));
        }

        public double GetAverageRounds(GameDifficulity difficulity) {
            double games = 0;
            double rounds = 0;
            Stats.ForEach(delegate(GameStat s) {
                if (s.Difficulity == difficulity) {
                    games++;
                    rounds += s.Rounds;
                }
            });
            return rounds / games;
        }

        public double GetWinPercentage(GameDifficulity difficulity) {
            double games = 0;
            double wins = 0;
            Stats.ForEach(delegate(GameStat s) {
                if (s.Difficulity == difficulity) {
                    games++;
                    if (s.Win) {
                        wins++;
                    }
                }
            });
            return (100 * wins / games);
        }

        private string getFilePath() {
            return $"user_data/{Name}";
        }

        public void Load() {
            // Reset();
            Stream file = File.OpenRead(@getFilePath());
            BinaryFormatter bf = new BinaryFormatter();
            Stats = (List<GameStat>) bf.Deserialize(file);
            file.Close();
            // StreamReader f = new StreamReader($"user_data/{fileName}");
            // stats.ForEach(delegate(GameStat s) {
                // string[] tabs = f.ReadLine().Split("\t");
                // string difficulity = tabs[0];
                // int rounds = int.Parse(tabs[1]);
                // bool win = bool.Parse(tabs[1]);
                // Add(difficulity, rounds, win);
            // });
            // f.Close();
        }

        public void Save() {
            Stream file = File.Create(@getFilePath());
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, Stats);
            file.Close();
            // StreamWriter f = new StreamWriter($"user_data/{fileName}");
            // stats.ForEach(delegate(GameStat s) {
                // f.WriteLine($"{s.difficulity}\t{s.rounds}\t{s.win}\n");
            // });
            // f.Close();
        }
    }
}

