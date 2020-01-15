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
    private ArenaRotation _arenaRotationScript;

    [Header("Player")]
    public List<GameObject> playerList;
    public GameObject[] playerPrefabs;
    private PlayerEntity[] playersEntityScripts;
    private AttackTest[] attackTestScripts;
    private List<GameObject> currentPlayersList = new List<GameObject>();
    private bool[] menuInfoMouvementBool;

    public GameObject[] playerUISprite;

    [Header("Arena")]
    public GameObject arena;
    public int currentPlayersOnArena;
    private int playerMax; 
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
    private NewSoundManager _newSoundMangerScript;
    private AudioSource[] _managerAudios;
    [SerializeField]
    private float _slowMotionTimerMax = 1;

    public GameObject[] wallHitObj;
    public GameObject[] CracksObj;

    private List<GameObject> suddenDeathPlayerToDisappear;

    private Animator _previousFaceAnimator;
    private bool _isRisingHappened;
    //debug
    public bool debug;

    private void Awake()
    {
        _scoreManagerScript = GetComponent<ScoreManager>();
        _faceClassScript = GetComponent<FaceClass>();
        _arenaRotationScript = arena.GetComponent<ArenaRotation>();
        menuInfoMouvementBool = new bool[4];


        if (GameObject.FindWithTag("MenuManager") != null)
        {

            _menuInformationScript = GameObject.FindWithTag("MenuManager").GetComponent<GetMenuInformation>();

            //permet de set les controles et d'instantier les personnages joueurs en fonction du nombre de joueurs
            if (_menuInformationScript != null && playerList.Count > 0)
            {
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

            for(int i = 0; i < _menuInformationScript.getPlayerMouvementMode().Length; i++)
            {
                menuInfoMouvementBool[i] = _menuInformationScript.getPlayerMouvementMode()[i];
            }
        }

        playersEntityScripts = new PlayerEntity[playerList.Count];
        attackTestScripts = new AttackTest[playerList.Count];
        playerMax = playerList.Count;

        currentPlayersOnArena = playerMax;

        for (int i = playerList.Count; i--> 0;)
        {
            currentPlayersList.Add(playerList[i]);
        }

        if (playerList.Count == 2)
        {
            wallHitObj[0].SetActive(false);
            wallHitObj[1].SetActive(false);
            playerUISprite[2].SetActive(false);
            playerUISprite[3].SetActive(false);
            CracksObj[2].SetActive(false);
            CracksObj[3].SetActive(false);
        }
        else if ( playerList.Count == 3)
        {
            CracksObj[3].SetActive(false);
            wallHitObj[1].SetActive(false);
            playerUISprite[3].SetActive(false);
        }


    }

    // Start is called before the first frame update
    void Start()
    {

        suddenDeathPlayerToDisappear = new List<GameObject>();

        _newSoundMangerScript = NewSoundManager.instance;
        _managerAudios = new AudioSource[_newSoundMangerScript.AudioLength()];
        _managerAudios = _newSoundMangerScript.GetMyAudios();
        currentFace = _arenaRotationScript._currentFace;

        //set la position de départ des joueurs
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].transform.position = _faceClassScript.faceTab[currentFace].playerStartingPosition[i].position;
            playerList[i].SetActive(true);
            playersEntityScripts[i] = playerList[i].GetComponent<PlayerEntity>();
            attackTestScripts[i] = playerList[i].GetComponent<AttackTest>();
        }

        _currentSlowMotion = _isSlowMotion;


        if (_faceClassScript.faceTab[currentFace].levelDesign != null)
        {
            currentLD = Instantiate(_faceClassScript.faceTab[currentFace].levelDesign);


        }
        _previousFaceAnimator = _faceClassScript.faceTab[currentFace].arenaWall.GetComponent<Animator>();
        _previousFaceAnimator.SetBool("isRising", true);
        hasRoundBegun = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isSlowMotion != _currentSlowMotion)
        {
            for (int i = 0; i < _managerAudios.Length; i++)
            {
                _managerAudios[i].pitch = Time.timeScale;
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
            
            _previousFaceAnimator.SetBool("isFalling", true);

            if (currentLD != null)
            {
                Destroy(currentLD);
            }
            for(int i =0; i < CracksObj.Length; i++)
            {
                if(CracksObj[i].activeSelf)
                {
                    attackTestScripts[i].ResetUlt();
                    CracksObj[i].SetActive(false);
                }
            }

            if(_scoreManagerScript.GetSuddenDeath())
            {
                for (int i = 0; i < suddenDeathPlayerToDisappear.Count; i++)
                {
                    suddenDeathPlayerToDisappear[i].SetActive(false);
                }
            }

        }
        else if(hasRoundBegun)
        {
            if(_previousFaceAnimator != null)
            {
                _previousFaceAnimator.SetBool("isFalling", false);
                _previousFaceAnimator = null;
            }


            _previousFaceAnimator = _faceClassScript.faceTab[currentFace].arenaWall.GetComponent<Animator>();
            if(!_isRisingHappened)
            {
                _previousFaceAnimator.SetBool("isRising", true);
                _isRisingHappened = true;
            }
            if (currentLD != null)
            {
                Destroy(currentLD);
            }
            if (_faceClassScript.faceTab[currentFace].levelDesign != null)
            {
                currentLD = Instantiate(_faceClassScript.faceTab[currentFace].levelDesign);


            }

            ReadyText.text = "Ready ?";
            for (int i = 0; i < playersEntityScripts.Length; i++)
            {
                playersEntityScripts[i].newRound();
            }

            lerpTimer += Time.deltaTime;
            float timerRatio = lerpTimer / lerpTimerMax;



            PlayerReset(playerList);
            for (int i = 0; i < playerList.Count; i ++)
            {
                PlayerLerp(i, timerRatio);
            }

            if(timerRatio > 0.90f)
            {
                ReadyText.text = "Go !";
                for (int i = 0; i < playerList.Count; i++)
                {
                    ReEnablingColliders(i);
                }
                for (int i = 0; i < _faceClassScript.faceTab[currentFace].arenaWall.transform.childCount; i++)
                {
                    _faceClassScript.faceTab[currentFace].arenaWall.transform.GetChild(i).GetComponent<WallChange>().ReEnablingWallBoxColliders();
                }
                _previousFaceAnimator.SetBool("isRising", false);

            }
            if (timerRatio >1)
            {
                ReadyText.text = "";
                currentPlayersOnArena = playerMax;
                lerpTimer = 0;
                timerRatio = 0;
                _isRisingHappened = false;
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

    //debug
    private void OnGUI()
    {
        if (debug)
        {
            GUI.color = Color.black;
            GUILayout.Label("PlayersOnArena : " + currentPlayersOnArena);
            //GUILayout.Label("diceCameraDistance : " + diceCameraDistance);
        }
    }

    //permet de reset et de replacer les joueurs à chaque changement de faces
    private void PlayerReset(List<GameObject> player)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        }
    }

    private void PlayerLerp(int playerNumber, float timerRatio)
    {
        attackTestScripts[playerNumber].GetPlayerScoreImage().color = Color32.Lerp(attackTestScripts[playerNumber].GetPlayerScoreImage().color, new Color32(255, 255, 255, 255), timerRatio);
        playerList[playerNumber].transform.position = Vector3.Lerp(playerList[playerNumber].transform.position, _faceClassScript.faceTab[currentFace].playerStartingPosition[playerNumber].position, timerRatio);
        if(timerRatio >= 1)
        {
            print("reached");

            playersEntityScripts[playerNumber].enabled = true;
           
        }
        else
        {
            playersEntityScripts[playerNumber].enabled = false;
        }
    }

    private void ReEnablingColliders(int playerNumber)
    {
        BoxCollider2D[] playerColliders = playerList[playerNumber].GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D colliders in playerColliders)
        {
            colliders.enabled = true;
        }


    }

    public void ThisPlayerHasLost(string player)
    {
        if(currentPlayersList.Count > 2)
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
        if(!_scoreManagerScript.GetSuddenDeath())
        {
            currentPlayersList.Clear();
            for (int i = playerList.Count; i-- > 0;)
            {
                currentPlayersList.Add(playerList[i]);
            }
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

    public bool[] GetMenuInfoMouvementBool()
    {
        return menuInfoMouvementBool;
    }

    public void SetPreviousFaceAnimatorRisingFalse()
    {
        _previousFaceAnimator.SetBool("isRising", false);

    }

    public void SetPreviousFaceAnimatorFallingFalse()
    {
        _previousFaceAnimator.SetBool("isFalling", false);
    }

    public void SetPlayerMax(int playerNumber)
    {
        playerMax = playerNumber;
    }

    public List<GameObject> GetCurrentPlayerList()
    {
        return currentPlayersList;
    }

    public void SetSuddenDeathPlayerToDisappear(GameObject playerToDisappear)
    {
        suddenDeathPlayerToDisappear.Add(playerToDisappear);
    }

    public PlayerEntity[] GetPlayerEntityScripts()
    {
        return playersEntityScripts;
    }

    public ArenaRotation GetArenaRotation()
    {
        return _arenaRotationScript;
    }
}
