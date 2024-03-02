using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace DataModel
{
    public class Board
    {
        private static int _size = 9;
        private int[,] _grid;

        public int[,] Grid => _grid;

        public Board(GameManager.E_Difficulty difficulty)
        {
            _grid = GenerateGrid(difficulty);
        }
        
        public Board(BoardData boardData)
        {
            _grid = boardData.Grid;
        }

        private static void Shuffle<T>(IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        private int[,] GenerateGrid(GameManager.E_Difficulty difficulty)
        {
            var grid = new int[_size, _size];

            var validCells = new HashSet<(int x, int y)>();
            for (var x = 0; x < _size; x++)
            {
                for (var y = 0; y < _size; y++)
                {
                    validCells.Add((x, y));
                }
            }
            var random  = new Random();
            var count = 0;
            while (validCells.Any() || (count == 10_000))
            {
                var cell = validCells.ElementAt(random.Next(validCells.Count));
                if (AddRandom(grid, cell.x, cell.y))
                {
                    var newGrid = grid.Clone() as int[,];
                    _solveCounter = 0;
                    if (!SolveSudoku(newGrid, _size))
                    {
                        grid[cell.x, cell.y] = 0;
                    }
                    else
                    {
                        validCells.Remove(cell);
                    }
                }

                count++;
            }


            var allCells = new List<(int x,int y)>();
            for (var x = 0; x < _size; x++)
            {
                for (var y = 0; y < _size; y++)
                {
                    allCells.Add((x, y));
                }
            }

            Shuffle(allCells);

            var removed = new List<(int x,int y, int val)>();

            foreach (var cell in allCells)
            {
                var save = grid[cell.x, cell.y];
                grid[cell.x, cell.y] = 0;
                if (GFG.CountSudoku(grid, _size) > 1)
                {
                    grid[cell.x, cell.y] = save;
                }
                else
                {
                    removed.Add((cell.x, cell.y,save));
                }
            }

            //Difficulty
            Shuffle(removed);

            var addBackCount = 0;
            switch (difficulty)
            {
                case GameManager.E_Difficulty.Easy:
                    addBackCount = removed.Count-1;
                    break;
                case GameManager.E_Difficulty.Medium:
                    addBackCount = removed.Count / 4;
                    break;
                case GameManager.E_Difficulty.Hard:
                    addBackCount = 0;
                    break;
            }

            for (var i = 0; i < addBackCount; i++)
            {
                grid[removed[i].x, removed[i].y] = removed[i].val;
            }

            return grid;
        }


        private int _solveCounter = 0;
        public bool SolveSudoku(int[, ] board,
            int n)
        {

            if(_solveCounter >10_000)
            {
                return false;
            }

            _solveCounter++;
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
                return true;
            }

            // else for each-row backtrack
            for (int num = 1; num <= n; num++)
            {
                if (GFG.isSafe(board, row, col, num))
                {
                    board[row, col] = num;
                    if (SolveSudoku(board, n))
                    {

                        // Print(board, n);
                        return true;
                    }
                    else
                    {

                        // Replace it
                        board[row, col] = 0;
                    }
                }
            }
            return false;
        }

        private bool AddRandom(int[,] grid, int x, int y)
        {
            var validNums = new List<int>();
            for (var i = 1; i <= _size; i++)
            {
                if (GFG.isSafe(grid, x, y, i))
                {
                    validNums.Add(i);
                }
            }

            if (validNums.Any())
            {
                var random  = new Random();
                grid[x, y] = validNums.ElementAt(random.Next(validNums.Count()));
                return true;
            }

            return false;
        }

    }

    [Serializable]
    public struct BoardData
    {
        private readonly int[,] _grid;

        public readonly int[,] Grid => _grid;

        public BoardData(Board board)
        {
            _grid = board.Grid;
        }
    }
}