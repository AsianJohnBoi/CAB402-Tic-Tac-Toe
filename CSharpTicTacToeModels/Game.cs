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
        public int Size => Size;
        public Player Turn => Turn;

        //add constructor
        public string getPiece(int row, int col)
        {
            //int array[] = new Array<int> { row, col };

            //if (Array is Not empyty)
            //{
            //    return Array.ToString()
            //}
            //return None;
            //return array.ToString();
            //returns a string
            //should this return all the values of the player
            throw new System.NotImplementedException("ApplyMove");

        }
    }
}