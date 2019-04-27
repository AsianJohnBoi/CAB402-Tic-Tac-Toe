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

                let rec EstablishScore (game : 'Game) (isMax: bool) = 
                    NodeCounter.Increment()
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
                NodeCounter.Reset()
                EstablishScore game (getTurn game = perspective)
            MiniMax

        let MiniMaxWithAlphaBetaPruningGenerator (heuristic:'Game -> 'Player -> int) 
                                                 (getTurn: 'Game -> 'Player) 
                                                 (gameOver:'Game->bool) 
                                                 (moveGenerator: 'Game->seq<'Move>) 
                                                 (applyMove: 'Game -> 'Move -> 'Game) 
                                                 : int -> int -> 'Game -> 'Player -> Option<'Move> * int =
            // Optimized MiniMax algorithm that uses alpha beta pruning to eliminate parts of the search tree that don't need to be explored            
            let rec MiniMax (alpha: int) (beta: int) oldState perspective : Option<'Move> * int = //alpha tracks lowest possible score. beta tracks highest possible score of node
                
                let rec EstablishScore (alpha:int) (beta:int) (game : 'Game) (isMax: bool) : Option<'Move> * int = 
                    NodeCounter.Increment()
                    if gameOver game then None, (heuristic game perspective) //player's final score
                    else 
                        let moves = moveGenerator game //Sequence of empty spaces on the board
                        if Seq.isEmpty moves then
                            None, 0 
                        else
                            let nextGameStates = 
                                moves
                                |> Seq.map (fun m -> m, applyMove game m) //Add next game states with applied moves

                            let rec mapScoresPrunned alpha beta (states: seq<'Move * 'Game>) : Option<'Move> * int = 
                                let m, g = Seq.head states
                                let move = EstablishScore alpha beta g (not isMax) //recursive call
                                let score = snd move //The score of the move

                                let alpha' = if isMax && score > alpha then score else alpha //Comparison of current player (if max) && score
                                let beta' = if not isMax && score < beta then score else beta //Comparison of current player (if not max) && score
                                if alpha' >= beta' || Seq.isEmpty (Seq.tail states) then  //If the value of alpha1 is greater than or equal to beta or there's only one state in the given sequence
                                    Some m, score //returns the final move and its score
                                else
                                    let nextMove = mapScoresPrunned alpha' beta' (Seq.tail states) //runs function again with the last value of states sequence
                                    if isMax then
                                        if score >= snd nextMove then Some m, score else nextMove //returns the best move for the max player
                                    else
                                        if score <= snd nextMove then Some m, score else nextMove //returns the best move for the min player

                            mapScoresPrunned alpha beta nextGameStates

                EstablishScore alpha beta oldState (getTurn oldState = perspective)

            NodeCounter.Reset()
            MiniMax