using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvements : MonoBehaviour
{

    public Transform Dice;
    public Transform[] cameraPosition;
    public float turningTimerMax;


    private int _cameraPositionNumber;
    private bool _isTurning;
    private float _turningTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Dice);
        if(Input.GetKeyDown(KeyCode.N) && !_isTurning && _cameraPositionNumber < 5)
        {
            Debug.Log("OK");
            _cameraPositionNumber += 1;
            _isTurning = true;
        }

        if (Input.GetKeyDown(KeyCode.B) && !_isTurning && _cameraPositionNumber > 0)
        {
            Debug.Log("OK");
            _cameraPositionNumber -= 1;
            _isTurning = true;
        }

        if (_isTurning)
        {
            _turningTimer += Time.deltaTime;
            float timerClamped = _turningTimer / turningTimerMax;
            Debug.Log(timerClamped);
            transform.position = Vector3.Lerp(transform.position, cameraPosition[_cameraPositionNumber].position, timerClamped);
            if(timerClamped >= 1)
            {
                timerClamped = 0;
                _turningTimer = 0;
                _isTurning = false;
            }
        }
    }
}
