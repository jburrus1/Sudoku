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

        public Board()
        {
            _grid = GenerateGrid();

            // _grid = new int[_size, _size];
            // for (var x = 0; x < _size; x++)
            // {
            //     for (var y = 0; y < _size; y++)
            //     {
            //         _grid[x, y] = 1;
            //     }
            // }
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

        private int[,] GenerateGrid()
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
                    if (!GFG.solveSudoku(newGrid, _size))
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
            
            foreach (var cell in allCells)
            {
                var save = grid[cell.x, cell.y];
                grid[cell.x, cell.y] = 0;
                if (GFG.CountSudoku(grid, _size) > 1)
                {
                    grid[cell.x, cell.y] = save;
                }
            }

            return grid;
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
}