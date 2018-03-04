namespace SudokuSolver

module Main =

    open Output
    open Input
    open Solve

    let PrintSolution (puzzle, solution) =
        printfn "\nPuzzle:"
        PrintPuzzle puzzle
        printfn "\nSolution:"
        PrintPuzzle solution
    
    let WorkOnPuzzle puzzle = 
        let solution = Solve puzzle
        (puzzle, solution)

    [<EntryPoint>]
    let main argv = 

        let puzzles = LoadPuzzles argv.[0]

        let stopWatch = System.Diagnostics.Stopwatch.StartNew()

        let work = [ for puzzle in puzzles -> async { return WorkOnPuzzle puzzle } ]

        let solutions =
            Async.Parallel work
            |> Async.RunSynchronously
        
        stopWatch.Stop()

        solutions |> Array.iter PrintSolution

        printfn "Total time to solve puzzles: %f seconds" stopWatch.Elapsed.TotalSeconds

        let count = Seq.length puzzles

        let avg : float = stopWatch.Elapsed.TotalSeconds / (float)count

        printfn "Average time to solve each puzzle: %f seconds" avg

        System.Console.ReadLine() |> ignore
        0 // return an integer exit code
