namespace QUT.CSharpTicTacToe
{
    public class WithAlphaBetaPruning : ITicTacToeModel<Game, Move, Player>
    {
        public Player Cross => Player.Cross;
        public Player Nought => Player.Nought;

        public override string ToString()
        {
            return "Impure C# with Alpha Beta Pruning";
        }
        public Game ApplyMove(Game game, Move move)
        {
            throw new System.NotImplementedException("ApplyMove"); 
            //return new game
        }
        public Move CreateMove(int row, int col)
        {
            throw new System.NotImplementedException("CreateMove");
        }
        public Move FindBestMove(Game game)
        {
            throw new System.NotImplementedException("FindBestMove");
        }
        public TicTacToeOutcome<Player> GameOutcome(Game game)
        {
            throw new System.NotImplementedException("GameOutcome");
        }
        public Game GameStart(Player first, int size)
        {
            //Game new_game = first, size;
            //return new_game;
            throw new System.NotImplementedException("ApplyMove");
        }
    }
}