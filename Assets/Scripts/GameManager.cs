using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using DataModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int _queueSize = 5;
    private Dictionary<E_Difficulty, Queue<DataModel.Board>> _boards;
    private Dictionary<E_Difficulty, object> _locks;
    private object _allBoardLock = new object();
    private Task[] _tasks;
    public static GameManager Instance;



    private UserData _userData;
    

    public E_Difficulty CurrentDifficulty;
    public Dictionary<E_Difficulty, Queue<DataModel.Board>> Boards => _boards;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        _boards = new Dictionary<E_Difficulty, Queue<DataModel.Board>>();
        _locks = new Dictionary<E_Difficulty, object>();
        
        
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            var bf = new BinaryFormatter();
            var file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            _userData = (UserData)bf.Deserialize(file);
        }
        foreach(var difficulty in Enum.GetValues(typeof(E_Difficulty)))
        {
            _boards.Add((E_Difficulty)difficulty, new Queue<DataModel.Board>());
            _locks.Add((E_Difficulty)difficulty, new object());
        }

        if (!_userData.Equals(default))
        {
            _boards = _userData.GetBoardQueues();
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

        lock (_allBoardLock)
        {
            UpdateUserData();
        }

    }

    private void UpdateUserData()
    {
        _userData = new UserData(_boards);
    }
    
    
    void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        var bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
        }
        else
        {
            file = File.Create(Application.persistentDataPath + "/save.dat");
        }
        
        bf.Serialize(file,_userData);
        file.Close();
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
