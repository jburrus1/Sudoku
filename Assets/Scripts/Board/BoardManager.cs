using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;
using Random = System.Random;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _tilePrefab;

    [SerializeField] private float _rotateSpeed = 90f;

    private float _tileBorder = 5;
    private float _largeTileBorder = 10;

    private UnityEngine.UI.Image _thisImage;

    private bool _isInitialized;

    private GameObject _loading;

    private Transform _buttons;

    private List<Transform> _numberButtons;
    private Transform[,] _tiles;

    private Board _board;

    private int _selectedNumber;
    // Start is called before the first frame update
    void Start()
    {
        _tiles = new Transform[9, 9];
        _selectedNumber = 0;
        _numberButtons = new List<Transform>();
        _isInitialized = false;
        _thisImage = GetComponent<UnityEngine.UI.Image>();
        _thisImage.enabled = false;
        _loading = transform.Find("Loading").gameObject;
        _buttons = transform.Find("Buttons");
        // TODO: Error handling
    }

    private void Initialize()
    {
        if (GameManager.Instance.Boards[GameManager.Instance.CurrentDifficulty].Any())
        {
            var tileSize = (1000 - 6*_tileBorder - 4*_largeTileBorder)/9;
            _board = GameManager.Instance.Boards[GameManager.Instance.CurrentDifficulty].Dequeue();
            var xOffset = _largeTileBorder;
            for (var x = 0; x < 9; x++)
            {
                var yOffset = _largeTileBorder;
                for (var y = 0; y < 9; y++)
                {
                    var tile = Instantiate(_tilePrefab, transform, true);
                    tile.transform.localScale = new Vector3(1, 1, 1);
                    var rectTransform = tile.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(tileSize, tileSize);
                    var background = tile.GetComponent<UnityEngine.UI.Image>();
                    if (_board.Grid[x, y] != 0)
                    {
                        background.color = new Color(0.9f, 0.9f, 0.9f);
                    }

                    rectTransform.anchoredPosition = new Vector2(xOffset,-yOffset);

                    var num = tile.transform.Find("Num");
                    var numField = num.GetComponent<UnityEngine.UI.Image>();
                    numField.sprite = _board.Grid[x, y] == 0 ? null : GetNum(_board.Grid[x, y]-1);

                    var buttonComponent = tile.GetComponent<Button>();
                    if (_board.Grid[x, y] != 0)
                    {
                        buttonComponent.enabled = false;
                    }
                    else
                    {
                        var x1 = x;
                        var y1 = y;
                        buttonComponent.onClick.AddListener(() => EditTile(x1,y1));
                    }

                    _tiles[x, y] = tile.transform;

                    yOffset = GetNewOffset(yOffset,y, tileSize);
                }
                xOffset = GetNewOffset(xOffset,x, tileSize);
            }

            var offset = 1000f / 9;
            var startOffset = -4*offset;
            for(var i=0; i<9; i++)
            {
                var button = Instantiate(Resources.Load("Prefabs/NumberSelect")) as GameObject;
                button.transform.SetParent(_buttons);
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(startOffset + offset * i, 0);
                button.transform.localScale = new Vector3(1, 1, 1);

                var num = button.transform.Find("Number");
                var image = num.GetComponent<UnityEngine.UI.Image>();
                image.sprite = LoadSprite(i);

                var buttonElement = button.GetComponent<Button>();
                var i1 = i;
                buttonElement.onClick.AddListener(() => SelectNumber(i1));

                _numberButtons.Add(button.transform);
            }

            _thisImage.enabled = true;
            _isInitialized = true;
        }
    }

    private Sprite GetNum(int num)
    {
        var random = new Random();
        var index = random.Next(8) * 9;

        var sprite = LoadSprite(index + num);

        return sprite;
    }

    private Sprite LoadSprite(int index)
    {
        Sprite[] all = Resources.LoadAll<Sprite>("Numbers");

        foreach( var s in all)
        {
            if (s.name == $"Numbers_{index}")
            {
                return s;
            }
        }
        return null;
    }

    float GetNewOffset(float currentOffset, int index, float tileSize)
    {
        var newOffset = currentOffset + tileSize;
        if ((index % 3) == 2)
        {
            return newOffset + _largeTileBorder;
        }

        return newOffset + _tileBorder;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isInitialized)
        {
            Initialize();
            _loading.transform.Rotate(0,0,-_rotateSpeed * Time.deltaTime);
        }
        else
        {
            _loading.SetActive(false);
        }
    }

    public void SelectNumber(int index)
    {
        for(var i=0; i<9; i++)
        {
            _numberButtons[i].Find("Highlight").gameObject.GetComponent<UnityEngine.UI.Image>().enabled = i == index;
        }

        _selectedNumber = index + 1;
    }



    public void EditTile(int x,int y)
    {
        var tile = _tiles[x,y];
        var num = tile.transform.Find("Num");
        var selectedNumber = _selectedNumber;
        if (_board.Grid[x, y] == _selectedNumber)
        {
            selectedNumber = 0;
        }
        var numField = num.GetComponent<UnityEngine.UI.Image>();
        numField.sprite = selectedNumber == 0 ? null : GetNum(selectedNumber-1);

        _board.Grid[x,y] = selectedNumber;
    }

    public bool Check()
    {
        var xHashes = new List<HashSet<int>>();
        var boxHashes = new List<HashSet<int>>();
        foreach(var x in Enumerable.Range(0,9))
        {
            xHashes.Add(new HashSet<int>());
            boxHashes.Add(new HashSet<int>());
        }

        for(var x=0; x<9; x++)
        {
            var yHash = new HashSet<int>();
            for(var y=0; y<9; y++)
            {
                var val = _board.Grid[x, y];

                var xGroup = x / 3;
                var yGroup = y / 3;

                var boxIndex = xGroup * 3 + yGroup;

                if ((val == 0) || yHash.Contains(val) || xHashes[y].Contains(val) || boxHashes[boxIndex].Contains(val))
                {
                    return false;
                }

                xHashes[y].Add(val);
                yHash.Add(val);
                boxHashes[boxIndex].Add(val);
            }
        }

        return true;
    }
}
