using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerManager : MonoBehaviour
{
    //Matilde a fait ce script

    public static PlayerManager Instance = null;

    private Player _player1;
    private Player _player2;
    private Player _player3;
    private Player _player4;

    public PlayerEntity playerEntity1;
    public PlayerEntity playerEntity2;

    private AttackTest _attackTest1;
    private AttackTest _attackTest2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        _attackTest1 = playerEntity1.GetComponent<AttackTest>();
        _attackTest2 = playerEntity2.GetComponent<AttackTest>();
    }

    // Start is called before the first frame update
    void Start()
    {

        _player1 = ReInput.players.GetPlayer("Player1");
        _player2 = ReInput.players.GetPlayer("Player2");
        _player3 = ReInput.players.GetPlayer("Player3");
        _player4 = ReInput.players.GetPlayer("Player4");
    }

    // Update is called once per frame
    void Update()
    {
        //Gère à quel joueur attribué quel action
        float inputXPlayer1 = _player1.GetAxis("HorizontalJoy1");
        float inputYPlayer1 = -_player1.GetAxis("VerticalJoy1");
        Vector2 dirPlayer1 = new Vector2(inputXPlayer1, inputYPlayer1);
        
        playerEntity1.SetInputX(dirPlayer1);

        if(_player1.GetButton("Push1") && _attackTest1.isShockWavePossible)
        {
            _attackTest1.Push();
        }
        

        float inputXPlayer2 = _player2.GetAxis("HorizontalJoy2");
        float inputYPlayer2 = -_player2.GetAxis("VerticalJoy2");
        Vector2 dirPlayer2 = new Vector2(inputXPlayer2, inputYPlayer2);
        
        playerEntity2.SetInputX(dirPlayer2);

        if (_player2.GetButton("Push2") && _attackTest2.isShockWavePossible)
        {
            _attackTest2.Push();
        }
    }
}
