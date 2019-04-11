using System.Collections.Generic;
using System;

namespace QUT.CSharpTicTacToe
{
    /// <summary>
    /// This represents the current state of the game, 
    /// including the size of the game (NxN), 
    /// who's turn it is and the pieces on the board
    /// </summary>
    /// 
    public class Game : ITicTacToeGame<Player>
    {
        public int Size { get; set; }
        public Player Turn { get; set; }

        public Game(Player first, int size)
        {
            this.Turn = first;
            this.Size = size;
        }

        public string getPiece(int row, int col)
        {
            //switch case Cross -> X, nouht -> O

            //switch match empty list (path) with given row and column
            throw new System.NotImplementedException("getPiece");
        }
    }
}





//    public class GameStart
//    {
//        public int Size;
//        public Player evenPlayer;
//        public Player oddPlayer;
//    }

//    public class Game : ITicTacToeGame<Player>
//    {
//        public List<Move> path;
//        public int[] lines;

//        //add constructor
//        public string getPiece(int row, int col)
//        {
//            //int array[] = new Array<int> { row, col };

//            //if (Array is Not empyty)
//            //{
//            //    return Array.ToString()
//            //}
//            //return None;
//            //return array.ToString();
//            //returns a string
//            //should this return all the values of the player
//            throw new System.NotImplementedException("ApplyMove");

//        }

//        public Player Turn => this.path.Count % 2 == 0 ? Pars.evenPlayer : Pars.oddPlayer;
//        public Player Turn1
//        {
//            get
//            {
//                return this.path.Count % 2 == 0 ? _pars.evenPlayer : _pars.oddPlayer;
//            }

//        }

//        private GameStart Pars => FSharpImpureTicTacToeModel.pars;


//    }

//    public static class FSharpImpureTicTacToeModel
//    {
//        public static GameStart pars;
//        public static Game game;
//    }
//}

//Gamestart(Player firstPlayer, int size)
//    FSharpImpureTicTacToeModel.pars.size = size;