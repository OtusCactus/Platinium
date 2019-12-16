﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int scoreToWin;
    public GameObject[] allPlayers;
    public int nbrPlayers;

    [Header("HUD")]
    public Text[] allScoresUI;
    public Text[] totalScores;
    public Image[] medals;
    public Sprite[] medalsSprites;
    private int[] _playerScore;

    private int[] _roundClassmentForEgality = new int[] { 0, 0, 0, 0 };
    private int _todaysWinner = 0;
    private int _todaysSecond = 0;
    private int _todaysThird = 0;
    private int _todaysLooser = 0;

    
    private int actualRound = 0;

    private GameManager _gameManagerScript;
    private PlayerManager _playerManagerScript;
    public GameObject gamePanel;

    [Header("Ecran Game Over")]
    public GameObject restartMenu;
    public Image[] playersClassement;
    public Sprite[] playersSprite;
    public GameObject podiumThird;
    public Button buttonMenu;

    private bool _mustSuddenDeath = false;
    private int[] _thisRoundClassement;
    


    // Start is called before the first frame update
    void Start()
    {
        _gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _playerManagerScript = GameObject.FindWithTag("GameController").GetComponent<PlayerManager>();
        nbrPlayers = _gameManagerScript.playerList.Count;
        _playerScore = new int[nbrPlayers];
        print(nbrPlayers);

        for (int i = _playerScore.Length; i-- > 0;)
        {
            _playerScore[i] = 0;
        }
        UpdateUI();
        gamePanel.SetActive(true);
    }

    void Update()
    {
        
    }

    public void ChangeScore(int playersOnScene, int player)
    {
        //attribut le nombre de point selon la position d'"éjection"
        switch (playersOnScene)
        {
            case 4:
                print("score +0");
                print(player - 1);
                allScoresUI[player - 1].text = _playerScore[player - 1].ToString();
                _roundClassmentForEgality[3] = player - 1;
                _todaysLooser = player - 1;
                break;
            case 3:
                print("score +1");
                _playerScore[player - 1] += 1;
                allScoresUI[player - 1].text = _playerScore[player - 1].ToString();
                _roundClassmentForEgality[2] = player - 1;
                _todaysThird = player -1;
                break;
            case 2:
                print("score +2");
                _playerScore[player - 1] += 2;
                allScoresUI[player - 1].text = _playerScore[player - 1].ToString();
                _roundClassmentForEgality[1] = player - 1;
                _todaysSecond = player -1;
                break;
            case 1:
                print("score +3");
                _playerScore[player - 1] += 3;
                allScoresUI[player - 1].text = _playerScore[player - 1].ToString();
                _roundClassmentForEgality[0] = player - 1;
                _todaysWinner = player - 1;
                _CheckScore();
                actualRound++;
                break;


        }

    }

    //check le score des joueurs, si il correspond au score à atteindre, finis la partie et fait apparaitre l'écran de fin
    void _CheckScore()
    {
        print("check score" + Classement().Length);
        _thisRoundClassement = Classement();//Si on a un gagnant, il faut affiché l'écran de fin, différent selon le nombre de joueur et qui est gagnant
        if (_playerScore[_thisRoundClassement[0]] >= scoreToWin && !_mustSuddenDeath)
        {
            if (gameObject.tag == "Player1")
            {
                _playerManagerScript.StopVibration(_playerManagerScript.player[0]);
            }
            else if (gameObject.tag == "Player2")
            {
                _playerManagerScript.StopVibration(_playerManagerScript.player[1]);
            }
            if (gameObject.tag == "Player3")
            {
                _playerManagerScript.StopVibration(_playerManagerScript.player[2]);
            }
            else if (gameObject.tag == "Player4")
            {
                _playerManagerScript.StopVibration(_playerManagerScript.player[3]);
            }

            playersClassement[0].sprite = playersSprite[_thisRoundClassement[0]];
            playersClassement[1].sprite = playersSprite[_thisRoundClassement[1]];
            if (nbrPlayers >= 3)
            {
                playersClassement[2].sprite = playersSprite[_thisRoundClassement[2]];
                podiumThird.SetActive(true);
                playersClassement[2].gameObject.SetActive(true);
            }
            if (nbrPlayers == 4)
            {
                playersClassement[3].sprite = playersSprite[_thisRoundClassement[3]];
                playersClassement[3].gameObject.SetActive(true);
            }
            playersClassement[0].gameObject.SetActive(true);
            playersClassement[1].gameObject.SetActive(true);
            gamePanel.SetActive(false);
            buttonMenu.Select();
            restartMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (_playerScore[_thisRoundClassement[0]] >= scoreToWin && _mustSuddenDeath)
        {
            _gameManagerScript.ReadyText.text = "Sudden Death";
            if(_playerScore[_thisRoundClassement[2]] != _playerScore[_thisRoundClassement[0]])
            {
                if (nbrPlayers >= 3)
                {
                    _gameManagerScript.playerList[_thisRoundClassement[2]].gameObject.SetActive(false);
                    _gameManagerScript.playerUISprite[_thisRoundClassement[2]].gameObject.SetActive(false);
                    medals[_thisRoundClassement[2]].gameObject.SetActive(false);
                    allScoresUI[_thisRoundClassement[2]].gameObject.SetActive(false);
                    totalScores[_thisRoundClassement[2]].gameObject.SetActive(false);
                    totalScores[_thisRoundClassement[2]].gameObject.SetActive(false);
                    print("joueur " + _thisRoundClassement[2] + " désactivé");
                }
                if (nbrPlayers == 4)
                {
                    _gameManagerScript.playerList[_thisRoundClassement[3]].gameObject.SetActive(false);
                    _gameManagerScript.playerUISprite[_thisRoundClassement[3]].gameObject.SetActive(false);
                    medals[_thisRoundClassement[3]].gameObject.SetActive(false);
                    allScoresUI[_thisRoundClassement[3]].gameObject.SetActive(false);
                    totalScores[_thisRoundClassement[3]].gameObject.SetActive(false);
                    print("joueur " + _thisRoundClassement[3] + " désactivé");
                }
                _gameManagerScript.SetPlayerMax(2);
            }
            else if (_playerScore[_thisRoundClassement[3]] != _playerScore[_thisRoundClassement[0]])
            {
                if (nbrPlayers == 4)
                {
                    _gameManagerScript.playerList[_thisRoundClassement[3]].gameObject.SetActive(false);
                    _gameManagerScript.playerUISprite[_thisRoundClassement[3]].gameObject.SetActive(false);
                    medals[_thisRoundClassement[3]].gameObject.SetActive(false);
                    allScoresUI[_thisRoundClassement[3]].gameObject.SetActive(false);
                    totalScores[_thisRoundClassement[3]].gameObject.SetActive(false);
                    print("joueur " + _thisRoundClassement[3] + " désactivé");
                    _gameManagerScript.SetPlayerMax(3);

                }
            }
            else
            {
                _gameManagerScript.SetPlayerMax(4);
            }

        }
        //gère l'apparition des médailles, différente selon nombre de joueur
        if (nbrPlayers == 4)
        {
            medals[_thisRoundClassement[3]].gameObject.SetActive(false);
            medals[_thisRoundClassement[2]].sprite = medalsSprites[2];
            medals[_thisRoundClassement[2]].gameObject.SetActive(true);
            medals[_thisRoundClassement[1]].sprite = medalsSprites[1];
            medals[_thisRoundClassement[1]].gameObject.SetActive(true);
            medals[_thisRoundClassement[0]].sprite = medalsSprites[0];
            medals[_thisRoundClassement[0]].gameObject.SetActive(true);
        }
        else if (nbrPlayers == 3)
        {
            medals[_thisRoundClassement[2]].sprite = medalsSprites[2];
            medals[_thisRoundClassement[2]].gameObject.SetActive(true);
            medals[_thisRoundClassement[1]].sprite = medalsSprites[1];
            medals[_thisRoundClassement[1]].gameObject.SetActive(true);
            medals[_thisRoundClassement[0]].sprite = medalsSprites[0];
            medals[_thisRoundClassement[0]].gameObject.SetActive(true);
        }
        else if (nbrPlayers == 2)
        {
            medals[_thisRoundClassement[3]].gameObject.SetActive(false);
            medals[_thisRoundClassement[2]].gameObject.SetActive(false);
            medals[_thisRoundClassement[1]].gameObject.SetActive(false);
            medals[_thisRoundClassement[0]].sprite = medalsSprites[0];
            medals[_thisRoundClassement[0]].gameObject.SetActive(true);
        }
    }

    public void UpdateUI()
    {
        for(int i = 0; i < nbrPlayers; i++)
        {
            allScoresUI[i].text = _playerScore[i].ToString();
            allScoresUI[i].gameObject.SetActive(true);
            allPlayers[i].gameObject.SetActive(true);
            totalScores[i].text = "/" + scoreToWin.ToString();
            totalScores[i].gameObject.SetActive(true);
        }
    }

    private int[] Classement()
    {
        int first = 0;
        int second = 0;
        int third = 0;
        int fourth = 0;
        int score = 0;
        int scoreS = 0;
        int scoreT = 0;
        int scoreF = 0;

        //permet de définir l'ordre des joueurs
        for (int x = 0; x < _playerScore.Length; x++)
        {
            if (_playerScore[x] >= score)
            {
                if (nbrPlayers == 4)
                {
                    scoreF = scoreT;
                    fourth = third;
                }
                if (nbrPlayers >= 3)
                {
                    scoreT = scoreS;
                    third = second;
                }
                scoreS = score;
                second = first;
                score = _playerScore[x];
                first = x;
            }
            else if (_playerScore[x] >= scoreS)
            {
                if (nbrPlayers == 4)
                {
                    scoreF = scoreT;
                    fourth = third;
                }
                if (nbrPlayers >= 3)
                {
                    scoreT = scoreS;
                    third = second;
                }
                scoreS = _playerScore[x];
                second = x;
            }
            else if (_playerScore[x] >= scoreT)
            {
                if (nbrPlayers == 4)
                {
                    scoreF = scoreT;
                    fourth = third;
                }
                scoreT = _playerScore[x];
                third = x;
            }
            else
            {
                scoreF = _playerScore[x];
                fourth = x;
            }
        }
        //gère les égalités, le gagnant est celui qui vient de remporter le round

        if (scoreT == scoreF)
        {
            int tempLow = 0;
            int tempHigh = 0;
            for (int bleuh = 0; bleuh < _roundClassmentForEgality.Length; bleuh++)
            {
                if (_roundClassmentForEgality[bleuh] == fourth)
                {
                    tempLow = bleuh;
                }
                else if (_roundClassmentForEgality[bleuh] == third)
                {
                    tempHigh = bleuh;
                }
            }
            if (tempLow < tempHigh)
            {
                fourth = _roundClassmentForEgality[tempLow];
                third = _roundClassmentForEgality[tempHigh];
            }
            else
            {
                fourth = _roundClassmentForEgality[tempHigh];
                third = _roundClassmentForEgality[tempLow];
            }
        }
        if (scoreS == scoreT)
        {
            int tempLow = 0;
            int tempHigh = 0;
            for (int bleuh = 0; bleuh < _roundClassmentForEgality.Length; bleuh++)
            {
                if (_roundClassmentForEgality[bleuh] == third)
                {
                    tempLow = bleuh;
                }
                else if (_roundClassmentForEgality[bleuh] == third)
                {
                    tempHigh = second;
                }
            }
            if (tempLow < tempHigh)
            {
                third = _roundClassmentForEgality[tempLow];
                second = _roundClassmentForEgality[tempHigh];
            }
            else
            {
                third = _roundClassmentForEgality[tempHigh];
                second = _roundClassmentForEgality[tempLow];
            }
        }
        if (scoreF == score && score < scoreToWin)
        {
            int tempLow = 0;
            int tempHigh = 0;
            for (int bleuh = 0; bleuh < _roundClassmentForEgality.Length; bleuh++)
            {
                if (_roundClassmentForEgality[bleuh] == third)
                {
                    tempLow = bleuh;
                }
                else if (_roundClassmentForEgality[bleuh] == third)
                {
                    tempHigh = second;
                }
            }
            if (tempLow < tempHigh)
            {
                second = _roundClassmentForEgality[tempLow];
                first = _roundClassmentForEgality[tempHigh];
            }
            else
            {
                second = _roundClassmentForEgality[tempHigh];
                first = _roundClassmentForEgality[tempLow];
            }
        }
        else if (score == scoreS && score >= scoreToWin)
        {
            second = first;
            first = _todaysWinner;
            _mustSuddenDeath = true;
        }
        int[] results;
        if (nbrPlayers == 2)
        {
            results = new int[] { first, second };
        }
        else if (nbrPlayers == 3)
        {
            results = new int[] { first, second, third };
        }
        else
        {
            results = new int[] { first, second, third, fourth };
        }
        return results;
    }

}
