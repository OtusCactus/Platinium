using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int scoreToWin;
    public int nbrPlayers;
    public GameObject[] scoreP1;
    public GameObject[] scoreP2;
    public GameObject restartMenu;

    private int[] _playerScore;
    private int actualRound = 0;


    // Start is called before the first frame update
    void Start()
    {
        _playerScore = new int[nbrPlayers];
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            AddScore(1);
        }
    }

    public void AddScore(int player)
    {
        player--;  //for the index
        _playerScore[player]++;
        _UpdateUI(player);
        _CheckScore(player);
        actualRound++;
    }

    void _CheckScore(int player)
    {
        if (_playerScore[player] == scoreToWin)
        {
            Debug.Log("player" + player+1 + "win");
            restartMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    void _UpdateUI(int player)
    {
        if(player == 1)
        {
            scoreP1[actualRound].GetComponent<Image>().color = Color.green;
            scoreP2[actualRound].GetComponent<Image>().color = Color.red;
        }
        else // if P2 win
        {
            scoreP1[actualRound].GetComponent<Image>().color = Color.red;
            scoreP2[actualRound].GetComponent<Image>().color = Color.green;
        }
    }

}
