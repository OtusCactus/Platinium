using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    //Grégoire s'est occupé de ce script

    //vie
    [Header("Vie")]
    private int _wallLife;
    public int wallLifeMax;

    //vélocité
    [Header("Velocité")]
    public float wallLimitVelocity;
    public float _playerVelocity;


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
        _wallLife = wallLifeMax;
        _currentFace = _cameraMouvementsScript._cameraPositionNumber;
    }

    // Update is called once per frame
    void Update()
    {
        //si la caméra est en train de changer de face, désactive les sprites ainsi que les colliders des murs, reset la vie des murs et
        //actualise la face actuelle de la caméra
        if (_cameraMouvementsScript._isTurning)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            _currentFace = _cameraMouvementsScript._cameraPositionNumber;
            _wallLife = wallLifeMax;
        }
        //sinon réactive les colliders et les sprites des murs.
        else
        {
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }

        //Debug.Log(_nextFace + " next face " + this.gameObject.name);
        //Debug.Log(_currentFace + " current face " + this.gameObject.name);

        //permet de savoir quel mur est descendu à 0 et vers quelle face tourner l'arène
        if (_wallLife <= 0)
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
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // si la vitesse du joueur est supérieur au seuil de vitesse du mur, détruit le mur.
        _playerVelocity = collision.GetComponent<Rigidbody2D>().velocity.sqrMagnitude;
        if (_playerVelocity >= wallLimitVelocity)
        {
            _wallLife = 0;
        }
        //sinon, réduit sa vie de 1
        else if (_playerVelocity < wallLimitVelocity)
        {
            _wallLife -= 1;
        }
    }

}
