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
using System.Linq;

namespace MasterMind {

    interface ISerializeObject {
        void Save();
        void Load();
    }

    [Serializable]
    class GameStat {
        public GameDifficulity Difficulity { get; private set; }
        public int Rounds { get; private set; }
        public GameStatus Status { get; private set; }

        public GameStat(GameDifficulity difficulity, int rounds, GameStatus status) {
            Difficulity = difficulity;
            Rounds = rounds;
            Status = status;
        }
    }

    class Player : ISerializeObject {
        // properties
        public string Name { get; private set; }
        public List<GameStat> Stats { get; private set; }

        // constructors
        public Player(string name, bool load) {
            Setup(name, load);
        }

        public Player(string name) {
            Setup(name, true);
        }

        // methods
        private void Setup(string name, bool load) {
            Name = name;
            if (load && File.Exists(GetFilePath())) {
                Load();
            } else {
                Stats = new List<GameStat>();
            }
        }

        public void Add(GameDifficulity difficulity, int rounds, GameStatus status) {
            Stats.Add(new GameStat(difficulity, rounds, status));
        }

        public double GetAverageRounds(GameDifficulity difficulity) {
            double games = 0;
            double rounds = 0;
            Stats.ForEach(delegate(GameStat s) {
                if (s.Difficulity == difficulity && s.Status == GameStatus.Won) {
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
                    if (s.Status == GameStatus.Won) {
                        wins++;
                    }
                }
            });
            return (100 * wins / games);
        }

        private string GetFilePath() {
            return $"user_data/{Name}";
        }

        public void Load() {
            Stream file = File.OpenRead(@GetFilePath());
            if (file.Length != 0) {
                BinaryFormatter bf = new BinaryFormatter();
                Stats = (List<GameStat>) bf.Deserialize(file);
            } else {
                Stats = new List<GameStat>();
            }
            file.Close();
        }

        public void Save() {
            Stream file = File.Create(@GetFilePath());
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, Stats);
            file.Close();
        }
    }

    class PlayerList : ISerializeObject {
        // properties
        public Dictionary<string, Player> Players { get; private set; }

        // construcors
        public PlayerList() {
            Load();
        }

        // methods
        public void Load() {
            Players = new Dictionary<string, Player>();
            foreach (string path in Directory.GetFiles("user_data")) {
                string userName = Path.GetFileName(path);
                Player player = new Player(userName);
                player.Load();
                Players.Add(userName, player);
            }
        }

        public void Save() {
            foreach(Player player in Players.Values) {
                player.Save();
            }
        }

        public Player GetPlayer(string playerName) {
            if (Players.ContainsKey(playerName)) {
                return Players[playerName];
            }
            Player player = new Player(playerName);
            Players.Add(playerName, player);
            return player;
        }

        public List<KeyValuePair<string, double>> GetHighScores(
                GameDifficulity difficulity, int topX) {
            List<KeyValuePair<string, double>> playerScores = new List<KeyValuePair<string, double>>();
            foreach(KeyValuePair<string, Player> player in Players) {
                double v = player.Value.GetWinPercentage(difficulity);
                KeyValuePair<string, double> p = new KeyValuePair<string, double>(player.Key, v);
                playerScores.Add(p);
            }
            playerScores.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            topX = Math.Min(playerScores.Count, topX);
            List<KeyValuePair<string, double>> highScores = playerScores.GetRange(0, topX);
            return highScores;
        }
    }
}

