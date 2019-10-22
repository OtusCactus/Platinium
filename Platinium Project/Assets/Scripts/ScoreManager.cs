using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int scoreToWin;
    public int nbrPlayers;

    private int[] _playerScore;


    // Start is called before the first frame update
    void Start()
    {
        _playerScore = new int[nbrPlayers];
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
            AddScore(1);
    }

    public void AddScore(int player)
    {
        _playerScore[player]++;
        _CheckScore(player);
    }

    void _CheckScore(int player)
    {
        if (_playerScore[player] == scoreToWin)
        {
            Debug.Log("player" + player+1 + "win");
        }
    }

}
