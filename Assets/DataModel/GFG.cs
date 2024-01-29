using System;

class GFG
{

    public static bool isSafe(int[, ] board,
                              int row, int col,
                              int num)
    {

        // Row has the unique (row-clash)
        for (int d = 0; d < board.GetLength(0); d++)
        {

            // Check if the number
            // we are trying to
            // place is already present in
            // that row, return false;
            if (board[row, d] == num)
            {
                return false;
            }
        }

        // Column has the unique numbers (column-clash)
        for (int r = 0; r < board.GetLength(0); r++)
        {

            // Check if the number
            // we are trying to
            // place is already present in
            // that column, return false;
            if (board[r, col] == num)
            {
                return false;
            }
        }

        // corresponding square has
        // unique number (box-clash)
        int sqrt = (int)Math.Sqrt(board.GetLength(0));
        int boxRowStart = row - row % sqrt;
        int boxColStart = col - col % sqrt;

        for (int r = boxRowStart;
             r < boxRowStart + sqrt; r++)
        {
            for (int d = boxColStart;
                 d < boxColStart + sqrt; d++)
            {
                if (board[r, d] == num)
                {
                    return false;
                }
            }
        }

        // if there is no clash, it's safe
        return true;
    }



    public static int CountSudoku(int[, ] board,
        int n)
    {

        int row = -1;
        int col = -1;
        bool isEmpty = true;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (board[i, j] == 0)
                {
                    row = i;
                    col = j;

                    // We still have some remaining
                    // missing values in Sudoku
                    isEmpty = false;
                    break;
                }
            }
            if (!isEmpty)
            {
                break;
            }
        }

        // no empty space left
        if (isEmpty)
        {
            return 1;
        }

        // else for each-row backtrack
        var allSolutions = 0;
        for (int num = 1; num <= n; num++)
        {
            if (isSafe(board, row, col, num))
            {
                board[row, col] = num;
                allSolutions += CountSudoku(board, n);
                board[row, col] = 0;
            }
        }
        return allSolutions;
    }
}