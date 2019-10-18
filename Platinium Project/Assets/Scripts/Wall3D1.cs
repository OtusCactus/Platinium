using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall3D1 : MonoBehaviour
{
    //merge des scripts Wall et Wall3D pour la scène Proto.

    [Header("Propriétés")]
    public float wallLifeMax;
    public float wallLimitVelocity;
    private float wallLife;
    //
    private bool _lastHit = false;

    //Materials
    [Header("Apparence")]
    public Material[] wallAppearance;

    private float _playerVelocityRatio;


    //differents scripts 
    private WallManager _wallManagerScript;
    private GameManager _gameManagerScript;
    private CameraMouvements _cameraMouvementsScript;

    //variables pour le changement de face de l'arène
    private int _currentFace;
    private int _nextFace;

    // Start is called before the first frame update
    void Start()
    {

        //récupération des scripts
        _cameraMouvementsScript = GameObject.FindWithTag("MainCamera").GetComponent<CameraMouvements>();
        _wallManagerScript = GameObject.FindWithTag("GameController").GetComponent<WallManager>();
        _gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();

        // set les valeurs de départs
        wallLife = wallLifeMax;
        _currentFace = _cameraMouvementsScript._cameraPositionNumber;

        //set le material du mur par défaut
        GetComponent<MeshRenderer>().material = wallAppearance[0];
    }

    // Update is called once per frame
    void Update()
    {
        //si la caméra est en train de changer de face, désactive les sprites ainsi que les colliders des murs, reset la vie des murs et
        //actualise la face actuelle de la caméra
        if (_cameraMouvementsScript._isTurning)
        {

            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            _currentFace = _cameraMouvementsScript._cameraPositionNumber;
            wallLife = wallLifeMax;
        }
        //sinon réactive les colliders et les sprites des murs.
        else
        {
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;

        }

        //Debug.Log(_nextFace + " next face " + this.gameObject.name);
        //Debug.Log(_currentFace + " current face " + this.gameObject.name);

        //permet de savoir quel mur est descendu à 0 et vers quelle face tourner l'arène
        if (wallLife <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
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
            //renvoie la prochaine face vers le script de rotation de caméra
            _cameraMouvementsScript._cameraPositionNumber = _nextFace;

            _lastHit = true;
            GetComponent<MeshRenderer>().material = wallAppearance[2];
        }
        if(wallLife < wallLifeMax && !_lastHit)
        {
            GetComponent<MeshRenderer>().material = wallAppearance[1];
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerVelocityRatio = collision.GetComponent<PlayerEntity>().GetVelocityRatio();
        //
        if (_playerVelocityRatio >= wallLimitVelocity)
        {
            wallLife = 0;
        }
        else if (_playerVelocityRatio < wallLimitVelocity)
        {
            wallLife -= _playerVelocityRatio;
        }
        if (_lastHit)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
