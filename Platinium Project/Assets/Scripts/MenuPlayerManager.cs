using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;


public class MenuPlayerManager : MonoBehaviour
{
    //Matilde a fait ce script

    public static MenuPlayerManager Instance = null;

    public Player _player;

    public InMenuPlayer playerEntity;

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
            StartGame();
        }
        else if (playerEntity.currentFace == 1 && _player.GetButton("Push1"))
        {
            ExitGame();
        }
    }
    public void Vibration(Player _player, int motorUsed, float motorVibrationStrength, float duration)
    {

        _player.SetVibration(motorUsed, motorVibrationStrength, duration);
    }


    void StartGame()
    {
        SceneManager.LoadScene("MainScene 2");
    }

    void ExitGame()
    {
        Application.Quit();
    }

}
