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
    
    public GameObject restartMenu;

    private int actualRound = 0;

    private GameManager _gameManagerScript;



    // Start is called before the first frame update
    void Start()
    {
        _gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        UpdateUI();
        _playerWinCount = new int[nbrPlayers];
        for (int i = _playerWinCount.Length; i-- > 0;)
        {
            _playerWinCount[i] = 0;
        }
    }

    void Update()
    {
    }

    public void ChangeScore(bool win, int player)
    {
        if (!win)
        {
            allScores[player - 1].transform.GetChild(actualRound).GetComponent<Image>().color = Color.red;
        }
        else
        {
            allScores[player - 1].transform.GetChild(actualRound).GetComponent<Image>().color = Color.green;
            _playerWinCount[player - 1] += 1;
            _CheckScore(player);
            actualRound++;
        }

    }

    //check le score des joueurs, si il correspond au score à atteindre, finis la partie et fait apparaitre l'écran de fin
    void _CheckScore(int player)
    {
        if (_playerWinCount[player - 1] == scoreToWin)
        {
            restartMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void UpdateUI()
    {
        for(int i = 0; i < nbrPlayers; i++)
        {
            allScores[i].SetActive(true);
            allPlayersText[i].SetActive(true);
        }
    }

}
