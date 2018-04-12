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

namespace MasterMind {
    class GameStat {
        // difficulity is defined as the number of colors
        public int Difficulity { get; private set; }
        public int Rouds { get; private set; }
        public bool Win { get; private set; }

        public GameStat(int difficulity, int rounds, bool win) {
            Difficulity = difficulity;
            Rounds = rounds;
            Win = win;
        }
    }

    class UserStats {
        private string name;
        private List<GameStat> stats;
        
        public UserStats(string userName) {
            name = userName;
            string file = getFilePath();
            if (File.Exists(file)) {
                Load();
            } else {
                stats = new List<GameStat>();
            }
        }

        public void Add(string difficulity, int rounds, bool win) {
            stats.Add(new GameStat(difficulity, rounds, win));
        }

        public double GetAverageRounds(int difficulity) {
            double games = 0;
            double rounds = 0;
            stats.ForEach(delegate(GameStat s) {
                if (s.Difficulity == difficulity) {
                    games++;
                    rounds += s.Rounds;
                }
            });
            return rounds / games;
        }

        public double GetWinPercentage(string difficulity) {
            double games = 0;
            double wins = 0;
            stats.ForEach(delegate(GameStat s) {
                if (s.Difficulity == difficulity) {
                    games++;
                    if (s.Win) {
                        wins++;
                    }
                }
            });
            return (100 * wins / games);
        }

        private getFilePath() {
            return $"user_data/{name}";
        }

        public void Load() {
            // Reset();
            Stream file = File.OpenRead(@getFilePath);
            BinaryFormatter bf = new BinaryFormatter();
            stats = (List<GameStat>) bf.Deserialize(file);
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
            Stream file = File.Create(@getFilePath);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(objectfil, stats);
            file.Close();
            // StreamWriter f = new StreamWriter($"user_data/{fileName}");
            // stats.ForEach(delegate(GameStat s) {
                // f.WriteLine($"{s.difficulity}\t{s.rounds}\t{s.win}\n");
            // });
            // f.Close();
        }
    }
}

