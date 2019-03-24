namespace QUT

    module GameTheory =
        let MiniMaxGenerator (heuristic:'Game -> 'Player -> int) //heuriestic score for any game situation
                             (getTurn: 'Game -> 'Player) //player's turn
                             (gameOver:'Game->bool) //Whether game is over or not
                             (moveGenerator: 'Game->seq<'Move>) //enumerate all possible moves from a given name situation
                             (applyMove: 'Game -> 'Move -> 'Game) 
                             : 'Game -> 'Player -> Option<'Move> * int = //apply a move to a game situation to create a new game situation
            // Basic MiniMax algorithm without using alpha beta pruning
            let rec MiniMax (game: 'Game) (perspective: 'Player) : Option<'Move> * int =
                NodeCounter.Increment()

                let rec EstablishScore (game : 'Game) (move: 'Move) (isMax: bool) : Option<'Move> * int = 
                    if gameOver game then
                        let score = heuristic game perspective
                        (Some move), score
                    else 
                        let moves = moveGenerator game
                        if Seq.isEmpty moves then
                            None, 0
                        else
                            let nextGameStates = Seq.map (applyMove game) moves
                            let nextGameScores = Seq.map (fun g -> EstablishScore g move (not isMax)) nextGameStates
                            let bestMove = if isMax then Seq.maxBy snd nextGameScores else Seq.minBy snd nextGameScores
                            bestMove

                let moves = moveGenerator game
                if Seq.isEmpty moves then
                     if gameOver game then
                        let score = (heuristic game perspective)
                        None, score
                     else
                        None, 0
                else 
                    let possibilities = Seq.map (fun m -> (m, applyMove game m)) moves
                    let endGames = Seq.map (fun (m, g) -> EstablishScore g m true) possibilities
                    let bestMove = Seq.maxBy snd endGames
                    bestMove
               
                //raise (System.NotImplementedException("MiniMax")) //remove line

            NodeCounter.Reset()
            MiniMax

        let MiniMaxWithAlphaBetaPruningGenerator (heuristic:'Game -> 'Player -> int) (getTurn: 'Game -> 'Player) (gameOver:'Game->bool) (moveGenerator: 'Game->seq<'Move>) (applyMove: 'Game -> 'Move -> 'Game) : int -> int -> 'Game -> 'Player -> Option<'Move> * int =
            // Optimized MiniMax algorithm that uses alpha beta pruning to eliminate parts of the search tree that don't need to be explored            
            let rec MiniMax alpha beta oldState perspective =
                NodeCounter.Increment()
                raise (System.NotImplementedException("Alpha Beta Pruning"))
            NodeCounter.Reset()
            MiniMax
