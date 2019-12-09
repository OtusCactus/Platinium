using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PlayerManager : MonoBehaviour
{
    //Matilde a fait ce script

    public static PlayerManager Instance = null;

    public List<Player> player;


    public List<PlayerEntity> playerEntity;


    private GameManager gameManagerScript;
    private Pause pauseScript;

    private List<AttackTest> _attackTest;
    private bool[] mouvementPlayerBool;
    private float[] inputXPlayer;
    private float[] inputYPlayer;

    public Image[] playerMouvement;
    public Sprite defaultMouv;
    public Sprite inversedMouv;

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
        pauseScript = GetComponent<Pause>();


    }

    // Start is called before the first frame update
    void Start()
    {

        player = new List<Player>();
        playerEntity = new List<PlayerEntity>();
        _attackTest = new List<AttackTest>();

        mouvementPlayerBool = new bool[4];
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
        

        for (int i = 0; i < gameManagerScript.GetMenuInfoMouvementBool().Length;i++)
        {
            mouvementPlayerBool[i] = gameManagerScript.GetMenuInfoMouvementBool()[i];
            if (mouvementPlayerBool[i])
            {
                playerMouvement[i].sprite = defaultMouv;
            }
            else
            {
                playerMouvement[i].sprite = inversedMouv;
            }
        }

        inputXPlayer = new float[4];
        inputYPlayer = new float[4];

    }

    // Update is called once per frame
    void Update()
    {
        //Gère à quel joueur attribuer quel action
        if (mouvementPlayerBool[0] == false)
        {
            inputXPlayer[0] = -player[0].GetAxis("HorizontalJoy1");
            inputYPlayer[0] = player[0].GetAxis("VerticalJoy1");

        }
        else
        {
            inputXPlayer[0] = player[0].GetAxis("HorizontalJoy1");
            inputYPlayer[0] = -player[0].GetAxis("VerticalJoy1");
        }
        Vector2 dirPlayer1 = new Vector2(inputXPlayer[0], inputYPlayer[0]);
        if (dirPlayer1.magnitude < 0.3f)
        {
            dirPlayer1 = Vector2.zero;
        }
        
        playerEntity[0].SetInputX(dirPlayer1);

        if(player[0].GetButton("Push1") && playerEntity[0].GetUltiBool())
        {
            _attackTest[0].Push();
        }


        //Gère à quel joueur attribuer quel action
        if (mouvementPlayerBool[1] == false)
        {
            inputXPlayer[1] = -player[1].GetAxis("HorizontalJoy1");
            inputYPlayer[1] = player[1].GetAxis("VerticalJoy1");

        }
        else
        {
            inputXPlayer[1] = player[1].GetAxis("HorizontalJoy1");
            inputYPlayer[1] = -player[1].GetAxis("VerticalJoy1");
        }
        //inputXPlayer[1] = -player[1].GetAxis("HorizontalJoy2");
        //float inputYPlayer2 = player[1].GetAxis("VerticalJoy2");
        Vector2 dirPlayer2 = new Vector2(inputXPlayer[1], inputYPlayer[1]);
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
        { //Gère à quel joueur attribuer quel action
            if (mouvementPlayerBool[3] == false)
            {
                inputXPlayer[3] = -player[3].GetAxis("HorizontalJoy1");
                inputYPlayer[3] = player[3].GetAxis("VerticalJoy1");

            }
            else
            {
                inputXPlayer[3] = player[3].GetAxis("HorizontalJoy1");
                inputYPlayer[3] = -player[3].GetAxis("VerticalJoy1");
            }
            Vector2 dirPlayer3 = new Vector2(inputXPlayer[2], inputYPlayer[2]);
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
            //Gère à quel joueur attribuer quel action
            if (mouvementPlayerBool[3] == false)
            {
                inputXPlayer[3] = -player[3].GetAxis("HorizontalJoy1");
                inputYPlayer[3] = player[3].GetAxis("VerticalJoy1");

            }
            else
            {
                inputXPlayer[3] = player[3].GetAxis("HorizontalJoy1");
                inputYPlayer[3] = -player[3].GetAxis("VerticalJoy1");
            }
            //float inputXPlayer4 = -player[3].GetAxis("HorizontalJoy4");
            //float inputYPlayer4 = player[3].GetAxis("VerticalJoy4");
            Vector2 dirPlayer4 = new Vector2(inputXPlayer[3], inputYPlayer[3]);
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

        if(pauseScript.GetItsOptions() && player[0].GetAxis("HorizontalJoy1") > 0.2f)
        {
            playerMouvement[0].sprite = inversedMouv;
            mouvementPlayerBool[0] = true;
        }
        else if (pauseScript.GetItsOptions() && player[0].GetAxis("HorizontalJoy1") < -0.2f)
        {
            playerMouvement[0].sprite = defaultMouv;
            mouvementPlayerBool[0] = false;
        }



    }


    public void Vibration(Player _player, int motorUsed, float motorVibrationStrength, float duration)
    {

        _player.SetVibration(motorUsed, motorVibrationStrength, duration);
    }
    public void StopVibration(Player _player)
    {
        _player.StopVibration();
    }


}
