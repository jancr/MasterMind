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
using System.Linq;

namespace MasterMind { 
    enum GameStatus {Won, Lost, Ongoing};
    enum GameDifficulity {Easy, Medium, Hard};

    class MasterMind {
        // attributes 
        private Entry secret;

        // properties
        public int RowCount { get; private set; }
        public int ColCount { get; private set; }
        public int ColorCount { get; private set; }

        public int GuessCount { get; private set; }
        public GameStatus Status { get; private set; }
        public GameDifficulity Difficulity { get; private set; }
        public Entry[] Bord { get; private set; }
        public Peg[] Pegs { get; private set; }

        // constructors
        public MasterMind() {
            NewGame(GameDifficulity.Medium);
        }

        public MasterMind(GameDifficulity difficulity) {
            NewGame(difficulity);
        }

        public MasterMind(int rows, int columns, int colors) {
            NewGame(rows, columns, colors);
        }

        // methods
        public void NewGame() {
            NewGame(GameDifficulity.Medium);
        }

        public void NewGame(GameDifficulity difficulity) {
            // bord is always 10 x 4, but have more colors at higher difficuility
            if (difficulity == GameDifficulity.Hard) {
                NewGame(10, 4, 8);
            } else if (difficulity == GameDifficulity.Medium) {
                NewGame(10, 4, 6);
            } else if (difficulity == GameDifficulity.Easy) {
                NewGame(10, 4, 4);
            }
        }

        public void NewGame(int rows, int columns, int colors) {
            RowCount = rows;
            ColCount = columns;
            ColorCount = colors;

            Random rand = new Random();
            Status = GameStatus.Ongoing;
            GuessCount = 0;
            Bord = new Entry[RowCount];
            Pegs = new Peg[RowCount];
            int[] _secret = new int[ColCount];

            for(int i = 0; i < ColCount; i++) {
                int color = rand.Next(0, ColorCount);
                _secret[i] = color;
            }
            secret = new Entry(_secret, ColorCount);
        }

        public Peg Guess(int[] guess) {
            if (Status != GameStatus.Ongoing) {
                string msg = String.Format("Game Over, you have {0}", Status.ToString());
                throw new MasterMindGameOverException(msg);
            }
            Entry newGuess = new Entry(guess, ColorCount); 
            Peg newPeg = new Peg(newGuess, secret);
            Bord[GuessCount] = newGuess;
            Pegs[GuessCount] = newPeg;
            GuessCount++;
            if (newPeg.Won()) {
                Status = GameStatus.Won;
            } else if (GuessCount == RowCount) {
                Status = GameStatus.Lost;
            }
            return newPeg;
        }
    }


    class Peg {
        // attributes
        private int max;

        // properties
        public int Black { get; set; } = -1;
        public int White { get; set; } = -1;

        // constructors
        public Peg() { }

        public Peg(int black, int white) {
            Set(black, white);
        }

        public Peg(int black, int white, int max) {
            Set(black, white);
            this.max = max;
        }

        public Peg(Entry guess, Entry secret) {
            Guess(guess, secret);
        }

        // methods
        public bool Won() {
            return Won(max);
        }

        public bool Won(int max) {
            if (Black == max) {
                return true;
            }
            return false;
        }

        public void Set(int black, int white) {
            Black = black;
            White = white;
        }

        public void Set(int black, int white, int max) {
            Set(black, white);
            this.max = max;
        }

        public bool IsSet() {
            return Black == -1;
        }

        public void Guess(Entry guess, Entry secret) {
            int black = 0;
            int white = 0;
            guess = guess.Clone();
            secret = secret.Clone();
            
            // calc pos hits (black pegs)
            for(int i = 0; i < secret.Vector.Length; i++) {
                int color = guess.Vector[i];
                if (color == secret.Vector[i]) {
                    black += 1;
                    // remove 'black' hits from the secret
                    // so you only get color points for unmatched colors
                    secret.ColorCount[color]--;
                    guess.ColorCount[color]--;
                }
            }
            
            // calc color hits (white pegs)
            for(int i = 0; i < secret.ColorCount.Length; i++) {
                int m = Math.Min(guess.ColorCount[i], secret.ColorCount[i]);
                if (m > 0) {
                    white += m;
                }
            }
            Set(black, white, guess.Vector.Length);
        }
    }


    class Entry {
        // properties
        public int[] Vector { get; private set; }
        public int[] ColorCount { get; private set; }

        // constructor
        public Entry(int[] colorVector, int nColors) {
            Vector = (int[]) colorVector.Clone();
            ColorCount = new int[nColors];
            foreach(int color in colorVector) {
                ColorCount[color]++;
            }
        }

        public Entry(int[] vector, int[] colorCount) {
            Vector = (int[]) vector.Clone();
            ColorCount = (int[]) colorCount.Clone();
        }

        public Entry Clone() {
            return new Entry(Vector, ColorCount);
        }
    }

    public class MasterMindGameOverException: Exception {
        public MasterMindGameOverException() { }
        public MasterMindGameOverException(string message) : base(message) { }
        // public MasterMindGameOverException(string message, Exception inner)
            // : base(message, inner) { }
    }

}
