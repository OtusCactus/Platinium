using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChange : MonoBehaviour
{
    //merge des scripts Wall et Wall3D pour la scène Proto.

    [Header("Propriétés")]
    public float wallLifeMax;
    public float wallLimitVelocity;
    public float friction = 15;
    private float wallLife;
    //
    private bool _lastHit = false;

    //Materials
    [Header("Apparence")]
    public Mesh[] wallAppearance;

    private float _playerVelocityRatio;

    //arene
    [Header("Arène")]
    public GameObject arena;
    //differents scripts 
    private WallManager _wallManagerScript;
    private GameManager _gameManagerScript;
    private ArenaRotation _arenaRotationScript;
    private ScoreManager _scoreManagerScript;
    private FaceClass _faceClassScript;

    //variables pour le changement de face de l'arène
    private int _currentFace;
    private int _nextFace;

    [Header("Camera Shake")]
    public Camera camera;
    public float magnitudeShake;
    public float speedShake;
    public float shakeDuration;
    public int numberWallStateMax;
    private int numberWallState;
    private Vector3 _cameraStartPosition;
    private float timer = 0;
   

    // Start is called before the first frame update
    void Start()
    {

        //récupération des scripts
        _arenaRotationScript = arena.GetComponent<ArenaRotation>();
        _wallManagerScript = GameObject.FindWithTag("WallController").GetComponent<WallManager>();
        _gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _scoreManagerScript = GameObject.FindWithTag("GameController").GetComponent<ScoreManager>();
        _faceClassScript = GameObject.FindWithTag("GameController").GetComponent<FaceClass>();

        // set les valeurs de départs
        wallLife = wallLifeMax;
        _currentFace = _arenaRotationScript._cameraPositionNumber;

        //set les valeurs pour screenshake
        _cameraStartPosition = camera.transform.position;
        numberWallState = numberWallStateMax;

        //set le material du mur par défaut
        GetComponent<MeshRenderer>().materials[0].color = new Color32(30, 255, 0, 255);
        GetComponent<MeshRenderer>().materials[1].color = new Color32(30, 200, 0, 255);
        GetComponent<MeshRenderer>().materials[2].color = new Color32(5, 255, 0, 255);
        GetComponent<MeshRenderer>().materials[3].color = new Color32(28, 235, 0, 255);
        GetComponent<MeshRenderer>().materials[4].color = new Color32(20, 189, 0, 255);

        for (int i = 0; i < _faceClassScript.faceTab[_currentFace].wallToHideNextToFace.Length; i++)
        {
            _faceClassScript.faceTab[_currentFace].wallToHideNextToFace[i].enabled = false;
        }
        gameObject.layer = 15;
        for (int i = 0; i < _faceClassScript.faceTab[_currentFace].arenaWall.transform.childCount; i++)
        {
            //_faceClassScript.faceTab[_currentFace].arenaWall.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
            _faceClassScript.faceTab[_currentFace].arenaWall.transform.GetChild(i).gameObject.layer = 14;

        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(wallLife + " " + gameObject.name);

        //si la caméra est en train de changer de face, désactive les sprites ainsi que les colliders des murs, reset la vie des murs et
        //actualise la face actuelle de la caméra
        if (_arenaRotationScript._isTurning)
        {
            _gameManagerScript.isTurning = true;

            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
            _currentFace = _arenaRotationScript._cameraPositionNumber;
            _lastHit = false;
            wallLife = wallLifeMax;
            GetComponent<MeshFilter>().mesh = wallAppearance[0];
            GetComponent<MeshRenderer>().materials[0].color = new Color32(30, 255, 0, 255);


            for (int i = 0; i < _faceClassScript.faceTab[_currentFace].wallToHideNextToFace.Length; i++)
            {
                _faceClassScript.faceTab[_currentFace].wallToHideNextToFace[i].enabled = false;
            }
            gameObject.layer = 15;
            for (int i = 0; i < _faceClassScript.faceTab[_currentFace].arenaWall.transform.childCount; i++)
            {
                //_faceClassScript.faceTab[_currentFace].arenaWall.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
                _faceClassScript.faceTab[_currentFace].arenaWall.transform.GetChild(i).gameObject.layer = 14;

            }
        }
        //sinon réactive les colliders et les sprites des murs.
        else
        {
            for (int i = 0; i < _faceClassScript.faceTab[_currentFace].wallToHideInOtherFace.Length; i++)
            {
                _faceClassScript.faceTab[_currentFace].wallToHideInOtherFace[i].enabled = false;
            }
            _gameManagerScript.isTurning = false;

        }

        if (wallLife <= 0)
        {
            _lastHit = true;
            if (numberWallState > numberWallStateMax - 3) ShakeScreen();
            GetComponent<MeshFilter>().mesh = wallAppearance[3];
        }
        else if (wallLife < wallLifeMax && wallLife >= wallLifeMax / 2)
        {
            if (numberWallState > numberWallStateMax - 1) ShakeScreen();
            GetComponent<MeshFilter>().mesh = wallAppearance[1];
        }
        else if (wallLife < wallLifeMax/2 && wallLife > 0){
            if (numberWallState > numberWallStateMax - 2) ShakeScreen();
            GetComponent<MeshFilter>().mesh = wallAppearance[2];
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
            GetComponent<MeshRenderer>().materials[0].color = Color32.Lerp(GetComponent<MeshRenderer>().materials[0].color, new Color32(236, 25, 25, 255), (wallLifeMax - wallLife) / 3);

            _playerVelocityRatio = collision.GetComponent<PlayerEntity>().GetVelocityRatio();
        if (!GetComponent<WallProprieties>().isIndestructible)
        {
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
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                if (collision.tag == "Player1")
                {
                    _scoreManagerScript.AddScore(1);
                }
                else if (collision.tag == "Player2")
                {
                    _scoreManagerScript.AddScore(2);
                }
                else if (collision.tag == "Player3")
                {
                    _scoreManagerScript.AddScore(3);
                }
                else if (collision.tag == "Player4")
                {
                    _scoreManagerScript.AddScore(4);
                }

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
                _gameManagerScript.currentFace = _nextFace - 1;
                _arenaRotationScript._cameraPositionNumber = _nextFace - 1;
                _lastHit = false;
            }
        }
            
        
    }

    private void ShakeScreen()
    {
        camera.transform.position = new Vector3((Mathf.Cos(Time.time * speedShake) * magnitudeShake) + _cameraStartPosition.x, (Mathf.Sin(Time.time * speedShake) * magnitudeShake) + _cameraStartPosition.y, _cameraStartPosition.z);
        timer += Time.deltaTime;
        if (timer >= shakeDuration)
        {
            camera.transform.position = _cameraStartPosition;
            numberWallState -= 1;
            timer = 0;
        }
    }

    public void SetDammageFromConnect(float dammage)
    {
        wallLife -= dammage;
        GetComponent<MeshRenderer>().materials[0].color = Color32.Lerp(GetComponent<MeshRenderer>().materials[0].color, new Color32(236, 25, 25, 255), (wallLifeMax - wallLife) / 3);
    }
}
