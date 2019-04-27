using System.Collections.Generic;
using System;

namespace QUT.CSharpTicTacToe
{
    public class Game : ITicTacToeGame<Player>
    {
        public int Size;
        public Player Turn;
        public List<Move> path;
        public int[] lines;
        public Player evenPlayer;
        public Player oddPlayer;
        public List<Move> squares;
        private int winningSumEven;
        private int winningSumOdd;
        public Move[] diag1;
        public Move[] diag2;

        int ITicTacToeGame<Player>.Size => this.Size;
        Player ITicTacToeGame<Player>.Turn => (this.path.Count % 2 == 0) ? evenPlayer : oddPlayer;

        /// <summary>
        /// Constructor for the game. Inititates game variables for a new game.
        /// Setting the size, who's min (odd) or max (even) and the winning
        /// moves for the game.
        /// </summary>
        /// <param name="first">The first player set as the even Player (max player)</param>
        /// <param name="size">The board size of the game</param>
        public Game(Player first, int size)
        {
            evenPlayer = first;
            if (evenPlayer == Player.Cross) oddPlayer = Player.Nought; else oddPlayer = Player.Cross;
            this.Turn = evenPlayer;
            this.Size = size;
            diag1 = new Move[size];
            diag2 = new Move[size];
            for (int i = 0; i < size; i++)
            {
                diag1[i] = new Move(i, i);
                diag2[i] = new Move(i, (size - i - 1));
            }
            lines = new int[2 * size + 2];
            squares = new List<Move>();
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    squares.Add(new Move(row, col));
                }
            }
            winningSumEven = size * (size + 1) / 2;
            winningSumOdd = 100 * size * (size + 1) / 2;
            path = new List<Move>();
        }

        /// <summary>
        /// Used when applying a new move to the game, making sure values are consistent.
        /// </summary>
        /// <param name="g">The current game played</param>
        public Game(Game g)
        {
            this.evenPlayer = g.evenPlayer;
            this.oddPlayer = g.oddPlayer;
            this.Turn = this.evenPlayer;
            this.Size = g.Size;
            this.diag1 = new Move[g.diag1.Length];
            Array.Copy(g.diag1, this.diag1, g.diag1.Length);
            this.diag2 = new Move[g.diag2.Length];
            Array.Copy(g.diag2, this.diag2, g.diag2.Length);
            this.lines = new int[g.lines.Length];
            Array.Copy(g.lines, this.lines, g.lines.Length);
            this.squares = new List<Move>(g.squares);
            this.winningSumEven = g.winningSumEven;
            this.winningSumOdd = g.winningSumOdd;
            this.path = new List<Move>(g.path);
        }

        /// <summary>
        /// Checks if the current game has ended in a draw
        /// </summary>
        /// <returns></returns>
        public bool IsDraw()
        {
            bool lineIsDraw(int sum)
            {
                if ((sum > 100) & (sum % 100) != 0) { return true; }
                return false;
            }

            bool linesChecker = true;
            foreach (int i in lines)
            {
                if (lineIsDraw(i) != true) { linesChecker = false; }
            }
            if (linesChecker) { return true; }
            return false;
        }

        /// <summary>
        /// Checks the winner of the current game.
        /// </summary>
        /// <returns></returns>
        public Player Winner()
        {
            if (((IList<int>)lines).Contains(this.winningSumEven))
            {
                return evenPlayer;
            }
            else if (((IList<int>)lines).Contains(this.winningSumOdd))
            {
                return oddPlayer;
            }
            return Player.No;
        }

        /// <summary>
        /// Obtains the score of the winning player.
        /// </summary>
        /// <param name="player">The player who potentially won the game</param>
        /// <returns></returns>
        public int Score(Player player)
        {
            var winner = this.Winner();
            if (winner == Player.No)
            {
                if (this.IsDraw())
                {
                    return 0;
                }
            }
            else
            {
                if (player == winner) return 1;
                else return -1;
            }

            throw new Exception("No Score");
        }

        /// <summary>
        /// Returns the current size of the game.
        /// </summary>
        public int TheSize
        {
            get { return this.Size; }
        }

        /// <summary>
        /// Get and set's the current player's turn
        /// </summary>
        public Player WhosTurn
        {
            get
            {
                if (this.path.Count % 2 == 0) { return this.evenPlayer; }
                return this.oddPlayer;
            }

            set { this.Turn = value; }
        }

        /// <summary>
        /// Obtains the player's piece ('X' or 'O') on the game board and 
        /// returns the string "X" or "O"
        /// </summary>
        /// <param name="row">Row where the player's piece is located</param>
        /// <param name="col">Column where the player's piece is located</param>
        /// <returns></returns>
        public string getPiece(int row, int col)
        {
            String piece(Player player)
            {
                if (player == Player.Cross) { return "X"; }
                else if (player == Player.Nought) { return "O"; }
                return null;
            }
            int n = this.path.FindIndex(a => a.Row == row && a.Col == col);
            if (n == -1) { return ""; }
            else if (n % 2 == 0) { return piece(evenPlayer); }
            else { return piece(oddPlayer); }
        }

    }
}