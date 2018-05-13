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
using System.Diagnostics;

namespace MasterMind {
    class TestHighScore {
        private static Player GetJoe() {
            // joe has who 8 games and lost 2 on Easy
            Player joe = new Player("Average Joe", false);
            for (int i = 2; i < 10; i++) {
                // won in 2,3,4,5,6,7,8,9 rounds, average = 5.5
                joe.Add(GameDifficulity.Easy, i, GameStatus.Won);
            }
            for (int i = 0; i < 2; i++) {
                joe.Add(GameDifficulity.Easy, 10, GameStatus.Lost);
            }
            
            // joe has won 5 and lost 5 on Medium
            for (int i = 5; i < 10; i++) {
                joe.Add(GameDifficulity.Medium, i, GameStatus.Won);
            }
            for (int i = 0; i < 5; i++) {
                joe.Add(GameDifficulity.Medium, 10, GameStatus.Lost);
            }
            
            // joe has won 2 and lost 8 on Hard
            for (int i = 8; i < 10; i++) {
                joe.Add(GameDifficulity.Hard, i, GameStatus.Won);
            }
            for (int i = 0; i < 8; i++) {
                joe.Add(GameDifficulity.Hard, 10, GameStatus.Lost);
            }
            Debug.Assert(joe.GetAverageRounds(GameDifficulity.Easy) == 5.5);
            Debug.Assert(joe.GetAverageRounds(GameDifficulity.Medium) == 7.0);
            Debug.Assert(joe.GetAverageRounds(GameDifficulity.Hard) == 8.5);
            Debug.Assert(joe.GetWinPercentage(GameDifficulity.Easy) == 80);
            Debug.Assert(joe.GetWinPercentage(GameDifficulity.Medium) == 50);
            Debug.Assert(joe.GetWinPercentage(GameDifficulity.Hard) == 20);
            return joe;
        }

        private static Player GetBob() {
            // bob has who 4 games and lost 6 on Easy
            Player bob = new Player("Beginner Bob", false);
            for (int i = 6; i < 10; i++) {
                // won in 6, 7, 8, 9 rounds, average = 7.5
                bob.Add(GameDifficulity.Easy, i, GameStatus.Won);
            }
            for (int i = 0; i < 6; i++) {
                bob.Add(GameDifficulity.Easy, 10, GameStatus.Lost);
            }
            // bob has won 1 and lost 4 on Medium
            bob.Add(GameDifficulity.Medium, 8, GameStatus.Won);
            for (int i = 0; i < 4; i++) {
                bob.Add(GameDifficulity.Medium, 10, GameStatus.Lost);
            }
            Debug.Assert(bob.GetAverageRounds(GameDifficulity.Easy) == 7.5);
            Debug.Assert(bob.GetAverageRounds(GameDifficulity.Medium) == 8.0);
            Debug.Assert(bob.GetWinPercentage(GameDifficulity.Easy) == 40);
            Debug.Assert(bob.GetWinPercentage(GameDifficulity.Medium) == 20);
            return bob;
        }

        public static void Test() {
            Player joe = GetJoe();
            joe.Save();
 
            Player bob = GetBob();
            bob.Save();

            // create PlayerList bu loading all the above
            PlayerList players = new PlayerList();
        }
    }
}

