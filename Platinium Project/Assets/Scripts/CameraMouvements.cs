using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvements : MonoBehaviour
{
    //Grégoire s'est occupé de ce script

    //positions de caméra
    //[Header("CameraPositions")]
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Quaternion _startRotation;
    private Quaternion _endRotation;

    //distance entre la caméra et l'arène
    private float diceCameraDistance;
    //public Transform[] faceCenter;

    //arène
    [Header("Arène")]
    public GameObject twelveSidedDice;

    //face utilisée
    [System.NonSerialized]
    public int _cameraPositionNumber;
    private int _cameraCurrentHolder;

    //caméra qui tourne
    [Header("ArenaRotation")]
    [System.NonSerialized]
    public bool _isTurning;
    private float timerClamped;
    private float _turningTimer;
    public float turningTimerMax;

    //GameManager
    [Header("gameManager")]
    public GameObject gameManager;
    private FaceClass _faceClassScript;
    private GameManager _gameManagerScript;

    //debug
    [Header("Debug")]
    public bool debug;

    // Start is called before the first frame update
    void Start()
    {
        //Scripts nécessaires
        _faceClassScript = gameManager.GetComponent<FaceClass>();
        _gameManagerScript = gameManager.GetComponent<GameManager>();

        //set la caméra sur la première face de l'arène.
        _cameraPositionNumber = 0;
        _cameraCurrentHolder = _cameraPositionNumber;
        transform.position = _faceClassScript.faceTab[_cameraPositionNumber].cameraPosition.position;
        transform.rotation = _faceClassScript.faceTab[_cameraPositionNumber].cameraPosition.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //permet d'avoir accès à la distance de la caméra
        diceCameraDistance = Vector3.Distance(this.transform.position, twelveSidedDice.transform.position);

        //si la face de la caméra doit changer, permet de modifier la position et rotation de la caméra en fonction de la face sur laquelle on doit aller
        if (_cameraCurrentHolder != _cameraPositionNumber)
        {
            _startPosition = transform.position;
            _endPosition = _faceClassScript.faceTab[_cameraPositionNumber - 1].cameraPosition.position;

            _startRotation = transform.rotation;
            _endRotation = _faceClassScript.faceTab[_cameraPositionNumber - 1].cameraPosition.rotation;

            //permet la rotation
            _isTurning = true;
            //reset la condition pour pouvoir tourner lors de la prochaine face
            _cameraCurrentHolder = _cameraPositionNumber;
        }
       
        //permet de tourner la caméra sur la prochaine face dans l'ordre de 1 à 12
        if(Input.GetKeyDown(KeyCode.N) && !_isTurning && _cameraPositionNumber < _faceClassScript.faceTab.Length -1)
        {
            Debug.Log("OK");
            _cameraPositionNumber += 1;

            _startPosition = transform.position;
            _endPosition = _faceClassScript.faceTab[_cameraPositionNumber].cameraPosition.position;

            _startRotation = transform.rotation;
            _endRotation = _faceClassScript.faceTab[_cameraPositionNumber].cameraPosition.rotation;

            _isTurning = true;
        }
        //permet de tourner la caméra sur la face précédente dans l'ordre de 1 à 12
        if (Input.GetKeyDown(KeyCode.B) && !_isTurning && _cameraPositionNumber > 0)
        {
            Debug.Log("OK");
            _cameraPositionNumber -= 1;

            _startPosition = transform.position;
            _endPosition = _faceClassScript.faceTab[_cameraPositionNumber].cameraPosition.position;

            _startRotation = transform.rotation;
            _endRotation = _faceClassScript.faceTab[_cameraPositionNumber].cameraPosition.rotation;
            _isTurning = true;
        }


        if (_isTurning)
        {
            //permet de lancer le lerp de la caméra
            _turningTimer += Time.deltaTime;
            timerClamped = _turningTimer / turningTimerMax;

            //change le transform de la caméra
            transform.position = Vector3.Lerp(_startPosition, _endPosition, timerClamped);
            Quaternion currentRotation = Quaternion.Lerp(_startRotation, _endRotation, timerClamped);
            transform.rotation = currentRotation;


            //reset le lerp.
            if (timerClamped >= 1)
            {
                timerClamped = 0;
                _turningTimer = 0;
                _isTurning = false;
            }
        }
    }

    //debug
    private void OnGUI()
    {
        if (debug)
        {
            GUILayout.Label("TimerClamped : " + timerClamped);
            GUILayout.Label("diceCameraDistance : " + diceCameraDistance);
        }
    }
}
