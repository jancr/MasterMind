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

namespace MasterMind {
    class MasterMindConsle {
        private static MasterMind Game = new MasterMind();
        private static string guessStr;

        private static void Help() {
            Console.WriteLine("Welcome to the MasterMind Game\n");

            Console.WriteLine($"There are {Game.ColorCount} colors in the game");
            Console.WriteLine($"you have {Game.RowCount} guesses to guess the secret code");
            Console.WriteLine($"The possible code values range from 0-{Game.ColorCount-1}\n");

            Console.WriteLine($"Enter your guess as a {Game.ColCount} consequative integers such as 1325");
            Console.WriteLine("The game will tell you how manny were in the corect possition");
            Console.WriteLine("and how manny has the correct color in the following format");
            Console.WriteLine("Guess Black/White");
            Console.WriteLine("Where Black is number of correct positional hits");
            Console.WriteLine("and White is the number of hits who has correct color but wrong position\n");
        }

        private static int[] ParseGuess() {
            int[] guess = new int[Game.ColCount];

            // Console.Write("Guess The secret code: ");
            do {
                Console.Write($"Guess The secret code (length {Game.ColCount}): ");
                guessStr = Console.ReadLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine();
            } while(guessStr.Length != Game.ColCount);
            for(int i = 0; i < guess.Length; i++) {
                guess[i] = int.Parse(guessStr[i].ToString());
            }
            return guess;
        }

        private static void ClearCurrentConsoleLine() {
            // inspired by https://stackoverflow.com/questions/8946808/can-console-clear-be-used-to-only-clear-a-line-instead-of-whole-console
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth)); 
            Console.SetCursorPosition(0, currentLineCursor);
        }

        private static void PrintBordLine(int[] guess, Peg peg) {
            foreach(int color in guess) {
                Console.Write(color);
            }
            Console.WriteLine(" {0}/{1}", peg.Black, peg.White);
        }


        public static void Run() {
            int[] guess;

            Help();
            // Game = new MasterMind(new int[4] {0, 4, 3, 1});
            while (true) {
                guess = ParseGuess();
                // guess = new int[] {1,1,1,1};
                Peg peg = Game.Guess(guess);
                PrintBordLine(guess, peg);
                if (peg.Won()) {
                    Console.WriteLine("Congratulation you won");
                    break;
                } else if (Game.GuessCount == Game.RowCount) {
                    Console.WriteLine("You Lost");
                    // foreach(int color in Game.Secret) {
                        // Console.Write(color);
                    // }
                    Console.WriteLine("{0} was the answer", guessStr);
                    break;
                }
            } 
            Console.WriteLine("you spend {0} guesses", Game.GuessCount);
        }
    }
}
