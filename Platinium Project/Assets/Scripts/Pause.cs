﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{

    public List<Player> player;
    public List<PlayerEntity> playerEntity;
    private GameManager gameManagerScript;
    private ScoreManager scoreManagerScript;

    public GameObject pausePanel;
    public Button resume;
    private bool _isInPause = false;

    public GameObject options;
    private bool _isInOptions = false;

    private float _timer = 0;
    private bool _canQuit = false;

    private void Awake()
    {
        gameManagerScript = GetComponent<GameManager>();
        scoreManagerScript = GetComponent<ScoreManager>();
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
        if (!scoreManagerScript.GetHasGameEnded())
        {
            if (_isInPause)
            {
                _timer += Time.deltaTime;
            }
            print(_isInPause);
            if ((player[0].GetButtonDown("Pause1") || player[1].GetButtonDown("Pause2") || player[2].GetButtonDown("Pause3") || player[3].GetButtonDown("Pause4")) && !_isInPause)
            {
                print("bonjhoru");
                itsPause();
                if(_timer >= 0.5f)
                {
                    _canQuit = true;
                }
            }
            else if ((player[0].GetButtonDown("Pause1") || player[1].GetButtonDown("Pause2") || player[2].GetButtonDown("Pause3") || player[3].GetButtonDown("Pause4")) && _isInPause && _canQuit)
            {
                itsNotPause();
            }
            if (_isInOptions && (player[0].GetButtonDown("BackMenu") || player[1].GetButtonDown("BackMenu2") || player[2].GetButtonDown("BackMenu3") || player[3].GetButtonDown("BackMenu4")))
            {
                itsPause();
            }
            else if (_isInOptions && (player[0].GetButtonDown("Pause1") || player[1].GetButtonDown("Pause2") || player[2].GetButtonDown("Pause3") || player[3].GetButtonDown("Pause4")))
            {
                itsNotPause();
            }
            else if (_isInPause && (player[0].GetButtonDown("Pause1") || player[1].GetButtonDown("Pause2") || player[2].GetButtonDown("Pause3") || player[3].GetButtonDown("Pause4")))
            {
                itsNotPause();
            }
        }
    }

    private void itsPause()
    {
        _timer = 0;
        pausePanel.SetActive(true);
        options.SetActive(false);
        Time.timeScale = 0;
        resume.Select();
        _isInPause = true;
        _isInOptions = false;
    }

    public void Options()
    {
        _timer = 0;
        options.SetActive(true);
        pausePanel.SetActive(false);
        _isInPause = false;
        _isInOptions = true;
    }

    public void itsNotPause()
    {
        _timer = 0;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        EventSystem.current.SetSelectedGameObject(null);
        _isInPause = false;
        _isInOptions = false;
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



    public bool GetItsOptions()
    {
        return _isInOptions;
    }


}
