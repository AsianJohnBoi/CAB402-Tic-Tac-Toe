using System;
using System.Collections.Generic;

namespace QUT.CSharpTicTacToe
{

    public class WithAlphaBetaPruning : ITicTacToeModel<Game, Move, Player>
    {
        public Player Cross => Player.Cross;
        public Player Nought => Player.Nought;

        private int row;
        private int col;

        public override string ToString()
        {
            return "Pure C# with Alpha Beta Pruning";
        }

        public Game ApplyMove(Game game, Move move)
        {
            int[] l1 = game.lines;
            int factor = (game.path.Count % 2 == 0) ? 1 : 100;
            int rowLine = move.Row;
            int colLine = game.Size + move.Col;
            bool isDiag1 = Array.Exists(game.diag1, element => element == move);
            bool isDiag2 = Array.Exists(game.diag2, element => element == move);
            l1[rowLine] = (l1[rowLine] + factor * (move.Col + 1));
            l1[colLine] = (l1[colLine] + factor * (move.Row + 1));
            if (isDiag1) { l1[2 * game.Size] = l1[2 * game.Size] + factor * (move.Col + 1); }
            if (isDiag2) { l1[2 * game.Size + 1] = l1[2 * game.Size + 1] + factor * (move.Col + 1); }
            game.path.Add(move);
            game.lines = l1;
            return game;
        }

        public Move CreateMove(int row, int col)
        {
            this.row = row;
            this.col = col;
            Move nextMove = new Move(row, col);
            return nextMove;
        }

        public Move FindBestMove(Game game)
        {
            throw new System.NotImplementedException("FindBestMove");
        }
        public TicTacToeOutcome<Player> GameOutcome(Game game)
        {
            //int winningline = game.lines.findIndex
            throw new System.NotImplementedException("GameOutcome");
        }
        public Game GameStart(Player first, int size)
        {
            List<Move> path = new List<Move>();
            int[] lines = new int[2 * size + 2];
            Game new_game = new Game(first, size);
            new_game.evenPlayer = first;
            if (first == Cross) { new_game.oddPlayer = Nought; }
            else if (first == Nought) { new_game.oddPlayer = Cross; }
            List<Move> squares = new List<Move>();
            for (int i = 0; i < size; i ++){
                for (int j = 0; j < size; j++)
                {         
                    squares.Add(new Move(i, j));
                }  
            }
            new_game.winningSumEven = size * (size + 1) / 2;
            new_game.winningSumOdd = 100 * size * (size + 1) / 2;
            for (int i = 0; i < size; i++) {
                new_game.diag1[i] = new Move(i, i);
                new_game.diag2[i] = new Move(i, (size - i - 1));
            }
            return new_game;
        }

        public int heuristic(Player player, Game game)
        {
            int score = game.Score(player);
            if (score > 0)
            {
                return score;
            }
            return 0;
        }

        public Player getTurn(Game game)
        {
            return game.Turn;
        }

        public bool gameOver(Game game)
        {
            if (game.path.Count == game.Size * game.Size || game.IsDraw())
            {
                return true;
            }
            //else if (game.Winner)
            //{
            //    return true;
            //}
            return false;
        }

        //create a list from the game.squares and remove ones that already have the game.path.
        public List<Move> moveGenerator(Game game)
        {
            List<Move> moves = new List<Move>();
            foreach (Move i in game.squares)
            {
                if (!game.path.Contains(i))
                {
                    moves.Add(i);
                }
            }
            return moves;
        }
    }
}