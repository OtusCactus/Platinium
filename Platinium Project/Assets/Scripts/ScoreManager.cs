using System.Collections;
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
    
    private int _todaysWinner = 0;
    private int _todaysSecond = 0;
    private int _todaysThird = 0;
    private int _todaysLooser = 0;

    
    private int actualRound = 0;

    private GameManager _gameManagerScript;
    public GameObject gamePanel;

    [Header("Ecran Game Over")]
    public GameObject restartMenu;
    public Image[] playersClassement;
    public Sprite[] playersSprite;
    public GameObject podiumThird;
    public Button buttonMenu;

    private bool _mustSuddenDeath = false;
    


    // Start is called before the first frame update
    void Start()
    {
        _gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
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
                _todaysLooser = player - 1;
                break;
            case 3:
                print("score +1");
                _playerScore[player - 1] += 1;
                allScoresUI[player - 1].text = _playerScore[player - 1].ToString();
                _todaysThird = player -1;
                break;
            case 2:
                print("score +2");
                _playerScore[player - 1] += 2;
                allScoresUI[player - 1].text = _playerScore[player - 1].ToString();
                _todaysSecond = player -1;
                break;
            case 1:
                print("score +3");
                _playerScore[player - 1] += 3;
                allScoresUI[player - 1].text = _playerScore[player - 1].ToString();
                _todaysWinner = player - 1;
                for (int i =0; i < _playerScore.Length; i++)
                {
                    _CheckScore(i);
                }
                actualRound++;
                break;


        }

    }

    //check le score des joueurs, si il correspond au score à atteindre, finis la partie et fait apparaitre l'écran de fin
    void _CheckScore(int player)
    {
        //Si on a un gagnant, il faut affiché l'écran de fin, différent selon le nombre de joueur et qui est gagnant
        if (_playerScore[player] >= scoreToWin)
        {
            if (Classement().Length != 0)
            {
                playersClassement[0].sprite = playersSprite[Classement()[0]];
                playersClassement[1].sprite = playersSprite[Classement()[1]];
                if (nbrPlayers >= 3)
                {
                    playersClassement[2].sprite = playersSprite[Classement()[2]];
                    podiumThird.SetActive(true);
                    playersClassement[2].gameObject.SetActive(true);
                }
                if (nbrPlayers == 4)
                {
                    playersClassement[3].sprite = playersSprite[Classement()[3]];
                    playersClassement[3].gameObject.SetActive(true);
                }
                playersClassement[0].gameObject.SetActive(true);
                playersClassement[1].gameObject.SetActive(true);
                gamePanel.SetActive(false);
                buttonMenu.Select();
                restartMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                //bioup
            }
        }
        //gère l'apparition des médailles, différente selon nombre de joueur
        if (nbrPlayers == 4)
        {
            medals[Classement()[3]].gameObject.SetActive(false);
            medals[Classement()[2]].sprite = medalsSprites[2];
            medals[Classement()[2]].gameObject.SetActive(true);
            medals[Classement()[1]].sprite = medalsSprites[1];
            medals[Classement()[1]].gameObject.SetActive(true);
            medals[Classement()[0]].sprite = medalsSprites[0];
            medals[Classement()[0]].gameObject.SetActive(true);
        }
        else if (nbrPlayers == 3)
        {
            medals[Classement()[2]].sprite = medalsSprites[2];
            medals[Classement()[2]].gameObject.SetActive(true);
            medals[Classement()[1]].sprite = medalsSprites[1];
            medals[Classement()[1]].gameObject.SetActive(true);
            medals[Classement()[0]].sprite = medalsSprites[0];
            medals[Classement()[0]].gameObject.SetActive(true);
        }
        else if (nbrPlayers == 2)
        {
            medals[Classement()[3]].gameObject.SetActive(false);
            medals[Classement()[2]].gameObject.SetActive(false);
            medals[Classement()[1]].gameObject.SetActive(false);
            medals[Classement()[0]].sprite = medalsSprites[0];
            medals[Classement()[0]].gameObject.SetActive(true);
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
            if (_playerScore[x] > score)
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
            else if (_playerScore[x] > scoreS)
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
        }
        //gère les égalités, le gagnant est celui qui vient de remporter le round

        if (scoreS == scoreT && second != _todaysSecond)
        {
            third = second;
            second = _todaysSecond;
        }
        if (scoreT == scoreF && third != _todaysThird)
        {
            fourth = third;
            third = _todaysThird;
        }
        //if (scoreF == scoreS && first != _todaysWinner)
        //{
        //    second = first;
        //    first = _todaysWinner;
        //}
        if (scoreF == scoreS)
        {
            _mustSuddenDeath = true;
            return new int[0];
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
