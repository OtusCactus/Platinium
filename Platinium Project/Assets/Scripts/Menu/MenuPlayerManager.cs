using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;


public class MenuPlayerManager : MonoBehaviour
{
    //Grégoire a fait ce script

    public static MenuPlayerManager Instance = null;

    public Player _player;

    public InMenuPlayer playerEntity;

    public GameObject playerSelection;
    public string sceneName;

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
    }
    public void Vibration(Player _player, int motorUsed, float motorVibrationStrength, float duration)
    {

        _player.SetVibration(motorUsed, motorVibrationStrength, duration);
    }


    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    void ShowPlayerSelection()
    {
        playerSelection.SetActive(true);
    }

    void ExitGame()
    {
        Application.Quit();
    }

}
