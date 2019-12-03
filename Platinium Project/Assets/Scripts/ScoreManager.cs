using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int scoreToWin;
    public GameObject[] allScores;
    public GameObject[] allPlayersText;
    private int[] _playerWinCount;
    public int nbrPlayers;

    public Text[] allScoresUI;
    private int[] _playerScore;

    public GameObject restartMenu;

    private int actualRound = 0;

    private GameManager _gameManagerScript;
    


    // Start is called before the first frame update
    void Start()
    {
        _gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        //_playerWinCount = new int[nbrPlayers];
        //for (int i = _playerWinCount.Length; i-- > 0;)
        //{
        //    _playerWinCount[i] = 0;
        //}
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
                allScoresUI[player - 1].text = "P" + player.ToString() + " : " + _playerScore[player - 1].ToString();
                break;
            case 3:
                print("score +1");
                _playerScore[player - 1] += 1;
                allScoresUI[player - 1].text = "P" + player.ToString() + " : " + _playerScore[player - 1].ToString();
                break;
            case 2:
                print("score +2");
                _playerScore[player - 1] += 2;
                allScoresUI[player - 1].text = "P" + player.ToString() + " : " + _playerScore[player - 1].ToString();
                break;
            case 1:
                print("score +3");
                _playerScore[player - 1] += 3;
                allScoresUI[player - 1].text = "P" + player.ToString() + " : " + _playerScore[player - 1].ToString();
                for (int i =0; i < _playerScore.Length; i++)
                {
                    _CheckScore(i);
                }
                actualRound++;
                break;


        }

        //if (!win)
        //{
        //    allScores[player - 1].transform.GetChild(actualRound).GetComponent<Image>().color = Color.red;
        //}
        //else
        //{
        //    allScores[player - 1].transform.GetChild(actualRound).GetComponent<Image>().color = Color.green;
        //    _playerWinCount[player - 1] += 1;
        //    _CheckScore(player);
        //    actualRound++;
        //}

    }

    //check le score des joueurs, si il correspond au score à atteindre, finis la partie et fait apparaitre l'écran de fin
    void _CheckScore(int player)
    {
        if (_playerScore[player] == scoreToWin)
        {
            restartMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void UpdateUI()
    {
        for(int i = 0; i < nbrPlayers; i++)
        {
            //allScores[i].SetActive(true);
            //allPlayersText[i].SetActive(true);
            allScoresUI[i].text = "P" + (i +1).ToString() + " : " + _playerScore[i].ToString();
            allScoresUI[i].gameObject.SetActive(true);
        }
    }

}
