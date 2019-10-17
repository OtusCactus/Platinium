using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvements : MonoBehaviour
{
    //positions de caméra
    //[Header("CameraPositions")]
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Quaternion _startRotation;
    private Quaternion _endRotation;

    //face utilisée
    [System.NonSerialized]
    public int _cameraPositionNumber;
    private int _cameraCurrentHolder;

    //objet qui tourne
    [Header("ArenaRotation")]
    [System.NonSerialized]
    public bool _isTurning;
    private float timerClamped;
    private float _turningTimer;
    public float turningTimerMax;

    //GameManager
    public GameObject gameManager;
    private FaceClass _faceClassScript;
    private GameManager _gameManagerScript;

    //debug
    public bool debug;

    // Start is called before the first frame update
    void Start()
    {
        _faceClassScript = gameManager.GetComponent<FaceClass>();
        _gameManagerScript = gameManager.GetComponent<GameManager>();
        _cameraPositionNumber = 0;
        _cameraCurrentHolder = _cameraPositionNumber;
    }

    // Update is called once per frame
    void Update()
    {

        if (_cameraCurrentHolder != _cameraPositionNumber)
        {
            _startPosition = transform.position;
            _endPosition = _faceClassScript.faceTab[_cameraPositionNumber - 1].cameraPosition.position;

            _startRotation = transform.rotation;
            _endRotation = _faceClassScript.faceTab[_cameraPositionNumber - 1].cameraPosition.rotation;

            _isTurning = true;
            _cameraCurrentHolder = _cameraPositionNumber;
        }
       
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
            _turningTimer += Time.deltaTime;

            timerClamped = _turningTimer / turningTimerMax;
            transform.position = Vector3.Lerp(_startPosition, _endPosition, timerClamped);
            Quaternion currentRotation = Quaternion.Lerp(_startRotation, _endRotation, timerClamped);
            transform.rotation = currentRotation;


            if (timerClamped >= 1)
            {
                timerClamped = 0;
                _turningTimer = 0;
                _isTurning = false;
            }
        }
    }

    private void OnGUI()
    {
        if (debug)
        {
            GUILayout.Label("TimerClamped : " + timerClamped);
        }
    }
}
