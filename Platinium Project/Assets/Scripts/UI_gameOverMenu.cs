﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_gameOverMenu : MonoBehaviour
{
    /*public Button restart;
    public Button quit;

    private void Update()
    {
        restart.onClick.Invoke();
        quit.onClick.Invoke();
    }*/

    public void RestartMatch()
    {
        SceneManager.LoadScene("Proto 1");
    }

    public void Quit()
    {
        Application.Quit();
    }

}