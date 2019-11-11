﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int scoreToWin;
    public int nbrPlayers;
    public GameObject[] scoreP1;
    public GameObject[] scoreP2;
    public GameObject[] scoreP3;
    public GameObject[] scoreP4;
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
        //if (Input.GetKeyDown("space"))
        //{
        //    AddScore(1);
        //}
    }

    public void AddScore(int player)
    {
        player--;  //for the index
        _playerScore[player]++;
        _UpdateUI(player);
        _CheckScore(player);
        actualRound++;
    }

    //check le score des joueurs, si il correspond au score à atteindre, finis la partie et fait apparaitre l'écran de fin
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
            scoreP3[actualRound].GetComponent<Image>().color = Color.red;
            scoreP4[actualRound].GetComponent<Image>().color = Color.red;
        }
        else if(player == 2) // if P2 win
        {
            scoreP1[actualRound].GetComponent<Image>().color = Color.red;
            scoreP2[actualRound].GetComponent<Image>().color = Color.green;
            scoreP3[actualRound].GetComponent<Image>().color = Color.red;
            scoreP4[actualRound].GetComponent<Image>().color = Color.red;
        }
        else if (player == 3) // if P3 win
        {
            scoreP1[actualRound].GetComponent<Image>().color = Color.red;
            scoreP2[actualRound].GetComponent<Image>().color = Color.red;
            scoreP3[actualRound].GetComponent<Image>().color = Color.green;
            scoreP4[actualRound].GetComponent<Image>().color = Color.red;
        }
        else if (player == 4) // if P4 win
        {
            scoreP1[actualRound].GetComponent<Image>().color = Color.red;
            scoreP2[actualRound].GetComponent<Image>().color = Color.red;
            scoreP3[actualRound].GetComponent<Image>().color = Color.red;
            scoreP4[actualRound].GetComponent<Image>().color = Color.green;
        }
    }

}
