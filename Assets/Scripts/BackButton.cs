using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    private GameObject _popup;

    private void Start()
    {
        _popup = transform.Find("Popup").gameObject;
        _popup.SetActive(false);
    }

    public void GoBack()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
            case 3:
                SceneManager.LoadScene(0);
                break;
            case 2:
                _popup.SetActive(true);
                break;
            default:
                throw new SwitchExpressionException();
        }
    }

    public void YesGoBack()
    {
        SceneManager.LoadScene(1);
    }

    public void NoStayHere()
    {
        _popup.SetActive(false);
    }
}
