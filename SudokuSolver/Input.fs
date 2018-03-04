module Input

open System.IO

let ReadLines filePath = System.IO.File.ReadLines(filePath);

let ParseSquare (square:char) =
    match square with
    | '0' -> Empty
    | n -> Fixed(System.Int32.Parse(n.ToString()))

let ParsePuzzle (puzzle:string) = 
    List.ofSeq(puzzle 
        |> Seq.filter (fun s -> System.Char.IsDigit(s))
        |> Seq.map (fun s -> ParseSquare s))

let LoadPuzzles (filePath:string) = 
    ReadLines filePath 
    |> Seq.map (fun str -> ParsePuzzle str)
