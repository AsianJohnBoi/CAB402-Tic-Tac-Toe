namespace QUT

    module GameTheory =

        let MiniMaxGenerator (heuristic:'Game -> 'Player -> int) //heuriestic score for any game situation
                             (getTurn: 'Game -> 'Player) //player's turn
                             (gameOver:'Game->bool) //Whether game is over or not
                             (moveGenerator: 'Game->seq<'Move>) //enumerate all possible moves from a given name situation
                             (applyMove: 'Game -> 'Move -> 'Game) : 'Game -> 'Player -> Option<'Move> * int = //apply a move to a game situation to create a new game situation
            // Basic MiniMax algorithm without using alpha beta pruning
            let rec MiniMax game perspective =
                NodeCounter.Increment()

                // if m is a Move
                // then None is an Option<Move>,  or Some m is an Option<Move>
                if gameOver then None, 0
                else 
                // maxBy : ('T -> 'U) -> seq<'T> -> 'T
                // maxBy : ('Move -> 'U) -> seq<'Move> -> 'Move
                    let score (move : 'Move) = (heuristic game perspective)
                    
                    let maxVal = Seq.maxBy score (moveGenerator game)

                raise (System.NotImplementedException("MiniMax")) //remove line

            NodeCounter.Reset()
            MiniMax

        let MiniMaxWithAlphaBetaPruningGenerator (heuristic:'Game -> 'Player -> int) (getTurn: 'Game -> 'Player) (gameOver:'Game->bool) (moveGenerator: 'Game->seq<'Move>) (applyMove: 'Game -> 'Move -> 'Game) : int -> int -> 'Game -> 'Player -> Option<'Move> * int =
            // Optimized MiniMax algorithm that uses alpha beta pruning to eliminate parts of the search tree that don't need to be explored            
            let rec MiniMax alpha beta oldState perspective =
                NodeCounter.Increment()
                raise (System.NotImplementedException("Alpha Beta Pruning"))
            NodeCounter.Reset()
            MiniMax
