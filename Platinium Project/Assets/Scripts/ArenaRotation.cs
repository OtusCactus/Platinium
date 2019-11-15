﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaRotation : MonoBehaviour
{
    //Grégoire s'est occupé de ce script

    //positions de caméra
    //[Header("CameraPositions")]
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Quaternion _startRotation;
    private Quaternion _endRotation;

    //distance entre la caméra et l'arène
    //private float diceCameraDistance;

    public Camera mainCamera;
    public Transform arenaLookAt;
    //public Transform[] faceCenter;

    //arène
    [Header("Arène")]
    public GameObject arena;

    //face utilisée
    public int _currentFace;
    private int _faceStored;

    //arene qui tourne
    [Header("ArenaRotation")]
    [System.NonSerialized]
    public bool _isTurning;
    public bool _hasRoundBegun;

    private float timerClamped;
    private float _turningTimer;
    public float turningTimerMax;

    //GameManager
    [Header("gameManager")]
    public GameObject gameManager;
    private FaceClass _faceClassScript;
    private GameManager _gameManagerScript;
    private SoundManager _soundManagerScript;
    private PlayerManager _playerManagerScript;
    //debug
    [Header("Debug")]
    public bool debug;

    // Start is called before the first frame update
    void Start()
    {
        //Scripts nécessaires
        _faceClassScript = gameManager.GetComponent<FaceClass>();
        _soundManagerScript = gameManager.GetComponent<SoundManager>();
        _playerManagerScript = gameManager.GetComponent<PlayerManager>();
        _gameManagerScript = gameManager.GetComponent<GameManager>();

        //set la caméra sur la première face de l'arène.
        _currentFace = 0;
        _faceStored = _currentFace;
        transform.rotation = _faceClassScript.faceTab[_currentFace].arenaRotation.rotation;


        for (int i = 0; i < _faceClassScript.faceTab[_currentFace].wallToHideNextToFace.Length; i++)
        {
            _faceClassScript.faceTab[_currentFace].wallToHideNextToFace[i].SetActive(false);
        }
        
        for (int i = 0; i < _faceClassScript.faceTab[_currentFace].arenaWall.transform.childCount; i++)
        {
            _faceClassScript.faceTab[_currentFace].arenaWall.transform.GetChild(i).gameObject.layer = 14;

        }
    }

    // Update is called once per frame
    void Update()
    {
        //permet d'avoir accès à la distance de la caméra
        //diceCameraDistance = Vector3.Distance(arena.transform.position, mainCamera.transform.position);

        //si la face de l'arène doit changer, permet de chercher la rotation nécéssaire à effectuer puis de jouer le son de fin de round
        if (_faceStored != _currentFace)
        {
            _startRotation = transform.rotation;
            _endRotation = _faceClassScript.faceTab[_currentFace].arenaRotation.rotation;

            _soundManagerScript.PlaySound(_soundManagerScript.myAudio, _soundManagerScript.endRound);

            //permet la rotation
            _isTurning = true;
            _gameManagerScript.isTurning = true;
            for (int i = 0; i < _faceClassScript.faceTab[_faceStored].arenaWall.transform.childCount; i++)
            {
                _faceClassScript.faceTab[_faceStored].arenaWall.transform.GetChild(i).gameObject.layer = 15;
            }
            //reset la condition pour pouvoir tourner lors de la prochaine face
            _faceStored = _currentFace;
        }

        //permet de tourner l'arène sur la prochaine face dans l'ordre de 1 à 12
        if (Input.GetKeyDown(KeyCode.N) && !_isTurning && _currentFace < _faceClassScript.faceTab.Length - 1)
        {
            Debug.Log("OK");
            _currentFace += 1;

           

            _startRotation = transform.rotation;
            _endRotation = _faceClassScript.faceTab[_currentFace].arenaRotation.rotation;

            _isTurning = true;
        }
        //permet de tourner l'arène sur la face précédente dans l'ordre de 1 à 12
        if (Input.GetKeyDown(KeyCode.B) && !_isTurning && _currentFace > 0)
        {
            _currentFace -= 1;

           

            _startRotation = transform.rotation;
            _endRotation = _faceClassScript.faceTab[_currentFace].arenaRotation.rotation;
            _isTurning = true;
        }


        if (_isTurning)
        {
            //permet de lancer le lerp de l'arène
            _turningTimer += Time.deltaTime;
            timerClamped = _turningTimer / turningTimerMax;

            //change le transform de l'arène
            Quaternion currentRotation = Quaternion.Lerp(_startRotation, _endRotation, timerClamped);
            transform.rotation = currentRotation;

            //stop les vibrations si vibrations il y a

            for (int i = 0; i < _playerManagerScript.player.Count; i++)
            {
                _playerManagerScript.player[i].StopVibration();
            }


            for (int i = 0; i < _faceClassScript.faceTab[_currentFace].wallToHideNextToFace.Length; i++)
            {
                _faceClassScript.faceTab[_currentFace].wallToHideNextToFace[i].SetActive(false);
            }
            for (int i = 0; i < _faceClassScript.faceTab[_currentFace].arenaWall.transform.childCount; i++)
            {
                _faceClassScript.faceTab[_currentFace].arenaWall.transform.GetChild(i).gameObject.layer = 14;

            }


            //reset le lerp.
            if (timerClamped >= 1)
            {
                timerClamped = 0;
                _turningTimer = 0;
                _soundManagerScript.PlaySound(_soundManagerScript.myAudio, _soundManagerScript.endRound);
                _isTurning = false;
                _gameManagerScript.isTurning = false;
                _gameManagerScript.hasRoundBegun = true;
            }
        }
        else
        {
            for (int i = 0; i < _faceClassScript.faceTab[_currentFace].wallToHideInOtherFace.Length; i++)
            {
                _faceClassScript.faceTab[_currentFace].wallToHideInOtherFace[i].SetActive(false);
            }

        }
    }

    //debug
    private void OnGUI()
    {
        if (debug)
        {
            //GUILayout.Label("TimerClamped : " + timerClamped);
            //GUILayout.Label("diceCameraDistance : " + diceCameraDistance);
        }
    }
}
