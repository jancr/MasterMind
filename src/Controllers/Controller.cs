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
        private static int[][] bord;
        private static int[][] pegs;

        [HttpGet("api/mm/new-game")]
        public ActionResult NewGame() {
            Reset();
            return Ok(MakeBord());
        }

        [HttpGet("api/mm/show")]
        public ActionResult Show() {
            return Ok(MakeBord());
        } 

        // [HttpGet]
        [HttpGet("api/mm/guess/{guess}")]
        public ActionResult Guess(string guess) {
            if (game == null || game.Status != GameStatus.Ongoing) {
                Reset();
            }

            int[] arrayGuess = Array.ConvertAll(guess.Split(','), int.Parse);
            Peg peg = game.Guess(arrayGuess);
            return Ok(MakeBord());
        }

        private void Reset() {
            game = new MasterMind();
            bord = new int[game.RowCount][];
            pegs = new int[game.RowCount][];
        }

        private Object MakeBord() {
            for (int i = 0; i < game.RowCount; i++) {
                if (i < game.GuessCount) {
                    bord[i] = game.bord[i].Vector;
                    pegs[i] = new[] { game.pegs[i].Black, game.pegs[i].White };
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
            return json;
        }
    }
}
