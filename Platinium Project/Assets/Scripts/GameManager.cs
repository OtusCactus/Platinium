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

    //public Vector3 destroyedWallPosition;
    //public string playerWhoDestroyedWall;


    private FaceClass _faceClassScript;
    private MenuManager _menuManagerScript;
    private ScoreManager _scoreManagerScript;

    public GameObject[] player;
    //public GameObject player2;
    //public GameObject player3;
    //public GameObject player4;

    public GameObject[] playerPrefabs;

    public bool isTurning;

    //private float timer;
    //private float timer2;
    //public float timerMax;
    //public float timerMax2;
    public int currentFace;
    //score

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

        


    }

    // Start is called before the first frame update
    void Start()
    {
        //set la position de départ des joueurs
        for (int i = 0; i < player.Length; i++)
        {
            player[i].transform.position = _faceClassScript.faceTab[0].playerStartingPosition[i].position;
        }



        //player[0].transform.position = _faceClassScript.faceTab[0].player1StartingPosition.position;
        //player[1].transform.position = _faceClassScript.faceTab[0].player2StartingPosition.position;
        //if(player[2] != null)
        //player[2].transform.position = _faceClassScript.faceTab[0].player3StartingPosition.position;
        //if (player[3] != null)
        //player[3].transform.position = _faceClassScript.faceTab[0].player4StartingPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        //check si on doit changer de face de l'arène
        if (isTurning)
        { 
            PlayerReset(player);





            //player[0].SetActive(false);
            //player[1].SetActive(false);

            //player[0].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //player[1].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //player[0].transform.position = _faceClassScript.faceTab[currentFace].playerStartingPosition[0].position;
            //player[1].transform.position = _faceClassScript.faceTab[currentFace].playerStartingPosition[1].position;


            //if (player[2] != null)
            //{
            //    player[2].SetActive(false);
            //    player[2].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //    player[2].transform.position = _faceClassScript.faceTab[currentFace].playerStartingPosition[2].position;

            //}
            //if (player[3] != null)
            //{
            //    player[3].SetActive(false);
            //    player[3].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //    player[3].transform.position = _faceClassScript.faceTab[currentFace].playerStartingPosition[3].position;

            //}

        }
        else
        {
            //réactive les joueurs quand le changement de face est terminé
            for (int i = 0; i < player.Length; i++)
            {
                player[i].SetActive(true);
            }
            //player[0].SetActive(true);
            //player[1].SetActive(true);
            //if (player[2] != null)
            //player[2].SetActive(true);
            //if (player[3] != null)
            //player[3].SetActive(true);
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
}
