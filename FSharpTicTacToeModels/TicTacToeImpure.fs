namespace QUT

    module FSharpImpureTicTacToeModel =
    
        type Player = Nought | Cross

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

        type Move = 
            { row: int; column: int }
            interface ITicTacToeMove with
                member this.Row with get() = this.row
                member this.Col with get() = this.column

        let GameOutcome game     = raise (System.NotImplementedException("GameOutcome"))

        let ApplyMove game move  = raise (System.NotImplementedException("ApplyMove"))

        let CreateMove row col   = raise (System.NotImplementedException("CreateMove"))

        let FindBestMove game    = raise (System.NotImplementedException("FindBestMove"))

        let GameStart first size = raise (System.NotImplementedException("GameStart"))

        // plus other helper functions ...




        type WithAlphaBetaPruning() =
            override this.ToString()         = "Impure F# with Alpha Beta Pruning";
            interface ITicTacToeModel<GameState, Move, Player> with
                member this.Cross with get()             = Cross
                member this.Nought with get()            = Nought
                member this.GameStart(firstPlayer, size) = GameStart firstPlayer size
                member this.CreateMove(row, col)         = CreateMove row col
                member this.GameOutcome(game)            = GameOutcome game 
                member this.ApplyMove(game, move)        = ApplyMove game  move
                member this.FindBestMove(game)           = FindBestMove game