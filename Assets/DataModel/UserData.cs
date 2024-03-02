using System;
using System.Collections.Generic;

namespace DataModel
{
    [Serializable]
    public struct UserData
    {
        public Dictionary<GameManager.E_Difficulty, BoardData[]> _boardData;

        public UserData(Dictionary<GameManager.E_Difficulty, Queue<Board>> boardData)
        {
            _boardData = ConvertToBoardDataQueues(boardData);
        }

        public Dictionary<GameManager.E_Difficulty, Queue<Board>> GetBoardQueues()
        {
            var outData = new Dictionary<GameManager.E_Difficulty, Queue<Board>>();
            foreach (var key in _boardData.Keys)
            {
                var copyArr = _boardData[key];
                var outQueue = new Queue<Board>();
                for (var i = 0; i < copyArr.Length; i++)
                {
                    outQueue.Enqueue(new Board(copyArr[i]));
                }

                outData[key] = outQueue;
            }

            return outData;
        }
        
        

        private static Dictionary<GameManager.E_Difficulty, BoardData[]> ConvertToBoardDataQueues(
            Dictionary<GameManager.E_Difficulty, Queue<Board>> boardQueues)
        {
            var outData = new Dictionary<GameManager.E_Difficulty, BoardData[]>();
            foreach (var key in boardQueues.Keys)
            {
                var copyQueue = boardQueues[key].ToArray();
                var outArray = new BoardData[copyQueue.Length];
                for (var i = 0; i < copyQueue.Length; i++)
                {
                    outArray[i] = new BoardData(copyQueue[i]);
                }

                outData[key] = outArray;
            }

            return outData;
        }
    }
}