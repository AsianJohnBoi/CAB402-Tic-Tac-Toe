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

            throw new System.NotImplementedException("ApplyMove"); 
        }

        public Move CreateMove(int row, int col)
        {
            this.row = row;
            this.col = col;
            Move nextMove = new Move(row, col);
            return nextMove;
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

        public Move FindBestMove(Game game)
        {
           //MiniMaxWithAlphaBetaPruningGenerator newMove = new MiniMaxWithAlphaBetaPruningGenerator(heuristic(getTurn(game), game), getTurn(game), gameOver(game), moveGenerator(game));

            //Move best = Minimax game game.evenPlayer;
            //return best;

            throw new System.NotImplementedException("FindBestMove");
        }
        public TicTacToeOutcome<Player> GameOutcome(Game game)
        {
            //if (game.IsDraw())
            //{
            //    return 0;
            //}
            //return game.Winner();
            throw new System.NotImplementedException("GameOutcome");
        }
        public Game GameStart(Player first, int size)
        {
            Game new_game = new Game(first, size);
            return new_game;
        }
    }
}