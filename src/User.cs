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
        public string difficulity;
        public int rounds;
        public bool win;

        public GameStat(string gameDifficulity, int gameRounds, bool gameWin) {
            difficulity = gameDifficulity;
            rounds = gameRounds;
            win = gameWin;
        }
    }

    class UserStats {
        public string name;
        public List<GameStat> stats;
        
        public UserStats(string userName) {
            name = userName;
            stats = new List<GameStat>();
        }

        public void Add(string difficulity, int rounds, bool win) {
            stats.Add(new GameStat(difficulity, rounds, win));
        }

        public void Reset() {
            stats = new List<GameStat>();
        }

        public double GetAverageRounds(string difficulity) {
            double games = 0;
            double rounds = 0;
            stats.ForEach(delegate(GameStat s) {
                if (s.difficulity == difficulity) {
                    games++;
                    rounds += s.rounds;
                }
            });
            return rounds / games;
        }

        public double GetWinPercentage(string difficulity) {
            double games = 0;
            double wins = 0;
            stats.ForEach(delegate(GameStat s) {
                if (s.difficulity == difficulity) {
                    games++;
                    if (s.win) {
                        wins++;
                    }
                }
            });
            return (100 * wins / games);
        }

        public void Load(string fileName) {
            Reset();
            StreamReader f = new StreamReader($"user_data/{fileName}");
            stats.ForEach(delegate(GameStat s) {
                string[] tabs = f.ReadLine().Split("\t");
                string difficulity = tabs[0];
                int rounds = int.Parse(tabs[1]);
                bool win = bool.Parse(tabs[1]);
                Add(difficulity, rounds, win);
            });
            f.Close();
        }

        public void Save(string fileName) {
            StreamWriter f = new StreamWriter($"user_data/{fileName}");
            stats.ForEach(delegate(GameStat s) {
                f.WriteLine($"{s.difficulity}\t{s.rounds}\t{s.win}\n");
            });
            f.Close();
        }
    }
}

