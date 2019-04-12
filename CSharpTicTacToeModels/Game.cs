using System.Collections.Generic;
using System;

namespace QUT.CSharpTicTacToe
{
    /// <summary>
    /// This represents the current state of the game, 
    /// including the size of the game (NxN), 
    /// who's turn it is and the pieces on the board
    /// </summary>    
    public class Game : ITicTacToeGame<Player>
    {
        public int Size;
        public Player Turn;
        public List<Move> path;
        private int[] lines;
        private Player evenPlayer;
        private Player oddPlayer;
        public List<Move> squares;
        private int winningSum;
        private Move diag1;
        private Move diag2;

        int ITicTacToeGame<Player>.Size => this.Size;
        Player ITicTacToeGame<Player>.Turn => (this.path.Count % 2 == 0) ? evenPlayer : oddPlayer;

        public Game(Player first, int size)
        {
            this.Turn = first;
            this.Size = size;
        }

        public string getPiece(int row, int col)
        {
            
            //find index that matches the row and column
                        
            //switch match empty list (path) with given row and column
            throw new System.NotImplementedException("getPiece");
        }

        public bool IsDraw()
        {
            if (this.path.Count == this.Size * this.Size)
            {
                return true;
            }
            return false;
        }

        public Player Winner()
        {
            if (((IList<int>)lines).Contains(this.winningSum)){
                return evenPlayer;
            }
            else if (((IList<int>)lines).Contains((-this.winningSum)))
            {
                return oddPlayer;
            }
            return 0;
        }

        public int Score(Player player)
        {
            Player theWinner = Winner();
            if (theWinner == 0)
            {
                return 0;
            }
            return 1;
        }

        public int TheSize
        {
            get
            {
                return this.Size;
            }

            set
            {
                this.Size = value;
            }
        }

        public Player WhosTurn
        {
            get
            {
                if (this.path.Count % 2 == 0)
                {
                    return this.evenPlayer;
                }
                return this.oddPlayer;
            }

            set
            {
                this.Turn = value;
            }
        }
    }
}