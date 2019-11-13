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

    public int currentPlayersOnArena;

    public bool isTurning;
    
    public int currentFace;

    private GameObject currentLD;

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

        currentPlayersOnArena = player.Length;


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
            PlayerReset(player);
            if (_faceClassScript.faceTab[currentFace].levelDesign != null)
            {
                currentLD = Instantiate(_faceClassScript.faceTab[currentFace].levelDesign);
            }




           

        }
        else
        {
            //réactive les joueurs quand le changement de face est terminé
            for (int i = 0; i < player.Length; i++)
            {
                player[i].SetActive(true);
                //playersEntityScripts[i].enabled = true;
                //CircleCollider2D[] playerColliders = player[i].GetComponents<CircleCollider2D>();
                //foreach(CircleCollider2D colliders in playerColliders)
                //{
                //    colliders.enabled = true;
                //}

            }
  
        }
    }

    //permet de reset et de replacer les joueurs à chaque changement de faces
    private void PlayerReset(GameObject[] player)
    {
        for (int i = 0; i < player.Length; i++)
        {
            player[i].SetActive(false);
            player[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player[i].transform.position = _faceClassScript.faceTab[currentFace].playerStartingPosition[i].position;

            
        }
    }

    private void PlayerLerp(GameObject player)
    {
        //player.transform.position = Vector3.Lerp()
    }
}
