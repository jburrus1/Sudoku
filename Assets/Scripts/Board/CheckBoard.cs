using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBoard : MonoBehaviour
{
    [SerializeField]
    private BoardManager _boardManager;

    private UnityEngine.UI.Button _button;
    private UnityEngine.UI.Image _image;
    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<UnityEngine.UI.Image>();
    }

    public void CheckIfSolved()
    {
        var check = _boardManager.Check();

        _button.colors = new ColorBlock()
        {
            normalColor = check ? Color.green : Color.red,
            highlightedColor = _button.colors.highlightedColor,
            pressedColor = _button.colors.pressedColor,
            selectedColor = _button.colors.selectedColor,
            disabledColor = _button.colors.disabledColor,
            colorMultiplier = _button.colors.colorMultiplier,
            fadeDuration = _button.colors.fadeDuration
        };

        _image.color = check ? Color.green : Color.red;

        if (check)
        {
            _boardManager.AnimateVictory();
        }
    }
}
