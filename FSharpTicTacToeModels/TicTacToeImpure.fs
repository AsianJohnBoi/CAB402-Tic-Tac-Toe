namespace QUT

    module FSharpImpureTicTacToeModel =
    
        // type to represent the two players: Noughts and Crosses
        type Player = Nought | Cross

        // type to represent a single move specified using (row, column) coordinates of the selected square
        type Move = 
            { 
                row: int
                col: int
            }
            interface ITicTacToeMove with
                member this.Row with get() = this.row
                member this.Col with get() = this.col

        // type to represent a new game with size board, who's odd or even player, winning moves.
        type private GameStart = 
            {
                size: int
                evenPlayer : Player
                oddPlayer : Player
                squares: Move list
                winningSumEven: int
                winningSumOdd: int
                diag1: Move array
                diag2: Move array
            }                

        let mutable private pars = Unchecked.defaultof<GameStart>

        // type to represent the current state of the game, including the size of the game (NxN), who's turn it is and the pieces on the board
        type GameState = 
            { 
                path: Move list
                lines: int array
            } 

            static member Create size player = 
                {
                    path = []
                    lines = Array.create (2 * pars.size + 2) 0
                }
            
            member this.IsDraw() = 
                let lineIsDraw sum = sum > 100 && sum % 100 <> 0
                this.lines |> Array.forall lineIsDraw

            member this.Winner
                with get() =
                    if this.lines |> Array.contains pars.winningSumEven then Some pars.evenPlayer
                    elif this.lines |> Array.contains pars.winningSumOdd then Some pars.oddPlayer
                    else None

            member this.Score player =
                match this.Winner with
                | Some pl -> Some(if pl = player then 1 else -1)
                | _ -> if this.IsDraw() then Some 0 else None
            
            interface ITicTacToeGame<Player> with
                member this.Turn with get()    = if this.path.Length % 2 = 0 then pars.evenPlayer else pars.oddPlayer

                member this.Size with get()    = pars.size

                member this.getPiece(row, col) = 
                    let piece = 
                        function
                        | Cross -> "X"
                        | Nought -> "O"
                    match this.path |> List.tryFindIndex ((=) {row = row; col = col}) with
                    | None -> ""
                    | Some n -> if (this.path.Length - 1 - n) % 2 = 0 then piece pars.evenPlayer else piece pars.oddPlayer
        
        // Returns a sequence containing all of the lines on the board: Horizontal, Vertical and Diagonal
        // The number of lines returned should always be (size*2+2)
        // the number of squares in each line (represented by (row,column) coordinates) should always be equal to size
        // For example, if the input size = 2, then the output would be: 
        //     seq [seq[(0,0);(0,1)];seq[(1,0);(1,1)];seq[(0,0);(1,0)];seq[(0,1);(1,1)];seq[(0,0);(1,1)];seq[(0,1);(1,0)]]
        let Lines (size:int) : seq<seq<int*int>> = 
            seq  {
                for i in 0 .. (size - 1) do
                    yield seq {
                        for j in 0 .. (size - 1) do
                            yield (i, j)
                    }
                for i in 0 .. (size - 1) do
                    yield seq {
                        for j in 0 .. (size - 1) do
                            yield (j, i)
                    }
                yield seq {
                    for i in 0 .. (size - 1) do
                        yield (i, i)
                }
                yield seq {
                    for i in 0 .. (size - 1) do
                        yield (i, size - i - 1)
                }
            }
        
        // Checks a single line (specified as a sequence of (row,column) coordinates) to determine if one of the players
        // has won by filling all of those squares, or a Draw if the line contains at least one Nought and one Cross
        let GameOutcome (game: GameState) = 
            let winningSquares() = 
                let winningLine = game.lines |> Array.findIndex (fun v -> v = pars.winningSumEven || v = pars.winningSumOdd)
                Lines pars.size |> Seq.item winningLine 

            match game with
            | game when game.IsDraw() -> TicTacToeOutcome<Player>.Draw
            | game ->
                match game.Winner with
                | None -> TicTacToeOutcome<Player>.Undecided
                | Some player -> TicTacToeOutcome<Player>.Win (player, winningSquares())

        let ApplyMove (game: GameState) (move: Move)  = 
            let l1 = game.lines |> Array.copy
            let factor = if game.path.Length % 2 = 0 then 1 else 100
            let rowLine = move.row
            let colLine = pars.size + move.col
            let isDiag1 = pars.diag1 |> Array.contains move
            let isDiag2 = pars.diag2 |> Array.contains move
            l1.[rowLine] <- l1.[rowLine] + factor * (move.col + 1)
            l1.[colLine] <- l1.[colLine] + factor * (move.row + 1)
            if isDiag1 then l1.[2 * pars.size] <- l1.[2 * pars.size] + factor * (move.col + 1)          
            if isDiag2 then l1.[2 * pars.size + 1] <- l1.[2 * pars.size + 1] + factor * (move.row + 1)
            { game with path = move :: game.path; lines = l1 }

        let CreateMove (row: int) (col: int) = { row = row; col = col }

        let GameStart (player: Player) (size: int) = 
            pars <- 
                { 
                size = size
                evenPlayer = player
                oddPlayer = match player with | Cross -> Nought | Nought -> Cross 
                squares = [
                    for i in 0 .. (size - 1) do
                        for j in 0 .. (size - 1) do
                            yield { row = i; col = j }
                ]
                winningSumEven = size * (size + 1) / 2
                winningSumOdd = 100 * size * (size + 1) / 2
                diag1 = [| for i in 0 .. (size - 1) do yield { row = i; col = i } |]
                diag2 = [| for i in 0 .. (size - 1) do yield { row = i; col = size - i - 1} |]
                }
            GameState.Create size player

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


        type WithAlphaBetaPruning() =
            inherit Model()
            override this.ToString()         = "Impure F# with Alpha Beta Pruning";
            override this.FindBestMove(game) = 
                let MiniMax = 
                    let heuristic (game: GameState) (player: Player) = 
                        match game.Score player with
                        | Some n -> n
                        | _ -> raise(System.Exception("No Score"))

                    let getTurn (game: GameState) = (game :> ITicTacToeGame<Player>).Turn

                    let gameOver (game: GameState) = 
                        game.IsDraw() || game.Winner.IsSome

                    let moveGenerator (game: GameState) = pars.squares |> List.except game.path |> Seq.ofList

                    let applyMove (game: GameState) (move: Move) = ApplyMove game move

                    GameTheory.MiniMaxWithAlphaBetaPruningGenerator heuristic getTurn gameOver moveGenerator applyMove
                
                let move, _ = MiniMax -1 1 game pars.evenPlayer 
                move.Value
