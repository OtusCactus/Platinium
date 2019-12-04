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
    public Mesh[] wallShadowAppearance;
    private Material[] _meshMaterials;
    private Material[] _meshMaterialsBambou;

    private MeshFilter _wallMesh;
    private MeshRenderer _wallMeshRenderer;

    private MeshFilter _wallShadowMesh;
    private MeshRenderer _wallShadowMeshRenderer;

    private SkinnedMeshRenderer _wallBambouAppearance;
    private SkinnedMeshRenderer _wallShadowMeshRendererBambou;

    private PlayerEntity _playerOnCollision;
    private float _playerVelocityRatio;
    private BoxCollider2D _wallCollider;

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

    bool _hasPlayerPassedTrigger = false;

    private GameObject _currentWallActive;


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
        _wallManagerScript.WhichWall(_wallProprieties);
        wallAppearance = _wallManagerScript.UpdateWallAppearance(_wallProprieties);
        wallShadowAppearance = _wallManagerScript.UpdateWallShadowAppearance(_wallProprieties);

        //set les valeurs pour screenshake
        _cameraStartPosition = camera.transform.position;
        numberWallState = numberWallStateMax;

        

        _wallCollider = GetComponent<BoxCollider2D>();

        gameObject.layer = 15;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.activeSelf == true)
            {
                _currentWallActive = transform.GetChild(i).gameObject;
                if(i == 2)
                {
                    //pilliers
                    _wallShadowMesh = _currentWallActive.transform.GetChild(1).GetChild(0).GetComponent<MeshFilter>();
                    _wallShadowMeshRenderer = _currentWallActive.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>();
                    //bambou
                    _wallBambouAppearance = _currentWallActive.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
                    _wallShadowMeshRendererBambou = _currentWallActive.transform.GetChild(1).GetChild(1).GetComponent<SkinnedMeshRenderer>();

                    _meshMaterials = _wallShadowMeshRenderer.materials;
                    _meshMaterialsBambou = _wallShadowMeshRendererBambou.materials;
                }
                else
                {
                    _wallShadowMesh = _currentWallActive.transform.GetChild(0).GetComponent<MeshFilter>();
                    _wallShadowMeshRenderer = _currentWallActive.transform.GetChild(0).GetComponent<MeshRenderer>();
                    _meshMaterials = _wallShadowMeshRenderer.materials;
                }

            }
        }

        _wallMesh = _currentWallActive.GetComponent<MeshFilter>();
        _wallMeshRenderer = _currentWallActive.GetComponent<MeshRenderer>();
        if (!_wallProprieties.isBouncy)
        {
            _wallMesh.mesh = wallAppearance[0];
        }
        else
        {
            _wallBambouAppearance.sharedMesh = wallAppearance[0];
        }
        if (!_wallProprieties.isIndestructible)
        {
            _meshMaterials[0].color = new Color32(30, 255, 0, 255);
            if (_wallProprieties.isBouncy)
            {
                _meshMaterialsBambou[0].color = new Color32(30, 255, 0, 255);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        //si la caméra est en train de changer de face, désactive les sprites ainsi que les colliders des murs, reset la vie des murs et
        //actualise la face actuelle de la caméra
        if (_arenaRotationScript._isTurning)
        {
            _wallManagerScript.WhichWall(_wallProprieties);
            _wallCollider.enabled = true;
            _wallMeshRenderer.enabled = true;
            //if(_currentWallActive == transform.GetChild(2).gameObject)
            //{
            //    for (int i =0; i < _currentWallActive.transform.childCount; i++)
            //    {
            //        if(_currentWallActive.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
            //        _currentWallActive.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            //    }
            //}

            _currentFace = _arenaRotationScript._currentFace;
            _lastHit = false;
            wallLife = wallLifeMax;
            _wallCollider.isTrigger = false;
            if (!_wallProprieties.isBouncy)
            {
                _wallMesh.mesh = wallAppearance[0];
                _wallShadowMeshRenderer.enabled = true;
                _wallShadowMesh.mesh = wallShadowAppearance[0];
            }
            else
            {
                _wallBambouAppearance.sharedMesh = wallAppearance[0];
                _wallBambouAppearance.enabled = true;
                _wallShadowMeshRendererBambou.enabled = true;
                _wallShadowMeshRendererBambou.sharedMesh = wallShadowAppearance[0];
            }

            if (!_wallProprieties.isIndestructible)
            {
                _meshMaterials[0].color = new Color32(30, 255, 0, 255);
                if (_wallProprieties.isBouncy)
                {
                    _meshMaterialsBambou[0].color = new Color32(30, 255, 0, 255);
                }
            }

        }


        if (wallLife <= 0)
        {
            _lastHit = true;
            if (numberWallState > numberWallStateMax - 4) ShakeScreen();
            _wallMeshRenderer.enabled = false;
            //if (_currentWallActive == transform.GetChild(2).gameObject)
            //{
            //    for (int i = 0; i < _currentWallActive.transform.childCount; i++)
            //    {
            //        if (_currentWallActive.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
            //            _currentWallActive.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
            //    }
            //}

            if (!_wallProprieties.isBouncy)
            {
                _wallShadowMeshRenderer.enabled = false;
            }
            else
            {
                _wallBambouAppearance.enabled = false;
                _wallShadowMeshRendererBambou.enabled = false;
            }

            //_wallShadowMeshRenderer.enabled = false;
            _wallCollider.isTrigger = true;

        }
        else if (wallLife < wallLifeMax && wallLife >= (wallLifeMax * 0.66))
        {
            if (numberWallState > numberWallStateMax - 1) ShakeScreen();
            if (!_wallProprieties.isBouncy)
            {
                _wallMesh.mesh = wallAppearance[1];
                _wallShadowMesh.mesh = wallShadowAppearance[1];
            }
            else
            {
                _wallBambouAppearance.sharedMesh = wallAppearance[1];
                _wallShadowMeshRendererBambou.sharedMesh = wallShadowAppearance[1];
            }
            
        }
        else if (wallLife < (wallLifeMax * 0.66) && wallLife > (wallLifeMax * 0.33))
        {
            if (numberWallState > numberWallStateMax - 2) ShakeScreen();
            //if (!_wallProprieties.isBouncy)
            //{
            //    _wallMesh.mesh = wallAppearance[2];
            //}

            if (!_wallProprieties.isBouncy)
            {
                _wallMesh.mesh = wallAppearance[2];
                _wallShadowMesh.mesh = wallShadowAppearance[2];
            }
            else
            {
                _wallBambouAppearance.sharedMesh = wallAppearance[2];
                _wallShadowMeshRendererBambou.sharedMesh = wallShadowAppearance[2];
            }
        }
        else if (wallLife < (wallLifeMax * 0.33) && wallLife > 0)
        {
            if (numberWallState > numberWallStateMax - 3) ShakeScreen();


            if (!_wallProprieties.isBouncy)
            {
                _wallMesh.mesh = wallAppearance[3];
                _wallShadowMesh.mesh = wallShadowAppearance[3];
            }
            else
            {
                _wallBambouAppearance.sharedMesh = wallAppearance[3];
                _wallShadowMeshRendererBambou.sharedMesh = wallShadowAppearance[3];
            }
        }

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        _playerOnCollision = collision.gameObject.GetComponent<PlayerEntity>();
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

            _meshMaterials[0].color = Color32.Lerp(_meshMaterials[0].color, new Color32(236, 25, 25, 255), (wallLifeMax - wallLife) / 3);
            if (_wallProprieties.isBouncy)
            {
                _meshMaterialsBambou[0].color = Color32.Lerp(_meshMaterialsBambou[0].color, new Color32(236, 25, 25, 255), (wallLifeMax - wallLife) / 3);
            }
        }
            
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerOnCollision = collision.gameObject.GetComponent<PlayerEntity>();

        if (_lastHit)
        {
            if (_gameManagerScript.currentPlayersOnArena > 2)
            {
             
                _wallMeshRenderer.enabled = false;
               _wallShadowMeshRenderer.enabled = false;


                _playerOnCollision.enabled = false;
                _playerOnCollision.DesactiveCollider();
                _scoreManagerScript.ChangeScore(_gameManagerScript.currentPlayersOnArena, int.Parse(collision.gameObject.tag.Substring(collision.gameObject.tag.Length - 1)));
                _gameManagerScript.ThisPlayerHasLost(collision.gameObject.tag);
                _gameManagerScript.currentPlayersOnArena--;
            }
            else if (_gameManagerScript.currentPlayersOnArena <= 2)
            {
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

                _scoreManagerScript.ChangeScore(2, int.Parse(collision.gameObject.tag.Substring(collision.gameObject.tag.Length - 1)));
                _gameManagerScript.ThisPlayerHasLost(collision.gameObject.tag);
                _scoreManagerScript.ChangeScore(1, int.Parse(_gameManagerScript.GetFirstCurrentPlayersItem().gameObject.tag.Substring(_gameManagerScript.GetFirstCurrentPlayersItem().gameObject.tag.Length - 1)));

                _gameManagerScript.ResetCurrentPlayers();

                _gameManagerScript.currentFace = _nextFace - 1;
                _arenaRotationScript._currentFace = _nextFace - 1;
                _lastHit = false;
            }

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
        if (_wallProprieties.isBouncy)
        {
            _meshMaterialsBambou[0].color = Color32.Lerp(_meshMaterialsBambou[0].color, new Color32(236, 25, 25, 255), (wallLifeMax - wallLife) / 3);
        }
        if (dammage >= wallLimitVelocity)
        {
            wallLife = 0;
        }
        else if (dammage < wallLimitVelocity)
        {
            wallLife -= dammage;
        }
        _meshMaterials[0].color = Color32.Lerp(_meshMaterials[0].color, new Color32(236, 25, 25, 255), (wallLifeMax - wallLife) / 3);
        if (_wallProprieties.isBouncy)
        {
            _meshMaterialsBambou[0].color = Color32.Lerp(_meshMaterialsBambou[0].color, new Color32(236, 25, 25, 255), (wallLifeMax - wallLife) / 3);
        }

    }

    public float GetPlayerVelocityRatio()
    {
        return _playerVelocityRatio;
    }

    public bool GetLastHit()
    {
        return _lastHit;
    }
}
