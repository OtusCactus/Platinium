using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerManager : MonoBehaviour
{
    //Matilde a fait ce script

    public static PlayerManager Instance = null;

    public Player player1;
    public Player player2;
    public Player player3;
    public Player player4;

    public PlayerEntity playerEntity1;
    public PlayerEntity playerEntity2;
    public PlayerEntity playerEntity3;
    public PlayerEntity playerEntity4;

    private GameManager gameManagerScript;

    private AttackTest _attackTest1;
    private AttackTest _attackTest2;
    private AttackTest _attackTest3;
    private AttackTest _attackTest4;

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
        gameManagerScript = GetComponent<GameManager>();



    }

    // Start is called before the first frame update
    void Start()
    {
        playerEntity1 = gameManagerScript.player[0].GetComponent<PlayerEntity>();
        playerEntity2 = gameManagerScript.player[1].GetComponent<PlayerEntity>();
        if(gameManagerScript.player[2] != null)
        playerEntity3 = gameManagerScript.player[2].GetComponent<PlayerEntity>();
        if (gameManagerScript.player[3] != null)
        playerEntity4 = gameManagerScript.player[3].GetComponent<PlayerEntity>();


        _attackTest1 = playerEntity1.GetComponent<AttackTest>();
        _attackTest2 = playerEntity2.GetComponent<AttackTest>();
        _attackTest3 = playerEntity3.GetComponent<AttackTest>();
        _attackTest4 = playerEntity4.GetComponent<AttackTest>();

        player1 = ReInput.players.GetPlayer("Player1");
        player2 = ReInput.players.GetPlayer("Player2");
        player3 = ReInput.players.GetPlayer("Player3");
        player4 = ReInput.players.GetPlayer("Player4");
    }

    // Update is called once per frame
    void Update()
    {
        //Gère à quel joueur attribué quel action
        float inputXPlayer1 = -player1.GetAxis("HorizontalJoy1");
        float inputYPlayer1 = player1.GetAxis("VerticalJoy1");
        Vector2 dirPlayer1 = new Vector2(inputXPlayer1, inputYPlayer1);
        
        playerEntity1.SetInputX(dirPlayer1);

        if(player1.GetButton("Push1") && _attackTest1.isShockWavePossible)
        {
            _attackTest1.Push();
        }


        

        float inputXPlayer2 = -player2.GetAxis("HorizontalJoy2");
        float inputYPlayer2 = player2.GetAxis("VerticalJoy2");
        Vector2 dirPlayer2 = new Vector2(inputXPlayer2, inputYPlayer2);
        
        playerEntity2.SetInputX(dirPlayer2);

        if (player2.GetButton("Push2") && _attackTest2.isShockWavePossible)
        {
            _attackTest2.Push();
        }

        float inputXPlayer3 = -player3.GetAxis("HorizontalJoy3");
        float inputYPlayer3 = player3.GetAxis("VerticalJoy3");
        Vector2 dirPlayer3 = new Vector2(inputXPlayer3, inputYPlayer3);

        playerEntity3.SetInputX(dirPlayer3);

        if (player3.GetButton("Push3") && _attackTest3.isShockWavePossible)
        {
            _attackTest3.Push();
        }

        float inputXPlayer4 = -player4.GetAxis("HorizontalJoy4");
        float inputYPlayer4 = player4.GetAxis("VerticalJoy4");
        Vector2 dirPlayer4 = new Vector2(inputXPlayer4, inputYPlayer4);

        playerEntity4.SetInputX(dirPlayer4);

        if (player4.GetButton("Push4") && _attackTest4.isShockWavePossible)
        {
            _attackTest4.Push();
        }
    }


    public void Vibration(Player _player, int motorUsed, float motorVibrationStrength, float duration)
    {

        _player.SetVibration(motorUsed, motorVibrationStrength, duration);
    }


}
