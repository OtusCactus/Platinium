using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int scoreToWin;
    public Text[] totalScores;
    public GameObject[] allPlayers;
    private int[] _playerWinCount;
    public int nbrPlayers;

    public Text[] allScoresUI;
    private int[] _playerScore;
    public Image[] playersClassement;
    public Sprite[] playersSprite;

    public GameObject restartMenu;

    private int actualRound = 0;

    private GameManager _gameManagerScript;
    


    // Start is called before the first frame update
    void Start()
    {
        _gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _playerScore = new int[nbrPlayers];

        for (int i = _playerScore.Length; i-- > 0;)
        {
            _playerScore[i] = 0;
        }
        UpdateUI();
    }

    void Update()
    {

    }

    public void ChangeScore(int playersOnScene, int player)
    {
        switch (playersOnScene)
        {
            case 4:
                print("score +0");
                print(player - 1);
                allScoresUI[player - 1].text = _playerScore[player - 1].ToString();
                break;
            case 3:
                print("score +1");
                _playerScore[player - 1] += 1;
                allScoresUI[player - 1].text = _playerScore[player - 1].ToString();
                break;
            case 2:
                print("score +2");
                _playerScore[player - 1] += 2;
                allScoresUI[player - 1].text = _playerScore[player - 1].ToString();
                break;
            case 1:
                print("score +3");
                _playerScore[player - 1] += 3;
                allScoresUI[player - 1].text = _playerScore[player - 1].ToString();
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
        if (_playerScore[player] >= scoreToWin)
        {
            playersClassement[0].sprite = playersSprite[Classement()[0]];
            playersClassement[1].sprite = playersSprite[Classement()[1]];
            if (nbrPlayers >= 3)
            {
                playersClassement[2].sprite = playersSprite[Classement()[2]];
                playersClassement[2].gameObject.SetActive(true);
            }
            if(nbrPlayers == 4)
            {
                playersClassement[3].sprite = playersSprite[Classement()[3]];
                playersClassement[3].gameObject.SetActive(true);
            }
            playersClassement[0].gameObject.SetActive(true);
            playersClassement[1].gameObject.SetActive(true);
            restartMenu.SetActive(true);
            Time.timeScale = 0;
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
        int scoreTw = 0;
        int scoreTh = 0;
        int scoreF = 0;
        for (int x = 0; x < _playerScore.Length; x++)
        {
            if (_playerScore[x] > score)
            {
                if (nbrPlayers == 4)
                {
                    scoreF = scoreTh;
                    fourth = third;
                }
                if (nbrPlayers >= 3)
                {
                    scoreTh = scoreTw;
                    third = second;
                }
                scoreTw = score;
                second = first;
                score = _playerScore[x];
                first = x;
            }
            else if (_playerScore[x] > scoreTw)
            {
                if (nbrPlayers == 4)
                {
                    scoreF = scoreTh;
                    fourth = third;
                }
                if (nbrPlayers >= 3)
                {
                    scoreTh = scoreTw;
                    third = second;
                }
                scoreTw = _playerScore[x];
                second = x;
            }
            else if (_playerScore[x] >= scoreTh)
            {
                if (nbrPlayers == 4)
                {
                    scoreF = scoreTh;
                    fourth = third;
                }
                scoreTh = _playerScore[x];
                third = x;
            }
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
