using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int _queueSize = 5;
    private Dictionary<E_Difficulty, Queue<DataModel.Board>> _boards;
    private Dictionary<E_Difficulty, object> _locks;
    private Task[] _tasks;
    public static GameManager Instance;

    public E_Difficulty CurrentDifficulty;
    public Dictionary<E_Difficulty, Queue<DataModel.Board>> Boards => _boards;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        _boards = new Dictionary<E_Difficulty, Queue<DataModel.Board>>();
        _locks = new Dictionary<E_Difficulty, object>();
        foreach(var difficulty in Enum.GetValues(typeof(E_Difficulty)))
        {
            _boards.Add((E_Difficulty)difficulty, new Queue<DataModel.Board>());
            _locks.Add((E_Difficulty)difficulty, new object());
        }

        _tasks = new Task[_boards.Keys.Count];
    }

    // Update is called once per frame
    void Update()
    {

        if (_tasks.Any(t => !(t is null) && !t.IsCompleted))
        {
            return;
        }
        var keys = _boards.Keys.ToList();
        for (var i = 0; i < _tasks.Length; i++)
        {
            var key = keys[i];
            _tasks[i] = Task.Run(() => LoadBoard(key));
        }
    }

    private void LoadBoard(E_Difficulty difficulty)
    {
        lock (_locks[difficulty])
        {
            if(_boards[difficulty].Count < _queueSize)
            {
                var board = new DataModel.Board(difficulty);
                _boards[difficulty].Enqueue(board);
            }
        }

    }

    public void GoToDifficultySelect(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public enum E_Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}
