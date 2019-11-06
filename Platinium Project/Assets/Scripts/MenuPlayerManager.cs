using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MenuPlayerManager : MonoBehaviour
{
    //Matilde a fait ce script

    public static MenuPlayerManager Instance = null;

    public Player _player;

    public MenuPlayerEntity menuPlayerEntity;

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

        menuPlayerEntity.SetInputX(dirPlayer1);
    }

    public void Vibration(Player _player, int motorUsed, float motorVibrationStrength, float duration)
    {

        _player.SetVibration(motorUsed, motorVibrationStrength, duration);
    }
}
