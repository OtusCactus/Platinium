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

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public bool isTurning;

    //private float timer;
    //private float timer2;
    //public float timerMax;
    //public float timerMax2;
    public int currentFace;
    //score

    // Start is called before the first frame update
    void Start()
    {
        faceClassScript = GetComponent<FaceClass>();
        player1.transform.position = faceClassScript.faceTab[0].player1StartingPosition.position;
        player2.transform.position = faceClassScript.faceTab[0].player2StartingPosition.position;
        player3.transform.position = faceClassScript.faceTab[0].player3StartingPosition.position;
        player4.transform.position = faceClassScript.faceTab[0].player4StartingPosition.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (isTurning)
        {


            player1.SetActive(false);
            player2.SetActive(false);
            player3.SetActive(false);
            player4.SetActive(false);

            player1.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player2.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player3.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player4.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player1.transform.position = faceClassScript.faceTab[currentFace].player1StartingPosition.position;
            player2.transform.position = faceClassScript.faceTab[currentFace].player2StartingPosition.position;
            player3.transform.position = faceClassScript.faceTab[currentFace].player3StartingPosition.position;
            player4.transform.position = faceClassScript.faceTab[currentFace].player4StartingPosition.position;
        }
        else
        {
            player1.SetActive(true);
            player2.SetActive(true);
            player3.SetActive(true);
            player4.SetActive(true);

        }
    }
}
