using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Grégoire s'est occupé de ce script

    //tableaux contenant les prochaines faces selon la face où l'on est et selon le mur détruit.
    [Header("NextFaceArrays")]
    public int[] _wallNorthEastTab;
    public int[] _wallNorthWestTab;
    public int[] _wallSouthEastTab;
    public int[] _wallSouthTab;
    public int[] _wallSouthWestTab;
    


    private FaceClass _faceClassScript;
    private MenuManager _menuManagerScript;
    private ScoreManager _scoreManagerScript;

    public GameObject[] player;
    public GameObject[] playerPrefabs;
    private PlayerEntity[] playersEntityScripts;
    private List<GameObject> currentPlayersList = new List<GameObject>(); 

    public int currentPlayersOnArena;

    public bool isTurning;
    public bool hasRoundBegun;
    
    public int currentFace;

    private GameObject currentLD;

    private float lerpTimer = 0;
    public float lerpTimerMax;

    private void Awake()
    {
        if(GameObject.FindWithTag("MenuManager") != null)
        {
            _menuManagerScript = GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>();

            //permet de set les controles et d'instantier les personnages joueurs en fonction du nombre de joueurs
            if (_menuManagerScript != null && player.Length > 0)
            {
                _scoreManagerScript.nbrPlayers = 0;

                for (int j = 0; j < player.Length; j++)
                {
                    player[j].SetActive(false);
                    player[j] = null;
                    _scoreManagerScript.nbrPlayers += 1;
                }
            }
            for (int i = 0; i < _menuManagerScript.numbersOfPlayers; i++)
            {
                GameObject playerInstantiation = Instantiate(playerPrefabs[i]);
                player[i] = playerInstantiation;
            }
        }
        _faceClassScript = GetComponent<FaceClass>();
        _scoreManagerScript = GetComponent<ScoreManager>();

        playersEntityScripts = new PlayerEntity[player.Length];

        currentPlayersOnArena = player.Length;

        for (int i = player.Length; i--> 0;)
        {
            currentPlayersList.Add(player[i]);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //set la position de départ des joueurs
        for (int i = 0; i < player.Length; i++)
        {
            player[i].transform.position = _faceClassScript.faceTab[0].playerStartingPosition[i].position;
            playersEntityScripts[i] = player[i].GetComponent<PlayerEntity>();
        }




    }

    // Update is called once per frame
    void Update()
    {
        //check si on doit changer de face de l'arène
        if (isTurning)
        {
            if (currentLD != null)
            {
                Destroy(currentLD);
            }
            if (_faceClassScript.faceTab[currentFace].levelDesign != null)
            {
                currentLD = Instantiate(_faceClassScript.faceTab[currentFace].levelDesign);
            }
        }
        else if(hasRoundBegun)
        {
            lerpTimer += Time.deltaTime;
            float timerRatio = lerpTimer / lerpTimerMax;


            PlayerReset(player);
            PlayerLerp(0, timerRatio);
            PlayerLerp(1, timerRatio);
            PlayerLerp(2, timerRatio);
            PlayerLerp(3, timerRatio);
            if(timerRatio >1)
            {
                currentPlayersOnArena = player.Length;
                lerpTimer = 0;
                timerRatio = 0;
                hasRoundBegun = false;
            }

        }
    }

    //permet de reset et de replacer les joueurs à chaque changement de faces
    private void PlayerReset(GameObject[] player)
    {
        for (int i = 0; i < player.Length; i++)
        {
            //player[i].SetActive(false);
            player[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //player[i].transform.position = _faceClassScript.faceTab[currentFace].playerStartingPosition[i].position;

        }
    }

    private void PlayerLerp(int playerNumber, float timerRatio)
    {
        player[playerNumber].transform.position = Vector3.Lerp(player[playerNumber].transform.position, _faceClassScript.faceTab[currentFace].playerStartingPosition[playerNumber].position, timerRatio);

        if(timerRatio >= 1)
        {
            print("reached");

            playersEntityScripts[playerNumber].enabled = true;
            CircleCollider2D[] playerColliders = player[playerNumber].GetComponents<CircleCollider2D>();
            foreach(CircleCollider2D colliders in playerColliders)
            { 
               colliders.enabled = true;
            }
        }
    }

    public void ThisPlayerHasLost(string player)
    {
        for (int i = currentPlayersList.Count; i--> 0;)
        {
            if (currentPlayersList[i].gameObject.tag == player)
            {
                currentPlayersList.Remove(currentPlayersList[i]);
            }
        }
    }

    public GameObject GetFirstCurrentPlayersItem()
    {
        return currentPlayersList[0];
    }
}
