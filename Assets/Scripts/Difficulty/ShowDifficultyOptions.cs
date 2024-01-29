using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowDifficultyOptions : MonoBehaviour
{
    private float _buttonOffset = 200f;
    // Start is called before the first frame update
    void Start()
    {
        var difficulties = Enum.GetValues(typeof(GameManager.E_Difficulty));
        var startOffset = (difficulties.Length - 1) * _buttonOffset / 2;

        foreach (GameManager.E_Difficulty difficulty in difficulties)
        {
            var button = Instantiate(Resources.Load("Prefabs/Button")) as GameObject;
            button.transform.SetParent(transform);
            button.transform.localPosition = new Vector3(0, startOffset, 0);
            button.transform.localScale = Vector3.one;

            var buttonComponent = button.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => StartGame(difficulty));

            var text = button.transform.Find("Text");
            text.GetComponent<TextMeshProUGUI>().text = difficulty.ToString().ToLower();
            startOffset -= _buttonOffset;
        }

    }

    public void StartGame(GameManager.E_Difficulty difficulty)
    {
        GameManager.Instance.CurrentDifficulty = difficulty;
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
