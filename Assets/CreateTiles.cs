using System;
using System.Collections;
using System.Collections.Generic;
using DataModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;
using Random = System.Random;

public class CreateTiles : MonoBehaviour
{
    [SerializeField]
    private GameObject _tilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        var board = new Board();
        var xOffset = 5f;
        for (var x = 0; x < 9; x++)
        {
            var yOffset = 5f;
            for (var y = 0; y < 9; y++)
            {
                var tile = Instantiate(_tilePrefab, transform, true);
                tile.transform.localScale = new Vector3(1, 1, 1);
                var rectTransform = tile.GetComponent<RectTransform>();

                rectTransform.anchoredPosition = new Vector2(xOffset,-yOffset);

                var num = tile.transform.Find("Num");
                var numField = num.GetComponent<UnityEngine.UI.Image>();
                numField.sprite = board.Grid[x, y] == 0 ? null : GetNum(board.Grid[x, y]);

                yOffset = GetNewOffset(yOffset,y);
            }
            xOffset = GetNewOffset(xOffset,x);
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

    float GetNewOffset(float currentOffset, int index)
    {
        var newOffset = currentOffset + 85;
        if ((index % 3) == 2)
        {
            return newOffset + 5;
        }

        return newOffset + 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
