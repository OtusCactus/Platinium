using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public static GameManager instance = null;

    private FaceClass _faceClassScript;
    private GetMenuInformation _menuInformationScript;
    private ScoreManager _scoreManagerScript;

    [Header("Player")]
    public List<GameObject> playerList;
    public GameObject[] playerPrefabs;
    private PlayerEntity[] playersEntityScripts;
    private List<GameObject> currentPlayersList = new List<GameObject>();

    [Header("Arena")]
    public int currentPlayersOnArena;
    public bool isTurning;
    public bool hasRoundBegun;
    public int currentFace;

    private GameObject currentLD;
    [Header("RoundStart")]
    public Text ReadyText;
    private float lerpTimer = 0;
    public float lerpTimerMax;

    [Header("SlowMotion")]
    [SerializeField]
    private float slowMotionScale = 0.1f;
    private bool _isSlowMotion;
    private bool _currentSlowMotion;
    private float _slowMotionTimer;
    [SerializeField]
    private float _slowMotionTimerMax = 1;

    private void Awake()
    {
        _scoreManagerScript = GetComponent<ScoreManager>();
        _faceClassScript = GetComponent<FaceClass>();


        if (GameObject.FindWithTag("MenuManager") != null)
        {

            _menuInformationScript = GameObject.FindWithTag("MenuManager").GetComponent<GetMenuInformation>();

            //permet de set les controles et d'instantier les personnages joueurs en fonction du nombre de joueurs
            if (_menuInformationScript != null && playerList.Count > 0)
            {
                print("ok");

                _scoreManagerScript.nbrPlayers = 0;
                for (int i = playerList.Count; i-- >0;)
                {
                    playerList[i].SetActive(false);
                }
                playerList.Clear();
            }
            for (int i = 0; i < _menuInformationScript.numbersOfPlayers; i++)
            {
                GameObject playerInstantiation = Instantiate(playerPrefabs[i]);
                playerList.Add(playerInstantiation);
                _scoreManagerScript.nbrPlayers ++;
            }
        }

        playersEntityScripts = new PlayerEntity[playerList.Count];

        currentPlayersOnArena = playerList.Count;

        for (int i = playerList.Count; i--> 0;)
        {
            currentPlayersList.Add(playerList[i]);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //set la position de départ des joueurs
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].transform.position = _faceClassScript.faceTab[0].playerStartingPosition[i].position;
            playerList[i].SetActive(true);
            playersEntityScripts[i] = playerList[i].GetComponent<PlayerEntity>();
        }

        _currentSlowMotion = _isSlowMotion;


    }

    // Update is called once per frame
    void Update()
    {
        if (_isSlowMotion != _currentSlowMotion)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                for (int j = 0; j < playersEntityScripts[i].GetAudioSourceTab().Length; j++)
                {
                    playersEntityScripts[i].GetAudioSourceTab()[j].pitch = Time.timeScale;
                }
            }
            _currentSlowMotion = _isSlowMotion;
        }

        if (_isSlowMotion )
        {
            _slowMotionTimer += Time.deltaTime;
            if(_slowMotionTimer >= _slowMotionTimerMax)
            {
                StopSlowMotion();
                _slowMotionTimer = 0;
            }
        }
      

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
            ReadyText.text = "Ready";
            for (int i = 0; i < playersEntityScripts.Length; i++)
            {
                playersEntityScripts[i].newRound();
            }

            lerpTimer += Time.deltaTime;
            float timerRatio = lerpTimer / lerpTimerMax;

            Debug.Log(timerRatio);

            PlayerReset(playerList);
            for (int i = 0; i < playerList.Count; i ++)
            {
                PlayerLerp(i, timerRatio);
            }

            if(timerRatio > 0.90f)
            {
                ReadyText.text = "Go";
            }
            if (timerRatio >1)
            {
                ReadyText.text = "";
                currentPlayersOnArena = playerList.Count;
                lerpTimer = 0;
                timerRatio = 0;
                hasRoundBegun = false;
            }

        }
        else
        {
            ReadyText.text = "";
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            SlowMotion();

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StopSlowMotion();

        }

    }

    //permet de reset et de replacer les joueurs à chaque changement de faces
    private void PlayerReset(List<GameObject> player)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            //player[i].SetActive(false);
            playerList[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //player[i].transform.position = _faceClassScript.faceTab[currentFace].playerStartingPosition[i].position;

        }
    }

    private void PlayerLerp(int playerNumber, float timerRatio)
    {
        playerList[playerNumber].transform.position = Vector3.Lerp(playerList[playerNumber].transform.position, _faceClassScript.faceTab[currentFace].playerStartingPosition[playerNumber].position, timerRatio);

        if(timerRatio >= 1)
        {
            print("reached");

            playersEntityScripts[playerNumber].enabled = true;
            BoxCollider2D[] playerColliders = playerList[playerNumber].GetComponents<BoxCollider2D>();
            foreach(BoxCollider2D colliders in playerColliders)
            { 
               colliders.enabled = true;
            }
        }
        else
        {
            playersEntityScripts[playerNumber].enabled = false;
        }
    }

    public void ThisPlayerHasLost(string player)
    {
        SlowMotion();
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

    public void ResetCurrentPlayers()
    {
        currentPlayersList.Clear();
        for (int i = playerList.Count; i-- > 0;)
        {
            currentPlayersList.Add(playerList[i]);
        }
    }
    #region SlowMo
    public void SlowMotion()
    {
        _isSlowMotion = true;
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void StopSlowMotion()
    {
        _isSlowMotion = false;
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }

    public bool GetSlowMotionBool()
    {
        return _isSlowMotion;
    }
    #endregion


}
