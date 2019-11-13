using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChange : MonoBehaviour
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
    public Mesh[] wallAppearance;
    private Material[] _meshMaterials;
    private MeshFilter _wallMesh;

    private PlayerEntity _playerOnCollision;
    private float _playerVelocityRatio;
    private BoxCollider2D _wallCollider;
    private MeshRenderer _wallMeshRenderer;

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
    

    private WallProprieties _wallProprieties;


    // Start is called before the first frame update
    void Start()
    {

        //récupération des scripts
        _arenaRotationScript = arena.GetComponent<ArenaRotation>();
        _wallManagerScript = GameObject.FindWithTag("WallController").GetComponent<WallManager>();
        _gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _scoreManagerScript = GameObject.FindWithTag("GameController").GetComponent<ScoreManager>();
        _faceClassScript = GameObject.FindWithTag("GameController").GetComponent<FaceClass>();
        _wallProprieties = GetComponent<WallProprieties>();

        // set les valeurs de départs
        wallLife = wallLifeMax;
        _currentFace = _arenaRotationScript._currentFace;

        //set les valeurs pour screenshake
        _cameraStartPosition = camera.transform.position;
        numberWallState = numberWallStateMax;

        //set le material du mur par défaut
        _meshMaterials = GetComponent<MeshRenderer>().materials;
        _meshMaterials[0].color = new Color32(30, 255, 0, 255);
        _meshMaterials[1].color = new Color32(30, 200, 0, 255);
        _meshMaterials[2].color = new Color32(5, 255, 0, 255);
        _meshMaterials[3].color = new Color32(28, 235, 0, 255);
        _meshMaterials[4].color = new Color32(20, 189, 0, 255);

        _wallMesh = GetComponent<MeshFilter>();
        _wallMeshRenderer = GetComponent<MeshRenderer>();

        _wallCollider = GetComponent<BoxCollider2D>();
        gameObject.layer = 15;


        //for (int i = 0; i < _faceClassScript.faceTab[_currentFace].wallToHideNextToFace.Length; i++)
        //{
        //    _faceClassScript.faceTab[_currentFace].wallToHideNextToFace[i].enabled = false;
        //}
        //for (int i = 0; i < _faceClassScript.faceTab[_currentFace].arenaWall.transform.childCount; i++)
        //{
        //    _faceClassScript.faceTab[_currentFace].arenaWall.transform.GetChild(i).gameObject.layer = 14;

        //}
    }

    // Update is called once per frame
    void Update()
    {

        //si la caméra est en train de changer de face, désactive les sprites ainsi que les colliders des murs, reset la vie des murs et
        //actualise la face actuelle de la caméra
        if (_arenaRotationScript._isTurning)
        {

            _wallCollider.enabled = true;
            _wallMeshRenderer.enabled = true;
            _currentFace = _arenaRotationScript._currentFace;
            _lastHit = false;
            wallLife = wallLifeMax;
            _wallMesh.mesh = wallAppearance[0];
            _meshMaterials[0].color = new Color32(30, 255, 0, 255);
            _wallMeshRenderer.enabled = true;



            //for (int i = 0; i < _faceClassScript.faceTab[_currentFace].wallToHideNextToFace.Length; i++)
            //{
            //    _faceClassScript.faceTab[_currentFace].wallToHideNextToFace[i].enabled = false;
            //}
            //gameObject.layer = 15;
            //for (int i = 0; i < _faceClassScript.faceTab[_currentFace].arenaWall.transform.childCount; i++)
            //{
            //    _faceClassScript.faceTab[_currentFace].arenaWall.transform.GetChild(i).gameObject.layer = 14;

            //}
        }
        //sinon réactive les colliders et les sprites des murs.
        //else
        //{
        //    for (int i = 0; i < _faceClassScript.faceTab[_currentFace].wallToHideInOtherFace.Length; i++)
        //    {
        //        _faceClassScript.faceTab[_currentFace].wallToHideInOtherFace[i].enabled = false;
        //    }

        //}

        if (wallLife <= 0)
        {
            _lastHit = true;
            if (numberWallState > numberWallStateMax - 4) ShakeScreen();
            _wallMeshRenderer.enabled = false;
            _wallCollider.isTrigger = true;

        }
        else if (wallLife < wallLifeMax && wallLife >= (wallLifeMax * 0.66))
        {
            if (numberWallState > numberWallStateMax - 1) ShakeScreen();
            _wallMesh.mesh = wallAppearance[1];
            
        }
        else if (wallLife < (wallLifeMax * 0.66) && wallLife > (wallLifeMax * 0.33))
        {
            if (numberWallState > numberWallStateMax - 2) ShakeScreen();
            _wallMesh.mesh = wallAppearance[2];
        }
        else if (wallLife < (wallLifeMax * 0.33) && wallLife > 0)
        {
            if (numberWallState > numberWallStateMax - 3) ShakeScreen();
            _wallMesh.mesh = wallAppearance[3];
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerOnCollision = collision.GetComponent<PlayerEntity>();
        _playerVelocityRatio = _playerOnCollision.GetVelocityRatio();

        //Si le mur n'est pas indestructible et que le joueur ne donne pas d'input (debug du problème ou le joueur charge la puissance en tournant et le mur prend des dégats) alors le mur prend des dégats
        if (!_wallProprieties.isIndestructible && _playerOnCollision.GetPlayerINPUTSTATE() != PlayerEntity.INPUTSTATE.GivingInput)
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
                if(_gameManagerScript.currentPlayersOnArena > 2)
                {

                    _wallMeshRenderer.enabled = false;
                    _gameManagerScript.currentPlayersOnArena--;
                    _playerOnCollision.enabled = false;
                    CircleCollider2D[] collisionColliders = collision.GetComponents<CircleCollider2D>();
                    foreach(CircleCollider2D colliders in collisionColliders)
                    {
                        colliders.enabled = false;
                    }
                    
                }
                else
                {
                    if (collision.tag == "Player1")
                    {
                        //_scoreManagerScript.AddScore(1);
                    }
                    else if (collision.tag == "Player2")
                    {
                        //_scoreManagerScript.AddScore(2);
                    }
                    else if (collision.tag == "Player3")
                    {
                        //_scoreManagerScript.AddScore(3);
                    }
                    else if (collision.tag == "Player4")
                    {
                        //_scoreManagerScript.AddScore(4);
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
                    _wallCollider.enabled = false;
                    _wallCollider.isTrigger = false;

                    _gameManagerScript.currentFace = _nextFace - 1;
                    _arenaRotationScript._currentFace = _nextFace - 1;
                    _lastHit = false;
                }
                
            }

            _meshMaterials[0].color = Color32.Lerp(_meshMaterials[0].color, new Color32(236, 25, 25, 255), (wallLifeMax - wallLife) / 3);
        }
            
        
    }

    private void ShakeScreen()
    {
        switch (gameObject.name)
        {
            case "WallNorthEast":
                camera.transform.position = new Vector3(Mathf.PingPong(Time.time * speedShake, magnitudeShake + magnitudeShake) + _cameraStartPosition.x - magnitudeShake, Mathf.PingPong(Time.time * speedShake, magnitudeShake + magnitudeShake) + _cameraStartPosition.y - magnitudeShake, _cameraStartPosition.z);
                break;
            case "WallNorthWest":
                camera.transform.position = new Vector3(-(Mathf.PingPong(Time.time * speedShake, magnitudeShake + magnitudeShake) + _cameraStartPosition.x + magnitudeShake), Mathf.PingPong(Time.time * speedShake, magnitudeShake + magnitudeShake) + _cameraStartPosition.y - magnitudeShake, _cameraStartPosition.z);
                break;
            case "WallSouthWest":
                camera.transform.position = new Vector3(Mathf.PingPong(Time.time * speedShake, magnitudeShake + magnitudeShake) + _cameraStartPosition.x - magnitudeShake, Mathf.PingPong(Time.time * speedShake, magnitudeShake + magnitudeShake) + _cameraStartPosition.y - magnitudeShake, _cameraStartPosition.z);
                break;
            case "WallSouth":
                camera.transform.position = new Vector3(_cameraStartPosition.x, Mathf.PingPong(Time.time * speedShake, magnitudeShake + magnitudeShake) + _cameraStartPosition.y - magnitudeShake, _cameraStartPosition.z);
                break;
            case "WallSouthEast":
                camera.transform.position = new Vector3(-(Mathf.Cos(Time.time * speedShake) * magnitudeShake) + _cameraStartPosition.x, (Mathf.Sin(Time.time * speedShake) * magnitudeShake) + _cameraStartPosition.y, _cameraStartPosition.z);
                break;
        }
        
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
        _meshMaterials[0].color = Color32.Lerp(_meshMaterials[0].color, new Color32(236, 25, 25, 255), (wallLifeMax - wallLife) / 3);
        if (dammage >= wallLimitVelocity)
        {
            wallLife = 0;
        }
        else if (dammage < wallLimitVelocity)
        {
            wallLife -= dammage;
        }
        _meshMaterials[0].color = Color32.Lerp(_meshMaterials[0].color, new Color32(236, 25, 25, 255), (wallLifeMax - wallLife) / 3);

    }

    public float GetPlayerVelocityRatio()
    {
        return _playerVelocityRatio;
    }
}
