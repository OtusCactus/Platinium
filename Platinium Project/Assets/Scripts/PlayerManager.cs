using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerManager : MonoBehaviour
{
    //public MouvementPlayer[] allPlayers;

    public static PlayerManager Instance = null;

    private Player _player1;
    private Player _player2;
    private Player _player3;
    private Player _player4;

    public PlayerEntity playerEntity1;
    public PlayerEntity playerEntity2;
    //public PlayerEntity playerEntity3;
    //public PlayerEntity playerEntity4;

    public AttackTest attackTest1;
    public AttackTest attackTest2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  //va vouloir ne pas detruire juste le script pas va détruire le gameobject si on met juste this
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        /*string[] controllers = Input.GetJoystickNames();
        int j = 0;
        for (int i = 0; i < controllers.Length; i++)
        {
            if (controllers[i] != "")
            {
                allPlayers[j].controllerNumber = i + 1;
                print("je viens d'assigner la manette" + (i + 1) + " au joueur " + j);
                j++;
            }
        }
        if (j < allPlayers.Length)
        {
            Debug.Log("Il manque " + (allPlayers.Length - j) + " manettes");
        }*/

        _player1 = ReInput.players.GetPlayer("Player1");
        _player2 = ReInput.players.GetPlayer("Player2");
        _player3 = ReInput.players.GetPlayer("Player3");
        _player4 = ReInput.players.GetPlayer("Player4");
    }

    // Update is called once per frame
    void Update()
    {
        /*float accelerationXPlayer1 = _player1.GetAxis("HorizontalJoy1");
        float accelerationYPlayer1 = _player1.GetAxis("VerticalJoy1");*/

        float inputXPlayer1 = _player1.GetAxis("HorizontalJoy1");
        float inputYPlayer1 = -_player1.GetAxis("VerticalJoy1");
        Vector2 dirPlayer1 = new Vector2(inputXPlayer1, inputYPlayer1);

        /*playerEntity1.GetAccelerationX(accelerationXPlayer1);
        playerEntity1.GetAccelerationY(accelerationYPlayer1);*/
        playerEntity1.SetInputX(dirPlayer1);

        if(_player1.GetButton("Push1") && attackTest1.isShockWavePossible)
        {
            attackTest1.Push();
        }

        /*float accelerationXPlayer2 = _player2.GetAxis("HorizontalJoy2");
        float accelerationYPlayer2 = _player2.GetAxis("VerticalJoy2");*/

        float inputXPlayer2 = _player2.GetAxis("HorizontalJoy2");
        float inputYPlayer2 = -_player2.GetAxis("VerticalJoy2");
        Vector2 dirPlayer2 = new Vector2(inputXPlayer2, inputYPlayer2);

        /*playerEntity2.GetAccelerationX(accelerationXPlayer2);
        playerEntity2.GetAccelerationY(accelerationYPlayer2);*/
        playerEntity2.SetInputX(dirPlayer2);

        if (_player2.GetButton("Push2") && attackTest2.isShockWavePossible)
        {
            attackTest2.Push();
        }
    }
}
