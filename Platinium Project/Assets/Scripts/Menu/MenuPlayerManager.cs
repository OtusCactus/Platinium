﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.SceneManagement;


public class MenuPlayerManager : MonoBehaviour
{
    //Grégoire a fait ce script

    public static MenuPlayerManager Instance = null;

    public Player _player;

    private List<Player> otherPlayers;

    public InMenuPlayer playerEntity;

    public GameObject playerSelection;
    public string sceneName;

    private bool _isStartGameShowing;

    public GameObject selecPanel;
    public GameObject[] twoOtherPlayerNumber;
    public GameObject[] twoOtherPlayerName;
    public Image[] readyButton;
    public Sprite readySprite;
    public Sprite originalSprite;
    public Sprite outSprite;
    private bool _isCharSelecShowing = false;
    public Slider playerNumberSlider;

    private float timerPOne = 0;
    private float timerPTwo = 0;
    private float timerPThree = 0;
    private float timerPFour = 0;

    private bool _isPOneReady = false;
    private bool _isPTwoReady = false;
    private bool _isPThreeReady = false;
    private bool _isPFourReady = false;

    private bool[] _isEveryoneReady = new bool[4];
    private int _howManyReady = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSelection.SetActive(false);
        selecPanel.SetActive(false);
        _player = ReInput.players.GetPlayer("Player1");
    }

    // Update is called once per frame
    void Update()
    {

        //Gère à quel joueur attribué quel action
        float inputXPlayer1 = -_player.GetAxis("HorizontalJoy1");
        float inputYPlayer1 = _player.GetAxis("VerticalJoy1");
        Vector2 dirPlayer1 = new Vector2(inputXPlayer1, inputYPlayer1);

        playerEntity.SetInputX(dirPlayer1);

        if (playerEntity.currentFace == 0 && _player.GetButton("Push1"))
        {
            ShowPlayerSelection();
        }
        else if (playerEntity.currentFace == 1 && _player.GetButton("Push1"))
        {
            ExitGame();
        }

        if (_isStartGameShowing && _player.GetButtonDown("BackMenu"))
        {
            playerEntity.enabled = true;
            playerSelection.SetActive(false);
            _isStartGameShowing = false;

        }
        if (_isCharSelecShowing && _player.GetButtonDown("BackMenu"))
        {
            selecPanel.SetActive(false);
            ShowPlayerSelection();
            _isPOneReady = false;
            _isPTwoReady = false;
            _isPThreeReady = false;
            _isPFourReady = false;
            _isCharSelecShowing = false;

        }
        if (_isCharSelecShowing && _player.GetButton("Push1"))
        {
            timerPOne += Time.deltaTime;
        }
        else if (_isCharSelecShowing && _player.GetButtonUp("Push1"))
        {
            timerPOne = 0;
        }
        if(timerPOne >= 1)
        {
            _isPOneReady = true;
        }
        if (_isPOneReady)
        {
            readyButton[0].sprite = readySprite;
        }
        else
        {
            readyButton[0].sprite = originalSprite;
        }

        if (_isCharSelecShowing && otherPlayers[0].GetButton("Push2"))
        {
            timerPTwo += Time.deltaTime;
        }
        else if (_isCharSelecShowing && otherPlayers[0].GetButtonUp("Push2"))
        {
            timerPTwo = 0;
        }
        if (timerPTwo >= 1)
        {
            _isPTwoReady = true;
        }
        if (_isPTwoReady)
        {
            readyButton[1].sprite = readySprite;
        }
        else
        {
            readyButton[1].sprite = originalSprite;
        }

        if (playerNumberSlider.value > 2)
        {
            if (_isCharSelecShowing && otherPlayers[1].GetButton("Push3"))
            {
                timerPThree += Time.deltaTime;
            }
            else if (_isCharSelecShowing && otherPlayers[1].GetButtonUp("Push3"))
            {
                timerPThree = 0;
            }
            if (timerPThree >= 1)
            {
                _isPThreeReady = true;
            }
        }
        if (_isPThreeReady)
        {
            readyButton[2].sprite = readySprite;
        }
        else
        {
            readyButton[2].sprite = originalSprite;
        }

        if (playerNumberSlider.value > 3)
        {
            if (_isCharSelecShowing && otherPlayers[2].GetButton("Push4"))
            {
                timerPFour += Time.deltaTime;
            }
            else if (_isCharSelecShowing && otherPlayers[2].GetButtonUp("Push4"))
            {
                timerPFour = 0;
            }
            if (timerPFour >= 1)
            {
                _isPFourReady = true;
            }

        }

        if (_isPFourReady)
        {
            readyButton[3].sprite = readySprite;
        }
        else
        {
            readyButton[3].sprite = originalSprite;
        }

        if ( playerNumberSlider.value == 2)
        {
            readyButton[2].sprite = outSprite;
            readyButton[3].sprite = outSprite;
            twoOtherPlayerName[0].SetActive(false);
            twoOtherPlayerName[1].SetActive(false);
            twoOtherPlayerNumber[0].SetActive(false);
            twoOtherPlayerNumber[1].SetActive(false);
            if (_isPOneReady && _isPTwoReady)
            {
                StartGame();
            }
        }
        else if (playerNumberSlider.value == 3)
        {
            twoOtherPlayerName[0].SetActive(true);
            twoOtherPlayerName[1].SetActive(false);
            twoOtherPlayerNumber[0].SetActive(true);
            twoOtherPlayerNumber[1].SetActive(false);
            readyButton[3].sprite = outSprite;
            if (_isPOneReady && _isPTwoReady && _isPThreeReady)
            {
                StartGame();
            }
        }
        else
        {
            twoOtherPlayerName[0].SetActive(true);
            twoOtherPlayerName[1].SetActive(true);
            twoOtherPlayerNumber[0].SetActive(true);
            twoOtherPlayerNumber[1].SetActive(true);
            if (_isPOneReady && _isPTwoReady && _isPThreeReady && _isPFourReady)
            {
                StartGame();
            }
        }

    }
    public void Vibration(Player _player, int motorUsed, float motorVibrationStrength, float duration)
    {

        _player.SetVibration(motorUsed, motorVibrationStrength, duration);
    }


    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowSelectionChar()
    {
        otherPlayers = new List<Player>();
        for (int i = 0; i < (int)playerNumberSlider.value -1; i++)
        {
            otherPlayers.Add(ReInput.players.GetPlayer("Player" + (i + 2)));
        }
        print(playerNumberSlider.value);
        playerSelection.SetActive(false);
        selecPanel.SetActive(true);
        _isCharSelecShowing = true;
    }

    void ShowPlayerSelection()
    {
        playerSelection.SetActive(true);
        playerEntity.enabled = false;
        _isStartGameShowing = true;

    }

    void ExitGame()
    {
        Application.Quit();
    }

}
