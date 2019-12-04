using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerManager : MonoBehaviour
{
    //Matilde a fait ce script

    public static PlayerManager Instance = null;

    public List<Player> player;


    public List<PlayerEntity> playerEntity;


    private GameManager gameManagerScript;

    private List<AttackTest> _attackTest;


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

        player = new List<Player>();
        playerEntity = new List<PlayerEntity>();
        _attackTest = new List<AttackTest>();
        for (int i = 0; i < gameManagerScript.playerList.Count; i++)
        {
            player.Add(ReInput.players.GetPlayer("Player" + (i+1)));
        }

        for (int i = 0; i < player.Count; i++)
        {
            playerEntity.Add(gameManagerScript.playerList[i].GetComponent<PlayerEntity>());
        }



        for (int i = 0; i < player.Count; i++)
        {
            _attackTest.Add(playerEntity[i].GetComponent<AttackTest>());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Gère à quel joueur attribuer quel action
        float inputXPlayer1 = -player[0].GetAxis("HorizontalJoy1");
        float inputYPlayer1 = player[0].GetAxis("VerticalJoy1");
        Vector2 dirPlayer1 = new Vector2(inputXPlayer1, inputYPlayer1);
        if (dirPlayer1.magnitude < 0.3f)
        {
            dirPlayer1 = Vector2.zero;
        }
        
        playerEntity[0].SetInputX(dirPlayer1);

        if(player[0].GetButton("Push1") && playerEntity[0].GetUltiBool())
        {
            _attackTest[0].Push();
        }


        Debug.Log(player[2].GetAxis("HorizontalJoy3"));

        float inputXPlayer2 = -player[1].GetAxis("HorizontalJoy2");
        float inputYPlayer2 = player[1].GetAxis("VerticalJoy2");
        Vector2 dirPlayer2 = new Vector2(inputXPlayer2, inputYPlayer2);
        if (dirPlayer2.magnitude < 0.3f)
        {
            dirPlayer2 = Vector2.zero;
        }
        playerEntity[1].SetInputX(dirPlayer2);


        if (player[1].GetButton("Push2") && playerEntity[1].GetUltiBool())
        {
            _attackTest[1].Push();
        }

        if (player.Count == 3 || player.Count == 4)
        {
            float inputXPlayer3 = -player[2].GetAxis("HorizontalJoy3");
            float inputYPlayer3 = player[2].GetAxis("VerticalJoy3");
            Vector2 dirPlayer3 = new Vector2(inputXPlayer3, inputYPlayer3);
            if (dirPlayer3.magnitude < 0.3f)
            {
                dirPlayer3 = Vector2.zero;
            }
            playerEntity[2].SetInputX(dirPlayer3);

            if (player[2].GetButton("Push3") && playerEntity[2].GetUltiBool())
            {
                _attackTest[2].Push();
            }
        }

        

        if(player.Count == 4)
        {
            float inputXPlayer4 = -player[3].GetAxis("HorizontalJoy4");
            float inputYPlayer4 = player[3].GetAxis("VerticalJoy4");
            Vector2 dirPlayer4 = new Vector2(inputXPlayer4, inputYPlayer4);
            if (dirPlayer4.magnitude < 0.3f)
            {
                dirPlayer4 = Vector2.zero;
            }
            playerEntity[3].SetInputX(dirPlayer4);

            if (player[3].GetButton("Push4") && playerEntity[3].GetUltiBool())
            {
                _attackTest[3].Push();
            }
        }
    }


    public void Vibration(Player _player, int motorUsed, float motorVibrationStrength, float duration)
    {

        _player.SetVibration(motorUsed, motorVibrationStrength, duration);
    }


}
