namespace QUT.CSharpTicTacToe
{
    /// <summary>
    /// Represents a single move specified using 
    /// (row, column) coordinates of the selected square
    /// </summary>
    public class Move : ITicTacToeMove
    {
        public int Row => Row;
        public int Col => Col;
    }
}
