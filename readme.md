## Sudoku Solver

I've been learning a little bit of F# a bit lately; this is a simple brute-force sudoku solver I wrote to help me learn.

It's not super efficient, and it's probably not the most idiomatic F# in the world. But I'm putting it here for reference and posterity.

### Usage

.\SudokuSolver.exe [puzzle file]

The puzzle file format is simple, one puzzle per line:

```
001700509573024106800501002700295018009400305652800007465080071000159004908007053
029650178705180600100900500257800309600219005900007046573400021800020453010395000
```

With the `0`s representing open spaces in the puzzle. The repo includes a couple of puzzle files I found at http://www.printable-sudoku-puzzles.com/wfiles/.
