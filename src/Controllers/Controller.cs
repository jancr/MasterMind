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
// using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace MasterMind.Controllers {
    public class MMController : Controller {
        private static MasterMind game;

        [HttpGet("api/mm/high-score/{difficulity}/{topX}")]
        public ActionResult HighScore(string difficulity, int topX) {
            List<KeyValuePair<string, double>> highScore;
            GameDifficulity gameDifficulity;
            PlayerList playerList = new PlayerList();

            GameDifficulity.TryParse(difficulity, out gameDifficulity);
            highScore = playerList.GetHighScores(gameDifficulity, topX);
            return Ok(highScore);
        }

        [HttpGet("api/mm/new-game/{difficulity}")]
        public ActionResult NewGame(string difficulity) {
            GameDifficulity gameDifficulity;
            GameDifficulity.TryParse(difficulity, out gameDifficulity);
            Reset(gameDifficulity);
            return MakeBord();
        }

        [HttpGet("api/mm/show")]
        public ActionResult Show() {
            return MakeBord();
        } 

        [HttpGet("api/mm/guess/{guess}/{userName}")]
        public ActionResult Guess(string guess, string userName) {
            if (game == null) {
                Reset(GameDifficulity.Medium);
            } 
            int[] arrayGuess = Array.ConvertAll(guess.Split(','), int.Parse);
            game.Guess(arrayGuess);
            
            // this code does now work when run from electron
            // because I do not nessesarily have read/write rights :S
            // so it has been commented out
            // if (game.Status != GameStatus.Ongoing) {
                // if (userName != "") {
                    // Player player = new PlayerList().GetPlayer(userName);
                    // player.Add(game.Difficulity, game.GuessCount, game.Status);
                    // player.Save();
                // } 
            // }
            return MakeBord();
        }

        private void Reset(GameDifficulity difficulity) {
            game = new MasterMind(difficulity);
        }

        private ActionResult MakeBord() {
            int[][] bord = new int[game.RowCount][];
            int[][] pegs = new int[game.RowCount][];

            for (int i = 0; i < game.RowCount; i++) {
                if (i < game.GuessCount) {
                    bord[i] = game.Bord[i].Vector;
                    pegs[i] = new[] { game.Pegs[i].Black, game.Pegs[i].White };
                } else {
                    bord[i] = new int[game.ColCount];
                    pegs[i] = new[] { -1, -1 };
                    for (int j = 0; j < game.ColCount; j++) {
                        bord[i][j] = -1;
                    }
                }
            }
            var json = new { 
                bord = bord, 
                pegs = pegs,
                game_status = game.Status.ToString()
            };
            return Ok(json);
        }
    }
}
