using System;
using System.Collections.Generic;
using System.Linq;

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
            return "Impure C# with Alpha Beta Pruning";
        }

        public Game GameStart(Player first, int size)
        {
            Game new_game = new Game(first, size);
            if (first == Cross) { new_game.oddPlayer = Nought; }
            else if (first == Nought) { new_game.oddPlayer = Cross; }
            return new_game;
        }

        public Move FindBestMove(Game game)
        {
            bool isMax = true;
            if (getTurn(game) == game.evenPlayer) { isMax = true; }
            else { isMax = false; }
            NodeCounter.Reset();
            (Move m, int i) best = MiniMaxWithAlphaBetaPruning(-1, 1, game, isMax, game.evenPlayer);
            return best.m;
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
            //Game nextState = game;
            Game nextState = new Game(game);
            int[] l1 = nextState.lines;
            int factor = (nextState.path.Count % 2 == 0) ? 1 : 100;
            int rowLine = move.Row;
            int colLine = nextState.Size + move.Col;
            bool isDiag1 = Array.Exists(nextState.diag1, element => element.Col == move.Col && element.Row == move.Row);
            bool isDiag2 = Array.Exists(nextState.diag2, element => element.Col == move.Col && element.Row == move.Row);
            l1[rowLine] = (l1[rowLine] + factor * (move.Col + 1));
            l1[colLine] = (l1[colLine] + factor * (move.Row + 1));
            if (isDiag1) { l1[2 * nextState.Size] = l1[2 * nextState.Size] + factor * (move.Col + 1); }
            if (isDiag2) { l1[2 * nextState.Size + 1] = l1[2 * nextState.Size + 1] + factor * (move.Row + 1); }
            var square = game.squares.Find(sq => sq.Col == move.Col && sq.Row == move.Row);
            nextState.path.Add(square);
            return nextState;
        }

        public TicTacToeOutcome<Player> GameOutcome(Game game)
        {

            IEnumerable<System.Tuple<int, int>> winningSquares()
            {

                return default(IEnumerable<System.Tuple<int, int>>);
            }

            if (game.IsDraw()) { return TicTacToeOutcome<Player>.Draw; }
            else
            {
                if (game.Winner() == Nought)
                {
                    return TicTacToeOutcome<Player>.NewWin(Player.Nought, winningSquares());
                }
                else if (game.Winner() == Cross)
                {
                    return TicTacToeOutcome<Player>.NewWin(Player.Cross, winningSquares());
                }
            }
            return (TicTacToeOutcome<Player>.Undecided);

        }
        public List<List<(int, int)>> Lines(int size)
        {
            List<List<(int, int)>> winningLines = new List<List<(int, int)>>();
            List<(int, int)> coordinates = new List<(int, int)>();

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
            return game.Score(player);
        }

        public Player getTurn(Game game)
        {
            return game.WhosTurn;
        }

        public bool gameOver(Game game)
        {
            if (game.IsDraw() || (game.Winner() != Player.No)) { return true; }
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

        public (Move, int) MiniMaxWithAlphaBetaPruning(int alpha, int beta, Game game, bool isMax, Player perspective)
        {
            NodeCounter.Increment();
            (Move, int) finalMove;
            if (gameOver(game))
            {
                return (null, heuristic(game, perspective));
            }
            else
            {
                List<Move> moves = moveGenerator(game);
                if (moves.Count == 0)
                {
                    (Move, int) noMoves = (null, 0);
                    return noMoves;
                }

                List<(Move m, Game i)> nextGameStates = new List<(Move, Game)>();
                foreach (Move i in moves)
                {
                    nextGameStates.Add((i, ApplyMove(game, i)));
                }

                finalMove = mapScoresPrunned(alpha, beta, nextGameStates, moves, isMax, game, game.evenPlayer);
            }
            return finalMove;
        }

        public (Move, int) mapScoresPrunned(int alpha, int beta, List<(Move m, Game i)> states, List<Move> moves, bool isMax, Game game, Player perspective)
        {
            (Move m, Game g) theState = states[0];
            (Move m, int i) move = MiniMaxWithAlphaBetaPruning(alpha, beta, theState.g, !isMax, perspective);
            int score = move.i;

            int alpha1 = (isMax && score > alpha) ? score : alpha;
            int beta1 = (!isMax && score < beta) ? score : beta;
            if (alpha1 >= beta1 || states.Count == 1)
            {
                return (theState.m, score);
            }

            var tail = new List<(Move m, Game i)>(states.Skip(1));
            (Move m, int i) nextMove = mapScoresPrunned(alpha, beta, tail, moves, isMax, game, perspective);
            if (isMax)
            {
                if (score >= nextMove.i)
                {
                    return (theState.m, score);
                }
                return nextMove;
            }
            if (score <= nextMove.i)
            {
                return (theState.m, score);
            }
            return nextMove;
        }
    }
}