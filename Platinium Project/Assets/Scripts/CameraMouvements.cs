using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvements : MonoBehaviour
{
    //positions de caméra
    [Header("CameraPositions")]
    public Transform[] cameraPosition;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Quaternion _startRotation;
    private Quaternion _endRotation;

    //face utilisée
    private int _cameraPositionNumber;

    //objet qui tourne
    [Header("ArenaRotation")]
    private bool _isTurning;
    private float timerClamped;
    private float _turningTimer;
    public float turningTimerMax;


    //debug
    public bool debug;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


       
        if(Input.GetKeyDown(KeyCode.N) && !_isTurning && _cameraPositionNumber < cameraPosition.Length -1)
        {
            Debug.Log("OK");
            _cameraPositionNumber += 1;

            _startPosition = transform.position;
            _endPosition = cameraPosition[_cameraPositionNumber].position;

            _startRotation = transform.rotation;
            _endRotation = cameraPosition[_cameraPositionNumber].rotation;

            _isTurning = true;
        }

        if (Input.GetKeyDown(KeyCode.B) && !_isTurning && _cameraPositionNumber > 0)
        {
            Debug.Log("OK");
            _cameraPositionNumber -= 1;

            _startPosition = transform.position;
            _endPosition = cameraPosition[_cameraPositionNumber].position;

            _startRotation = transform.rotation;
            _endRotation = cameraPosition[_cameraPositionNumber].rotation;
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
