﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    public List<Player> player;
    public List<PlayerEntity> playerEntity;
    private GameManager gameManagerScript;

    public GameObject pausePanel;
    public Button resume;
    private bool _isInPause = false;

    private void Awake()
    {
        gameManagerScript = GetComponent<GameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = new List<Player>();
        playerEntity = new List<PlayerEntity>();
        for (int i = 0; i < gameManagerScript.playerList.Count; i++)
        {
            player.Add(ReInput.players.GetPlayer("Player" + (i + 1)));
        }

        for (int i = 0; i < player.Count; i++)
        {
            playerEntity.Add(gameManagerScript.playerList[i].GetComponent<PlayerEntity>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player[0].GetButtonDown("Pause1") && !_isInPause)
        {
            itsPause();
        }
        else if (player[0].GetButtonDown("Pause1") && _isInPause)
        {
            itsNotPause();
        }
    }

    private void itsPause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        resume.Select();
        _isInPause = true;
    }

    public void itsNotPause()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        _isInPause = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Time.timeScale = 1;
        Application.Quit();
    }

}