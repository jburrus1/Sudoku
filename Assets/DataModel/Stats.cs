using System;
using System.Collections.Generic;

namespace DataModel
{
    [Serializable]
    public class Stats
    {
        private GameManager.E_Difficulty _difficulty;
        
        private int _numberOfWins;

        private float _bestTime;

        private Queue<float> _pastTimes;


        public GameManager.E_Difficulty _Difficulty => _difficulty;
        public int NumberOfWins => _numberOfWins;
        public float BestTime => _bestTime;
        public float AverageTime => CalculateAverageTime();

        public Stats(GameManager.E_Difficulty difficulty)
        {
            _difficulty = difficulty;
            _numberOfWins = 0;
            _bestTime = float.MaxValue;
            _pastTimes = new Queue<float>();

        }

        public float CalculateAverageTime()
        {
            var numTimes = _pastTimes.Count;
            var sum = 0f;
            foreach (var time in _pastTimes.ToArray())
            {
                sum += time;
            }

            return sum / numTimes;
        }

        public void AddNewTime(float newTime)
        {
            _numberOfWins++;
            if (_bestTime.Equals(float.MinValue) || (newTime < _bestTime))
            {
                _bestTime = newTime;
            }

            _pastTimes.Enqueue(newTime);

            if (_pastTimes.Count > GameManager.NUM_TIMES_TO_KEEP)
            {
                _pastTimes.Dequeue();
            }
        }
    }
}