module Output

let PrintSquare (square:Square) =
        match square with
        | Fixed(value = v) -> sprintf "[%i]" v
        | Filled(value = v) -> sprintf "[%i]" v
        | Empty -> sprintf "[ ]"

let PrintSquares (puzzle:Puzzle) =
    List.map PrintSquare puzzle
    |> System.String.Concat 

let PrintPuzzle (puzzle:Puzzle) = 
    List.chunkBySize 9 puzzle
    |>  List.iter (fun s -> printfn "%s" (PrintSquares s))

