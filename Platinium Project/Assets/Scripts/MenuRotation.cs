using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRotation : MonoBehaviour
{
    public FaceClassMenu faceClassMenuScript;
    public InMenuPlayer inMenuPlayerScript;

    public GameObject player;

    private float _turningTimer;
    public float turningTimerMax;
    private float timerClamped;


    // Start is called before the first frame update
    void Start()
    {
        //set la position de départ de l'arene
        transform.rotation = faceClassMenuScript.faceTab[0].arenaRotation.rotation;
    }

    // Update is called once per frame
    void Update()
    {



        if (inMenuPlayerScript.isTurning)
        {
            player.SetActive(false);
            //permet de lancer le lerp de la caméra
            _turningTimer += Time.deltaTime;
            timerClamped = _turningTimer / turningTimerMax;
            //change la rotation de l'arène
            Quaternion currentRotation = Quaternion.Lerp(inMenuPlayerScript.arenaRotation, faceClassMenuScript.faceTab[inMenuPlayerScript.currentFace].arenaRotation.rotation, timerClamped);
            transform.rotation = currentRotation;





            //reset le lerp.
            if (timerClamped >= 1)
            {

                player.transform.position = faceClassMenuScript.faceTab[inMenuPlayerScript.currentFace].player1StartingPosition.position;
                player.SetActive(true);

                inMenuPlayerScript.isTurning = false;
                timerClamped = 0;
                _turningTimer = 0;

            }
        }
    }

    
}
