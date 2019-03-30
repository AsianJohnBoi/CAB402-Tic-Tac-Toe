namespace QUT

    module FSharpPureTicTacToeModel =
        // type to represent the two players: Noughts and Crosses
        type Player = Nought | Cross

        // type to represent a single move specified using (row, column) coordinates of the selected square
        type Move = 
            { row: int; column: int }
            interface ITicTacToeMove with
                member this.Row with get() = this.row
                member this.Col with get() = this.column

        // type to represent the current state of the game, including the size of the game (NxN), who's turn it is and the pieces on the board
        type GameState = 
            { turn: Player; size: int; board: Option<Player> [,] }
            interface ITicTacToeGame<Player> with
                member this.Turn with get()    = this.turn
                member this.Size with get()    = this.size
                member this.getPiece(row, col) = 
                    match Array2D.get this.board row col with
                    | None -> " "
                    | Some Nought -> "o"
                    | Some Cross -> "x"


        let CreateMove row col = {row = row; column = col}

        let ApplyMove (oldState:GameState) 
                      (move: Move) = 
                      raise (System.NotImplementedException("CreateMove"))

        // Returns a sequence containing all of the lines on the board: Horizontal, Vertical and Diagonal
        // The number of lines returned should always be (size*2+2)
        // the number of squares in each line (represented by (row,column) coordinates) should always be equal to size
        // For example, if the input size = 2, then the output would be: 
        //     seq [seq[(0,0);(0,1)];seq[(1,0);(1,1)];seq[(0,0);(1,0)];seq[(0,1);(1,1)];seq[(0,0);(1,1)];seq[(0,1);(1,0)]]
        // The order of the lines and the order of the squares within each line does not matter
        let Lines (size:int) : seq<seq<int*int>> = 
            //get straight row
            let StraightRow = seq { for row in 0 .. 1 do 
                                      for col in 0 ..1 do 
                                        yield (row, col)
                                  }
            printfn "%A" StraightRow

            //get straight column
            let StraightColumn = seq { for row in 0 .. 1 do 
                                        for col in 0 .. 1 do 
                                         yield (col, row)
                                         
                                  }
            printfn "%A" StraightColumn

            //get straight diagonal


            //combine all sequence into one under one sequence

            raise (System.NotImplementedException("Lines"))

            //let (height, width) = (size, size)
            //seq { for row in 0 ..width - 1 do
            //        for col in 0 .. height - 1 do
            //            yield (row, col)
            //}


        // Checks a single line (specified as a sequence of (row,column) coordinates) to determine if one of the players
        // has won by filling all of those squares, or a Draw if the line contains at least one Nought and one Cross
        let CheckLine (game:GameState) (line:seq<int*int>) : TicTacToeOutcome<Player> = raise (System.NotImplementedException("CheckLine"))

        let GameOutcome game = raise (System.NotImplementedException("GameOutcome"))

        let GameStart (firstPlayer:Player) size = Cross

        let MiniMax game = GameTheory.MiniMaxGenerator(game)

        let MiniMaxWithPruning game = GameTheory.MiniMaxWithAlphaBetaPruningGenerator(game)

        // plus other helper functions ...




        [<AbstractClass>]
        type Model() =
            abstract member FindBestMove : GameState -> Move
            interface ITicTacToeModel<GameState, Move, Player> with
                member this.Cross with get()             = Cross 
                member this.Nought with get()            = Nought 
                member this.GameStart(firstPlayer, size) = GameStart firstPlayer size
                member this.CreateMove(row, col)         = CreateMove row col
                member this.GameOutcome(game)            = GameOutcome game
                member this.ApplyMove(game, move)        = ApplyMove game move 
                member this.FindBestMove(game)           = this.FindBestMove game

        type BasicMiniMax() =
            inherit Model()
            override this.ToString()         = "Pure F# with basic MiniMax";
            override this.FindBestMove(game) = raise (System.NotImplementedException("FindBestMove"))


        type WithAlphaBetaPruning() =
            inherit Model()
            override this.ToString()         = "Pure F# with Alpha Beta Pruning";1
            override this.FindBestMove(game) = raise (System.NotImplementedException("FindBestMove"))