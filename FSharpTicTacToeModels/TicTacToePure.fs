namespace QUT

module FSharpPureTicTacToeModel =

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

    // type to represent the current state of the game, including the size of the game (NxN), who's turn it is and the pieces on the board
    type GameState = 
        { 
            path: Move list
            lines: int array
            size: int
            evenPlayer : Player
            oddPlayer : Player
            squares: Move list
            winningSum: int
            diag1: Move array
            diag2: Move array
        }

        member this.IsDraw() = this.path.Length = this.size * this.size

        member this.Winner
            with get() =
                if this.lines |> Array.contains this.winningSum then Some this.evenPlayer
                elif this.lines |> Array.contains (-this.winningSum) then Some this.oddPlayer
                else None

        member this.Score player = 
            match this.Winner with
            | Some pl -> Some(if pl = player then 1 else -1)
            | _ ->  if (this.path.Length = this.size * this.size) then Some 0 else None

        interface ITicTacToeGame<Player> with
            member this.Turn with get() = if this.path.Length % 2 = 0 then this.evenPlayer else this.oddPlayer

            member this.Size with get() = this.size

            member this.getPiece(row, col) = 
                let piece = 
                    function 
                    | Cross -> "X"
                    | Nought -> "O"

                match this.path |> List.tryFindIndex ((=) {row = row; col = col}) with
                | None -> ""
                | Some n -> if (this.path.Length - 1 - n) % 2 = 0 then piece this.evenPlayer else piece this.oddPlayer


    let CreateMove row col = { row = row; col = col }

    let ApplyMove (oldState:GameState) (move: Move) = 
        let l1 = oldState.lines |> Array.copy
        let factor = if oldState.path.Length % 2 = 0 then 1 else -1
        let rowLine = move.row
        let colLine = oldState.size + move.col
        let isDiag1 = oldState.diag1 |> Array.contains move
        let isDiag2 = oldState.diag2 |> Array.contains move
        l1.[rowLine] <- l1.[rowLine] + factor * (move.col + 1)
        l1.[colLine] <- l1.[colLine] + factor * (move.row + 1)
        if isDiag1 then l1.[2 * oldState.size] <- l1.[2 * oldState.size] + factor * (move.col + 1)          
        if isDiag2 then l1.[2 * oldState.size + 1] <- l1.[2 * oldState.size + 1] + factor * (move.row + 1)
        { oldState with path = move :: oldState.path; lines = l1 }

    // Returns a sequence containing all of the lines on the board: Horizontal, Vertical and Diagonal
    // The number of lines returned should always be (size*2+2)
    // the number of squares in each line (represented by (row,column) coordinates) should always be equal to size
    // For example, if the input size = 2, then the output would be: 
    //     seq [seq[(0,0);(0,1)];seq[(1,0);(1,1)];seq[(0,0);(1,0)];seq[(0,1);(1,1)];seq[(0,0);(1,1)];seq[(0,1);(1,0)]]
    // The order of the lines and the order of the squares within each line does not matter
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
    let CheckLine (game:GameState) (line:seq<int*int>) : TicTacToeOutcome<Player> = raise (System.NotImplementedException("CheckLine"))

    let GameOutcome (game: GameState) = 
        let winningSquares() = 
            let winningLine = game.lines |> Array.findIndex (fun v -> abs(v) = game.winningSum)
            Lines game.size |> Seq.item winningLine 

        match game with
        | game when game.IsDraw() -> TicTacToeOutcome<Player>.Draw
        | game ->
            match game.Winner with
            | None -> TicTacToeOutcome<Player>.Undecided
            | Some player -> TicTacToeOutcome<Player>.Win (player, winningSquares())


    let MiniMaxWithPruning game = raise (System.NotImplementedException("MiniMaxWithPruning"))

    // plus other helper functions ...

    [<AbstractClass>]
    type Model() =
        abstract member FindBestMove : GameState -> Move
        interface ITicTacToeModel<GameState, Move, Player> with
            member this.Cross with get()             = Cross 
            member this.Nought with get()            = Nought 
            member this.GameStart(firstPlayer, size) = 
                    { 
                    path = [] 
                    lines = Array.create (2 * size + 2) 0
                    size = size
                    evenPlayer = firstPlayer
                    oddPlayer = match firstPlayer with | Cross -> Nought | Nought -> Cross 
                    squares = [
                        for i in 0 .. (size - 1) do
                            for j in 0 .. (size - 1) do
                                yield { row = i; col = j }
                    ]
                    winningSum = size * (size + 1) / 2
                    diag1 = [| for i in 0 .. (size - 1) do yield { row = i; col = i } |]
                    diag2 = [| for i in 0 .. (size - 1) do yield { row = i; col = size - i - 1} |]
                    }
                

            member this.CreateMove(row, col)         = CreateMove row col
            member this.GameOutcome(game)            = GameOutcome game
            member this.ApplyMove(game, move)        = ApplyMove game move 
            member this.FindBestMove(game)           = this.FindBestMove game

    type BasicMiniMax() =
        inherit Model()
        override this.ToString()         = "Pure F# with basic MiniMax";
        override this.FindBestMove(game) = 
            let MiniMax = 
                let heuristic (game: GameState) player = 
                    match game.Score player with
                    | Some n -> n
                    | _ -> raise(System.Exception("No Score"))

                let getTurn (game: GameState) = (game :> ITicTacToeGame<Player>).Turn

                let gameOver (game: GameState) = 
                    game.path.Length = game.size * game.size || game.Winner.IsSome

                let moveGenerator (game: GameState) = game.squares |> List.except game.path |> Seq.ofList

                let applyMove (game: GameState) (move: Move) = ApplyMove game move

                GameTheory.MiniMaxGenerator heuristic getTurn gameOver moveGenerator applyMove

            let move, _ = MiniMax game game.evenPlayer
            move.Value

    type WithAlphaBetaPruning() =
        inherit Model()
        override this.ToString()         = "Pure F# with Alpha Beta Pruning";
        override this.FindBestMove(game) = 
            let MiniMax = 
                let heuristic (game: GameState) player = 
                    match game.Score player with
                    | Some n -> n
                    | _ -> raise(System.Exception("No Score"))

                let getTurn (game: GameState) = (game :> ITicTacToeGame<Player>).Turn

                let gameOver (game: GameState) = 
                    game.path.Length = game.size * game.size || game.Winner.IsSome

                let moveGenerator (game: GameState) = game.squares |> List.except game.path |> Seq.ofList

                let applyMove (game: GameState) (move: Move) = ApplyMove game move

                GameTheory.MiniMaxWithAlphaBetaPruningGenerator heuristic getTurn gameOver moveGenerator applyMove

            let move, _ = MiniMax 1 10 game game.evenPlayer
            move.Value



