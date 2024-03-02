using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DataModel
{
    [Serializable]
    public class UserData: IDeserializationCallback
    {
        private Dictionary<GameManager.E_Difficulty, BoardData[]> _boardData;


        private Dictionary<GameManager.E_Difficulty, Stats> _statsByDifficulty;
        private int _adCounter;
        
        public bool IsInitialized => (_boardData is null);

        public Dictionary<GameManager.E_Difficulty, Stats> StatsByDifficulty => _statsByDifficulty;

        public UserData()
        {
            _boardData = new Dictionary<GameManager.E_Difficulty, BoardData[]>();
            _statsByDifficulty = new Dictionary<GameManager.E_Difficulty, Stats>();
        
            foreach(var difficulty in Enum.GetValues(typeof(GameManager.E_Difficulty)))
            {
                _statsByDifficulty.Add((GameManager.E_Difficulty)difficulty, new Stats((GameManager.E_Difficulty)difficulty));
            }
        }

        public void UpdateBoardData(Dictionary<GameManager.E_Difficulty, Queue<Board>> boardData)
        {
            _boardData = ConvertToBoardDataQueues(boardData);
        }

        public bool IncrementAdCounterAndServe()
        {
            _adCounter++;
            if (_adCounter >= GameManager.GAMES_BETWEEN_ADS)
            {
                _adCounter = 0;
                return true;
            }

            return false;
        }

        public void UpdateStats(GameManager.E_Difficulty difficulty, float time)
        {
            _statsByDifficulty[difficulty].AddNewTime(time);
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

        public void OnDeserialization(object sender)
        {
            if (_statsByDifficulty is null)
            {
                _statsByDifficulty = new Dictionary<GameManager.E_Difficulty, Stats>();
        
                foreach(var difficulty in Enum.GetValues(typeof(GameManager.E_Difficulty)))
                {
                    _statsByDifficulty.Add((GameManager.E_Difficulty)difficulty, new Stats((GameManager.E_Difficulty)difficulty));
                }
            }

            if ((_boardData is null) || (_boardData.Count == 0))
            {
                _boardData = new Dictionary<GameManager.E_Difficulty, BoardData[]>();
        
                foreach(var difficulty in Enum.GetValues(typeof(GameManager.E_Difficulty)))
                {
                    _boardData.Add((GameManager.E_Difficulty)difficulty, new BoardData[]{});
                }
            }
        }
    }
}