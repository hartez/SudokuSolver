[<AutoOpen>]
module Types  

type Square =
    | Fixed of value : int
    | Filled of value : int
    | Empty

type Puzzle = List<Square>