using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatList : MonoBehaviour
{
    [SerializeField] private GameObject _statsBlockPrefab;
    // Start is called before the first frame update
    void Start()
    {
        var offset = 0;
        foreach (var stats in GameManager.Instance.UserData.StatsByDifficulty.Values)
        {
            var statBlockObj = Instantiate(_statsBlockPrefab,transform);
            var statBlock = statBlockObj.GetComponent<StatBlock>();
            var rectTransform = statBlockObj.GetComponent<RectTransform>();

            rectTransform.anchoredPosition = new Vector2(0, offset);
            
            statBlock.Initialize(stats);
            
            offset -= 500;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
