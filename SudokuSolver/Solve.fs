module Solve

let GetNumbers squares =    
    squares
    |> List.choose (fun s ->
        match s with 
        | Fixed v -> Some v
        | Filled v -> Some v
        | Empty -> None )   

let FindDuplicates squares = 
    GetNumbers squares  
    |> List.groupBy id  
    |> List.choose (fun (key, set) -> 
        if set.Length > 1
            then Some key
            else None)

let HasDuplicates squares = 
    (FindDuplicates squares).Length > 0

let FillSquareByIndex (puzzle:Puzzle) (index:int) (value:int) =
    
    let newSquare = 
        match puzzle.[index] with
        | Fixed v -> invalidArg "index" "Can't fill fixed squares"
        | _ -> Filled value

    (List.take index puzzle) @ (newSquare :: (List.skip (index + 1) puzzle))

let ToIndex row col = 
    row * 9 + col

let FromIndex index =
    let col = index % 9
    let row = index / 9
    row,col

let FillSquare puzzle row col value = 
    FillSquareByIndex puzzle (ToIndex row col) value
        
let GetRow row puzzle = 
    puzzle
    |> List.skip (row * 9) 
    |> List.take 9

let GetSquaresByIndices indices puzzle = 
    puzzle
    |> List.indexed 
    |> List.choose (fun (index, square) ->
        match index with
        | index when (List.exists (fun x -> x = index) indices) -> Some square
        | _ -> None
    )

let GetColumn col puzzle = 
    let indices = [col..9..81]
    GetSquaresByIndices indices puzzle
   
let GetSector sector (puzzle:Puzzle) = 
    let col = sector % 3
    let row : int = (sector / 3) * 3;
    let indices = List.map (fun i ->  i + (col * 3) + (row * 9)) [0;1;2;9;10;11;18;19;20]
    GetSquaresByIndices indices puzzle

let GetSectorFromIndex index = 
    let row : int = index / 27;
    let col : int = (index % 9) / 3;
    (3 * row) + col

let IsEmpty square = match square with | Empty -> true | _ -> false

let IsComplete puzzle = 
    not (List.exists IsEmpty puzzle)
   
let PuzzleHasErrors puzzle = 
    let indices = [0..8]
    let HasRowErrors = List.exists (fun index -> HasDuplicates (GetRow index puzzle)) indices
    let HasColumnErrors = List.exists (fun index -> HasDuplicates (GetColumn index puzzle)) indices
    let HasSectorErrors = List.exists (fun index -> HasDuplicates (GetSector index puzzle)) indices
    if HasRowErrors || HasColumnErrors || HasSectorErrors then
        true
    else
        false

let IsSolved puzzle = 
    not (PuzzleHasErrors puzzle) && IsComplete puzzle

let FindFirstEmptySquareIndex (puzzle:Puzzle) = 
    puzzle |> List.findIndex (fun square -> IsEmpty square)

let rec GenerateSolutions (puzzle:Puzzle) =

    let NextPuzzle puzzle value =
        FillSquareByIndex puzzle (FindFirstEmptySquareIndex puzzle) value

    let Candidates puzzle = 
        let index = FindFirstEmptySquareIndex puzzle
        let row,col = FromIndex index
        [1..9] 
        |> List.except (GetNumbers (GetColumn col puzzle)) // Don't bother checking numbers which are already in this column
        |> List.except (GetNumbers (GetRow row puzzle)) // Don't bother checking numbers which are already in this row
        |> List.except (GetNumbers (GetSector (GetSectorFromIndex index) puzzle)) // Don't bother checking numbers which are already in this sector

    let NewLayer puzzle = 
        match puzzle with
        | puzzle when IsComplete puzzle -> Seq.empty<Puzzle>
        | puzzle when PuzzleHasErrors puzzle -> Seq.empty<Puzzle>
        | _ ->
            Candidates puzzle
            |> Seq.collect (fun value -> GenerateSolutions (NextPuzzle puzzle value))

    seq {
        yield puzzle
        yield! NewLayer puzzle
    }

let Solve puzzle = 
    
    let predicate p = 
        not (IsSolved p)
    
    let sols = 
        GenerateSolutions puzzle

    sols |> Seq.skipWhile predicate |> Seq.truncate 1 |> Seq.last
