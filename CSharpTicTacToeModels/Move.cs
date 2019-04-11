namespace QUT.CSharpTicTacToe
{
    /// <summary>
    /// Represents a single move specified using 
    /// (row, column) coordinates of the selected square
    /// </summary>
    public class Move : ITicTacToeMove
    {
        public Move(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        public int Row { get; set; }
        public int Col { get; set; }

    }
}
