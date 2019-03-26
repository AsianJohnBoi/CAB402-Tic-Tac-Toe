namespace QUT

    module GameTheory =
        let MiniMaxGenerator (heuristic:'Game -> 'Player -> int) //heuriestic score for any game situation
                             (getTurn: 'Game -> 'Player) //player's turn
                             (gameOver:'Game->bool) //Whether game is over or not
                             (moveGenerator: 'Game->seq<'Move>) //enumerate all possible moves from a given name situation
                             (applyMove: 'Game -> 'Move -> 'Game) : 'Game -> 'Player -> Option<'Move> * int = //apply a move to a game situation to create a new game situation
            // Basic MiniMax algorithm without using alpha beta pruning
            let rec MiniMax (game: 'Game) (perspective: 'Player) = //gets the game and the player (Nought or Cross)
                NodeCounter.Increment()

                let rec EstablishScore (game : 'Game) (isMax: bool) = 
                    if gameOver game then
                        let score = heuristic game perspective //the score for the player
                        None, score
                    else 
                        let moves = moveGenerator game //finds moves, returns a sequence
                        if Seq.isEmpty moves then
                            None, 0 //no moves available return 0
                        else
                            let nextGameStates = 
                                moves //uses move sequence
                                |> Seq.map (fun m -> m, applyMove game m) //returns the moves available and applies the move
                            let nextGameScores = 
                                nextGameStates //gets created sequence
                                |> Seq.map (fun (m, g) -> m, EstablishScore g (not isMax)) //calls the function
                            let secondElementMove = 
                                nextGameScores //sequence
                                |> Seq.map (fun (m, r) -> m, snd r) //returns second element of tuple and the move option
                            let bestMove = 
                                if isMax then Seq.maxBy snd secondElementMove //returns the max value with the move
                                else Seq.minBy snd secondElementMove //returns the min value with the move
                            Some (fst bestMove), snd bestMove //Some is an option, first with bestMove, second with bestMove

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
            let rec MiniMax alpha beta oldState perspective =
                NodeCounter.Increment()
                raise (System.NotImplementedException("Alpha Beta Pruning"))
            NodeCounter.Reset()
            MiniMax
