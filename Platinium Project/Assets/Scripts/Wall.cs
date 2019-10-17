﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int wallLife;
    public float wallLimitVelocity;
    public float wallMinVelocity;

    //differents scripts 
    private WallManager _wallManagerScript;
    private GameManager _gameManagerScript;
    private CameraMouvements _cameraMouvementsScript;

    private int _currentFace;
    private int _nextFace;

    public float _playerVelocity;

    private LineRenderer line;
    public float xradius;
    public float yradius;

    // Start is called before the first frame update
    void Start()
    {
        _cameraMouvementsScript = GameObject.FindWithTag("MainCamera").GetComponent<CameraMouvements>();
        _wallManagerScript = GameObject.FindWithTag("GameController").GetComponent<WallManager>();
        _gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();

        _currentFace = _cameraMouvementsScript._cameraPositionNumber;
        wallLife = _wallManagerScript._wallLife;


        //line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cameraMouvementsScript._isTurning)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            wallLife = _wallManagerScript._wallLife;
            _currentFace = _cameraMouvementsScript._cameraPositionNumber;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }

        Debug.Log(_nextFace + " next face " + this.gameObject.name);
        Debug.Log(_currentFace + " current face " + this.gameObject.name);
        if (wallLife <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            //GetComponent<LineRenderer>().enabled = false;

            switch (this.gameObject.name)
            {
                case "WallNorthEast":
                    _nextFace = _wallManagerScript.WallFaceChange(_gameManagerScript._wallNorthEastTab, _currentFace);
                    break;
                case "WallNorthWest":
                    _nextFace = _wallManagerScript.WallFaceChange(_gameManagerScript._wallNorthWestTab, _currentFace);
                    break;
                case "WallSouthWest":
                    _nextFace = _wallManagerScript.WallFaceChange(_gameManagerScript._wallSouthWestTab, _currentFace);
                    break;
                case "WallSouth":
                    _nextFace = _wallManagerScript.WallFaceChange(_gameManagerScript._wallSouthTab, _currentFace);
                    break;
                case "WallSouthEast":
                    _nextFace = _wallManagerScript.WallFaceChange(_gameManagerScript._wallSouthEastTab, _currentFace);
                    break;
            }
            _cameraMouvementsScript._cameraPositionNumber = _nextFace;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        _playerVelocity = collision.GetComponent<Rigidbody2D>().velocity.sqrMagnitude;
        if (_playerVelocity >= wallLimitVelocity)
        {
            wallLife = 0;
        }
        else if (_playerVelocity >= wallMinVelocity && _playerVelocity < wallLimitVelocity)
        {
            CreatePoints();
            wallLife -= 1;
        }
    }

    void CreatePoints()
    {
        float x;
        float y;
        float z = 0f;
        int segments = 25;

        float angle = 20f;
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;

        for (int i = 0; i < ((segments + 1)/2); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments/2);
        }
    }

}
