using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall3D : MonoBehaviour
{
    public float wallLife;
    public float wallLimitVelocity;

    //differents scripts 
    private WallManager _wallManagerScript;
    private GameManager _gameManagerScript;
    private CameraMouvements _cameraMouvementsScript;

    private int _currentFace;
    private int _nextFace;

    private bool _lastHit = false;

    //Materials
    public Material[] wallAppearance;

    public float _playerVelocity;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material = wallAppearance[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (wallLife <= 0)
        {
            _lastHit = true;
        }
        if(wallLife < 1 && !_lastHit)
        {
            GetComponent<MeshRenderer>().material = wallAppearance[1];
        }
        if (_lastHit)
        {
            GetComponent<MeshRenderer>().material = wallAppearance[2];
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        _playerVelocity = collision.GetComponent<Rigidbody2D>().velocity.sqrMagnitude/625;
        if (_playerVelocity >= wallLimitVelocity)
        {
            wallLife = 0;
        }
        else if (_playerVelocity < wallLimitVelocity)
        {
            wallLife -= _playerVelocity;
        }
        if (_lastHit)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
