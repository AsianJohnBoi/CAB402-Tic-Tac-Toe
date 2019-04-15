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

        public Game GameStart(Player first, int size)
        {
            List<Move> path = new List<Move>();
            int[] lines = new int[2 * size + 2];
            Game new_game = new Game(first, size);
            new_game.evenPlayer = first;
            if (first == Cross) { new_game.oddPlayer = Nought; }
            else if (first == Nought) { new_game.oddPlayer = Cross; }
            List<Move> squares = new List<Move>();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    squares.Add(new Move(i, j));
                }
            }
            new_game.winningSumEven = size * (size + 1) / 2;
            new_game.winningSumOdd = 100 * size * (size + 1) / 2;
            for (int i = 0; i < size; i++)
            {
                new_game.diag1[i] = new Move(i, i);
                new_game.diag2[i] = new Move(i, (size - i - 1));
            }
            return new_game;
        }

        public Move FindBestMove(Game game)
        {
            bool isMax = true;
            if (getTurn(game) == game.evenPlayer) { isMax = true; }
            else { isMax = false; }
            List<(Move m, int i)> best = MiniMaxWithAlphaBetaPruning(1, 10, game, isMax);
            return best[0].m;
        }

        public Move CreateMove(int row, int col)
        {
            this.row = row;
            this.col = col;
            Move nextMove = new Move(row, col);
            return nextMove;
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

        public TicTacToeOutcome<Player> GameOutcome(Game game)
        {
            List<(int, int)> winningSquares()
            {

                throw new System.NotImplementedException("GameOutcome");
            }
            
            if (game.IsDraw()) { TicTacToeOutcome<Player> Draw; }
            else
            {
                if (game.Winner() == Nought)
                {
                    TicTacToeOutcome<Player> Win;
                }
                else if (game.Winner() == Cross)
                {
                    TicTacToeOutcome<Player> Win;
                }
            }
            //return (TicTacToeOutcome<Player> Undecided);

            throw new NotImplementedException();
        }

        public List<List<(int, int)>> Lines(int size)
        {
            List<List<(int, int)>> winningLines = new List<List<(int, int)>>();
            List<(int, int)>coordinates = new List<(int, int)>();

            //row append
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    coordinates.Add((i, j));
                    winningLines.Add(coordinates);//add the list to winningLines list
                    coordinates.Clear(); //flush the coordinates list
                }
            }

            //column append
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    coordinates.Add((j, i));
                    winningLines.Add(coordinates);
                    coordinates.Clear();
                }
            }

            //Diagonal from top left to bottom right
            for (int i = 0; i < size; i++)
            {
                coordinates.Add((i, i));
                winningLines.Add(coordinates);
                coordinates.Clear();
            }

            //Diagonal from top right to bottom left
            for (int i = 0; i < size; i++)
            {
                coordinates.Add((i, size - i - 1));
                winningLines.Add(coordinates);
                coordinates.Clear();
            }
            return winningLines;
        }

        public int heuristic(Game game, Player player)
        {
            int score = game.Score(player);
            if (score > 0) { return score; }
            return 0;
        }

        public Player getTurn(Game game)
        {
            return game.Turn;
        }

        public bool gameOver(Game game)
        {
            if (game.IsDraw() || (game.Winner() != 0)) { return true; }
            return false;
        }

        public List<Move> moveGenerator(Game game)
        {
            List<Move> moves = new List<Move>();
            foreach (Move i in game.squares)
            {
                if (!game.path.Contains(i)) { moves.Add(i); }
            }
            return moves;
        }

        public List<(Move, int)> MiniMaxWithAlphaBetaPruning(int alpha, int beta, Game game, bool isMax)
        {
            NodeCounter.Increment();
            if (gameOver(game))
            {
                List<(Move, int)> gameOverList = new List<(Move, int)> { (null, heuristic(game, getTurn(game))) };
                return gameOverList;
            }
            else
            {
                List<Move> moves = moveGenerator(game);
                if (moves.Count == 0)
                {
                    List<(Move, int)> noMoves = new List<(Move, int)> { (null, 0) };
                    return noMoves;
                }

                List<(Move m, Game i)> nextGameStates = new List<(Move, Game)>();
                foreach (Move i in moves) { nextGameStates.Add((i, ApplyMove(game, i))); }

                mapScoresPrunned(alpha, beta, nextGameStates, moves, isMax, game);
            }
            List<(Move, int)> noMove = new List<(Move, int)> { (null, 0) };
            return noMove;
        }

        public List<(Move, int)> mapScoresPrunned(int alpha, int beta, List<(Move m, Game i)> states, List<Move> moves, bool isMax, Game game)
        {
            (Move m, Game g) = states[0];
            List<(Move m, int i)> move = MiniMaxWithAlphaBetaPruning(alpha, beta, g, !isMax);
            int score = move[0].i;

            int alpha1 = (isMax && score > alpha) ? score : alpha;
            int beta1 = (!isMax && score < beta) ? score : alpha;
            if (alpha1 >= beta1 || (states[states.Count - 1].m == null && (states[states.Count - 1].i == null)))
            {
                List<(Move, int)> alphaIsGreatOrEqual = new List<(Move, int)> { (m, score) };
                return alphaIsGreatOrEqual;
            }
            List<(Move, Game)> newState = new List<(Move, Game)> { states[states.Count - 1] };
            List<(Move m, int i)> nextMove = mapScoresPrunned(alpha, beta, states, moves, isMax, game);
            if (isMax)
            {
                if (score >= nextMove[0].i)
                {
                    List<(Move, int)> MaxScore = new List<(Move, int)> { (m, score) };
                    return MaxScore;
                }
                return nextMove;
            }
            if (score <= nextMove[0].i)
            {
                List<(Move, int)> MaxScore = new List<(Move, int)> { (m, score) };
                return MaxScore;
            }
            return nextMove;
        }
    }
}