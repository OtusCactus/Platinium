using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Grégoire s'est occupé des arrays des différents murs

    //tableaux contenant les prochaines faces selon la face où l'on est et selon le mur détruit.
    [Header("NextFaceArrays")]
    public int[] _wallNorthEastTab;
    public int[] _wallNorthWestTab;
    public int[] _wallSouthEastTab;
    public int[] _wallSouthTab;
    public int[] _wallSouthWestTab;

    //public Vector3 destroyedWallPosition;
    //public string playerWhoDestroyedWall;

    private FaceClass faceClassScript;

    public GameObject[] player;
    //public GameObject player2;
    //public GameObject player3;
    //public GameObject player4;
    private MenuManager menuManagerScript;

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
        menuManagerScript = GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>();
        if (menuManagerScript != null && player.Length > 0)
        {
            for (int j = 0; j < player.Length; j++)
            {
                player[j].SetActive(false);
                player[j] = null;
            }
        }

        for (int i = 0; i < menuManagerScript.numbersOfPlayers; i++)
        {
            GameObject playerInstantiation = Instantiate(playerPrefabs[i]);
            player[i] = playerInstantiation;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        faceClassScript = GetComponent<FaceClass>();

        

        player[0].transform.position = faceClassScript.faceTab[0].player1StartingPosition.position;
        player[1].transform.position = faceClassScript.faceTab[0].player2StartingPosition.position;
        if(player[2] != null)
        player[2].transform.position = faceClassScript.faceTab[0].player3StartingPosition.position;
        if (player[3] != null)
        player[3].transform.position = faceClassScript.faceTab[0].player4StartingPosition.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (isTurning)
        {
            player[0].SetActive(false);
            player[1].SetActive(false);

            player[0].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player[1].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player[0].transform.position = faceClassScript.faceTab[currentFace].player1StartingPosition.position;
            player[1].transform.position = faceClassScript.faceTab[currentFace].player2StartingPosition.position;


            if (player[2] != null)
            {
                player[2].SetActive(false);
                player[2].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player[2].transform.position = faceClassScript.faceTab[currentFace].player3StartingPosition.position;

            }
            if (player[3] != null)
            {
                player[3].SetActive(false);
                player[3].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player[3].transform.position = faceClassScript.faceTab[currentFace].player4StartingPosition.position;

            }

        }
        else
        {
            player[0].SetActive(true);
            player[1].SetActive(true);
            if (player[2] != null)
            player[2].SetActive(true);
            if (player[3] != null)
            player[3].SetActive(true);
        }
    }
}
