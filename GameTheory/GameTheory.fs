namespace QUT

    module GameTheory =
        open System.Globalization

        let MiniMaxGenerator (heuristic:'Game -> 'Player -> int)
                             (getTurn: 'Game -> 'Player) 
                             (gameOver:'Game->bool) 
                             (moveGenerator: 'Game->seq<'Move>) 
                             (applyMove: 'Game -> 'Move -> 'Game) : 'Game -> 'Player -> Option<'Move> * int = 
            // Basic MiniMax algorithm without using alpha beta pruning
            let rec MiniMax (game: 'Game) (perspective: 'Player) =
                NodeCounter.Increment()

                let rec EstablishScore (game : 'Game) (isMax: bool) = 
                    if gameOver game then 
                        let score = heuristic game perspective //player's score
                        None, score //player's final score
                    else 
                        let moves = moveGenerator game //finds moves, returns a sequence
                        if Seq.isEmpty moves then
                            None, 0 
                        else
                            let nextGameStates = 
                                moves
                                |> Seq.map (fun m -> m, applyMove game m) //creates a new sequence of elements with applied function
                            let nextGameScores = 
                                nextGameStates
                                |> Seq.map (fun (m, g) -> m, EstablishScore g (not isMax)) //creates a new sequence of elements with applied function
                            let secondElementMove = 
                                nextGameScores 
                                |> Seq.map (fun (m, r) -> m, snd r) //creates a new sequence of elements with the second element of tuple
                            let bestMove = 
                                if isMax then Seq.maxBy snd secondElementMove //Max value in sequence 
                                else Seq.minBy snd secondElementMove //Min value in sequence
                            Some (fst bestMove), snd bestMove //Some for first and second bestMove. Suitable for test 0

                EstablishScore game (getTurn game = perspective)

            NodeCounter.Reset()
            MiniMax

        let MiniMaxWithAlphaBetaPruningGenerator (heuristic:'Game -> 'Player -> int) 
                                                 (getTurn: 'Game -> 'Player) 
                                                 (gameOver:'Game->bool) 
                                                 (moveGenerator: 'Game->seq<'Move>) 
                                                 (applyMove: 'Game -> 'Move -> 'Game) 
                                                 : int -> int -> 'Game -> 'Player -> Option<'Move> * int =
            // Optimized MiniMax algorithm that uses alpha beta pruning to eliminate parts of the search tree that don't need to be explored            
            let rec MiniMax (alpha: int) (beta: int) oldState perspective : Option<'Move> * int = //alpha tracks lowest possible score. beta tracks highest possible score of node
                NodeCounter.Increment()
                
                let rec EstablishScore (alpha:int) (beta:int) (game : 'Game) (isMax: bool) : Option<'Move> * int = 
                    if gameOver game then None, (heuristic game perspective) //player's final score
                    else 
                        let moves = moveGenerator game //finds moves, returns a sequence
                        if Seq.isEmpty moves then
                            None, 0 
                        else
                            let nextGameStates = 
                                moves
                                |> Seq.map (fun m -> m, applyMove game m) //creates a new sequence of elements with applied function

                            let rec mapScoresPrunned alpha beta (states: seq<'Move * 'Game>) : Option<'Move> * int = 
                                let m, g = Seq.head states
                                let move = EstablishScore alpha beta g (not isMax)
                                let score = snd move

                                let alpha' = if isMax && score > alpha then score else alpha
                                let beta' = if not isMax && score < beta then score else beta
                                if alpha' >= beta' || Seq.isEmpty (Seq.tail states) then
                                    Some m, score
                                else
                                    let nextMove = mapScoresPrunned alpha' beta' (Seq.tail states)
                                    if isMax then
                                        if score >= snd nextMove then Some m, score else nextMove
                                    else
                                        if score <= snd nextMove then Some m, score else nextMove

                            mapScoresPrunned alpha beta nextGameStates

                EstablishScore alpha beta oldState (getTurn oldState = perspective)

            NodeCounter.Reset()
            MiniMax