﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChange : MonoBehaviour
{
    //merge des scripts Wall et Wall3D pour la scène Proto.

    [Header("Propriétés")]
    private float _wallLifeMax;
    //public float wallLimitVelocity;
    private float wallLife;
    //
    private bool _lastHit = false;

    //Materials
    [Header("Apparence")]
    public Mesh[] wallAppearance;
    public Mesh[] wallShadowAppearance;
    private Material[] _meshMaterials;
    private Material[] _meshMaterialsOriginal;
    private Material[] _meshMaterialsBambou;

    private MeshFilter _wallMesh;
    private MeshRenderer _wallMeshRenderer;
    private Material[] _wallMeshRendererOriginalMaterials;

    private MeshFilter _wallShadowMesh;
    private MeshRenderer _wallShadowMeshRenderer;

    private SkinnedMeshRenderer _wallBambouAppearance;
    private SkinnedMeshRenderer _wallShadowMeshRendererBambou;

    private PlayerEntity _playerOnCollision;
    private AttackTest _attackTestOnCollision;
    private float _playerVelocityRatio;
    private BoxCollider2D[] _wallCollider = new BoxCollider2D[2];

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

    private bool _hasPlayerCollided = false;
    private Animator _bouncyAnimator;

    private GameObject _currentWallActive;

    private bool _hasCreatedArray = false;
    private bool _hasCreatedArrayTwo = false;


    private NewSoundManager _newSoundManagerScript;
    private GameObject lastEjectedPlayer;
    private float lerpTimerRatio = 0;

    private bool _isShaderNeeded;
    private bool _hasShaderCompletelyAppeared;
    private MeshRenderer _shaderRenderer;
    private float _shaderLerp;
    private float _shaderLerpMax;
    
    private float shakeWallIntensity;
    private float maxShakeWall;
    private bool _isWallShaking = false;
    private float wallShakeTimer;
    private float wallShakeTimerMax;
    private Vector3 wallInitialPosition;

    private void Awake()
    {
        gameObject.layer = 15;

        //récupération des scripts
        _arenaRotationScript = arena.GetComponent<ArenaRotation>();
        _wallManagerScript = GameObject.FindWithTag("WallController").GetComponent<WallManager>();
        _gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _scoreManagerScript = GameObject.FindWithTag("GameController").GetComponent<ScoreManager>();
        _faceClassScript = GameObject.FindWithTag("GameController").GetComponent<FaceClass>();
        _wallProprieties = GetComponent<WallProprieties>();
        _newSoundManagerScript = NewSoundManager.instance;
    }
    // Start is called before the first frame update
    void Start()
    {

        


        // set les valeurs de départs
        _wallLifeMax = _wallManagerScript.GetWallLifeMax();
        wallLife = _wallLifeMax;
        _currentFace = _arenaRotationScript._currentFace;
        //_wallManagerScript.WhichWall(_wallProprieties);
        //wallAppearance = _wallManagerScript.UpdateWallAppearance(_wallProprieties);
        //wallShadowAppearance = _wallManagerScript.UpdateWallShadowAppearance(_wallProprieties);

        //set les valeurs pour screenshake
        _cameraStartPosition = camera.transform.position;
        numberWallState = numberWallStateMax;

        

        _wallCollider = GetComponents<BoxCollider2D>();
        _wallCollider[1].enabled = false;


        _wallMeshRendererOriginalMaterials = transform.GetChild(0).GetComponent<MeshRenderer>().materials;

        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    if (gameObject.transform.GetChild(i).gameObject.activeSelf == true)
        //    {
        //        _currentWallActive = transform.GetChild(i).gameObject;
        //        //si le mur est bouncy, c'est composant à attribuer sont diférents, il y a les pillier et le bambou, au lieu de juste le mur
        //        if(i == 2)
        //        {
        //            //pilliers
        //            _wallShadowMesh = _currentWallActive.transform.GetChild(1).GetChild(0).GetComponent<MeshFilter>();
        //            _wallShadowMeshRenderer = _currentWallActive.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>();
        //            //bambou
        //            _wallBambouAppearance = _currentWallActive.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        //            _wallShadowMeshRendererBambou = _currentWallActive.transform.GetChild(1).GetChild(1).GetComponent<SkinnedMeshRenderer>();

        //            _meshMaterials = _wallShadowMeshRenderer.materials;
        //            _meshMaterialsOriginal = _wallShadowMeshRendererBambou.materials;
        //            _meshMaterialsBambou = _wallShadowMeshRendererBambou.materials;

        //            _bouncyAnimator = _currentWallActive.transform.GetChild(2).GetComponent<Animator>();
        //        }
        //        else
        //        {
        //            _wallShadowMesh = _currentWallActive.transform.GetChild(0).GetComponent<MeshFilter>();
        //            _wallShadowMeshRenderer = _currentWallActive.transform.GetChild(0).GetComponent<MeshRenderer>();
        //            _meshMaterials = _wallShadowMeshRenderer.materials;
        //            _meshMaterialsOriginal = _wallShadowMeshRenderer.materials;
        //        }
        //        break;
        //    }
        //}

        //_wallMesh = _currentWallActive.GetComponent<MeshFilter>();
        //_wallMeshRenderer = _currentWallActive.GetComponent<MeshRenderer>();
        //if (!_wallProprieties.GetIsBouncy())
        //{
        //    _wallMesh.mesh = wallAppearance[0];
        //}
        //else
        //{
        //    _wallBambouAppearance.sharedMesh = wallAppearance[0];
        //}
        //if (!_wallProprieties.GetIsIndestructible())
        //{
        //    if (_wallProprieties.GetIsBouncy())
        //    {
        //        _meshMaterialsBambou[0].color = new Color32(30, 255, 0, 255);
        //        _meshMaterialsOriginal[0].color = new Color32(30, 255, 0, 255);
        //    }
        //    else
        //    {
        //        _meshMaterials[0].color = new Color32(30, 255, 0, 255);
        //        _meshMaterialsOriginal[0].color = new Color32(30, 255, 0, 255);
        //    }
        //}


        //if(gameObject.layer == 15)
        //{
        //    gameObject.SetActive(false);
        //    _wallShadowMeshRenderer.enabled = false;
        //    if(_wallShadowMeshRendererBambou != null)
        //    {
        //        _wallShadowMeshRendererBambou.enabled = false;
        //    }
        //}
        //if(transform.GetChild(3).gameObject.activeSelf)
        //{
        //    _shaderRenderer = transform.GetChild(3).GetComponent<MeshRenderer>();
        //    _shaderRenderer.material.SetFloat("_Etatdudissolve", -1);
        //    _shaderLerpMax = _wallManagerScript.shaderAppearanceTime;
        //    _shaderLerp = -1;
        //}

        shakeWallIntensity = _wallManagerScript.wallShakeIntensity;
        maxShakeWall = _wallManagerScript.wallShakeMax;
        wallShakeTimerMax = _wallManagerScript.wallShakeTimerMax;
        wallInitialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        if (_isWallShaking)
        {
            ShakeWall();
        }
        
        if(_isShaderNeeded)
        {
            if(_shaderLerp <= 1 && !_hasShaderCompletelyAppeared)
            {

                _shaderLerp += Time.deltaTime * _shaderLerpMax;
                if(_shaderLerp >= 1)
                {
                    _shaderLerp = 1;
                    _hasShaderCompletelyAppeared = true;

                }
            }
            else if (_shaderLerp >= -1 && _hasShaderCompletelyAppeared)
            {
                _shaderLerp -= Time.deltaTime * _shaderLerpMax;
                if(_shaderLerp <= -1)
                {
                    _shaderLerp = -1;
                    _hasShaderCompletelyAppeared = false;
                    _isShaderNeeded = false;
                }
            }

            _shaderRenderer.material.SetFloat("_Etatdudissolve", _shaderLerp);

        }

        if(_attackTestOnCollision != null)
        {
            
            lerpTimerRatio += Time.deltaTime;
            _attackTestOnCollision.GetPlayerScoreImage().color = Color32.Lerp(_attackTestOnCollision.GetPlayerScoreImage().color, new Color32(255, 255, 255, 100), lerpTimerRatio);
            if(lerpTimerRatio >=1)
            {
                lerpTimerRatio = 0;
                _attackTestOnCollision = null;
            }

        }

       

        if (lastEjectedPlayer != null && lastEjectedPlayer.GetComponent<AttackTest>().hasPositionBeenSet())
        {
            _gameManagerScript.currentFace = _nextFace - 1;
            _arenaRotationScript._currentFace = _nextFace - 1;
            lastEjectedPlayer = null;
        }
        //si la caméra est en train de changer de face, désactive les sprites ainsi que les colliders des murs, reset la vie des murs et
        //actualise la face actuelle de la caméra
        if (_arenaRotationScript._isTurning)
        {
            _hasCreatedArrayTwo = false;
            _hasCreatedArray = false;
            _wallProprieties.UpdateProprieties();
            _wallManagerScript.WhichWall(_wallProprieties);
            _wallProprieties.IAmConnectedIMustConnect();
            InitiateWall();
            _wallCollider[0].enabled = true;
            _wallCollider[1].enabled = false;
            _wallMeshRenderer.enabled = true;

            _currentFace = _arenaRotationScript._currentFace;
            _lastHit = false;
            wallLife = _wallLifeMax;
            if (!_wallProprieties.GetIsBouncy())
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

            if (!_wallProprieties.GetIsIndestructible())
            {
                if (_wallProprieties.GetIsBouncy())
                {
                    _meshMaterialsBambou[0].color = new Color32(30, 255, 0, 255);
                }
                //réinitialise le tableau de matériaux du mur normal
                else
                {
                    _meshMaterials[0].color = new Color32(30, 255, 0, 255);
                    Material[] temp = new Material[_wallMeshRendererOriginalMaterials.Length];
                    for (int i = 0; i < temp.Length; i++)
                    {
                        temp[i] = _wallMeshRendererOriginalMaterials[i];
                    }
                    _wallMeshRenderer.materials = temp;
                }
            }
            if (gameObject.layer == 15)
            {
                gameObject.SetActive(false);
                _wallShadowMeshRenderer.enabled = false;
                if (_wallShadowMeshRendererBambou != null)
                {
                    _wallShadowMeshRendererBambou.enabled = false;
                }
            }

            if(gameObject.layer == 14)
            {
                gameObject.SetActive(true);
                BoxCollider2D[] boxColliderTab = GetComponents<BoxCollider2D>();
                for (int i = 0; i < boxColliderTab.Length; i++)
                {
                    boxColliderTab[i].enabled = false;
                }
            }
        }


        if (wallLife == _wallLifeMax)
        {
            if(_wallProprieties.GetIsBouncy())
            {            
                if (_hasPlayerCollided)
                    _bouncyAnimator.SetBool("isState4", true);
                else
                    _bouncyAnimator.SetBool("isState4", false);
            }
        }
        if (wallLife <= 0)
        {
            _lastHit = true;
            if (transform.GetChild(3).gameObject.activeSelf)
            {
                transform.GetChild(3).gameObject.SetActive(false);
            }
            if (numberWallState > numberWallStateMax - 4) ShakeScreen();
            _wallMeshRenderer.enabled = false;

            if (!_wallProprieties.GetIsBouncy())
            {
                _wallShadowMeshRenderer.enabled = false;
            }
            else
            {
                _wallBambouAppearance.enabled = false;
                _wallShadowMeshRendererBambou.enabled = false;
            }
            
            _wallCollider[0].enabled = false;
            _wallCollider[1].enabled = true;


        }
        else if (wallLife < _wallLifeMax && wallLife >= (_wallLifeMax * 0.66))
        {
            if (numberWallState > numberWallStateMax - 1) ShakeScreen();
            if (!_wallProprieties.GetIsBouncy())
            {
                _wallMesh.mesh = wallAppearance[1];
                _wallShadowMesh.mesh = wallShadowAppearance[1];
            }
            else
            {
                _wallBambouAppearance.sharedMesh = wallAppearance[1];
                _wallShadowMeshRendererBambou.sharedMesh = wallShadowAppearance[1];
                if(_hasPlayerCollided)
                    _bouncyAnimator.SetBool("isState3", true);
                else
                    _bouncyAnimator.SetBool("isState3", false);
            }
            
        }
        else if (wallLife < (_wallLifeMax * 0.66) && wallLife > (_wallLifeMax * 0.33))
        {
            if (numberWallState > numberWallStateMax - 2) ShakeScreen();

            if (!_wallProprieties.GetIsBouncy())
            {
                _wallMesh.mesh = wallAppearance[2];
                _wallShadowMesh.mesh = wallShadowAppearance[2];
                //on change le nombre de matériaux du mur normal car son mesh a changé
                if (!_wallProprieties.GetIsIndestructible() && !_hasCreatedArray)
                {
                    Material[] temp = new Material[(3)]; /*_wallMeshRenderer.materials.Length - 2*/ //valeur d'avant au cas où bugg
                    for (int i = 0; i < temp.Length; i++)
                    {
                        temp[i] = _wallMeshRenderer.materials[i];
                    }
                    _wallMeshRenderer.materials = temp;
                    _hasCreatedArray = true;
                }
            }
            else
            {
                _wallBambouAppearance.sharedMesh = wallAppearance[2];
                _wallShadowMeshRendererBambou.sharedMesh = wallShadowAppearance[2];
                if (_hasPlayerCollided)
                    _bouncyAnimator.SetBool("isState2", true);
                else
                    _bouncyAnimator.SetBool("isState2", false);
            }
        }
        else if (wallLife < (_wallLifeMax * 0.33) && wallLife > 0)
        {
            if (numberWallState > numberWallStateMax - 3) ShakeScreen();


            if (!_wallProprieties.GetIsBouncy())
            {
                _wallMesh.mesh = wallAppearance[3];
                _wallShadowMesh.mesh = wallShadowAppearance[3];
                //on change le nombre de matériaux du mur normal car son mesh a changé
                if (!_wallProprieties.GetIsIndestructible() && !_hasCreatedArrayTwo)
                {
                    Material[] temp = new Material[(2)];/*_wallMeshRenderer.materials.Length - 1*/ //valeur d'avant au cas où bugg
                    for (int i = 0; i < temp.Length; i++)
                    {
                        temp[i] = _wallMeshRenderer.materials[i];
                    }
                    _wallMeshRenderer.materials = temp;
                    _hasCreatedArrayTwo = true;
                }
            }
            else
            {
                _wallBambouAppearance.sharedMesh = wallAppearance[3];
                _wallShadowMeshRendererBambou.sharedMesh = wallShadowAppearance[3];
                if (_hasPlayerCollided)
                    _bouncyAnimator.SetBool("isState1", true);
                else
                    _bouncyAnimator.SetBool("isState1", false);
            }
        }


    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        _playerOnCollision = collision.gameObject.GetComponent<PlayerEntity>();
        _playerVelocityRatio = _playerOnCollision.GetVelocityRatio();

        _hasPlayerCollided = true;

        //Si le mur n'est pas indestructible et que le joueur ne donne pas d'input (debug du problème ou le joueur charge la puissance en tournant et le mur prend des dégats) alors le mur prend des dégats
        if (!_wallProprieties.GetIsIndestructible() && _playerOnCollision.GetPlayerINPUTSTATE() != PlayerEntity.INPUTSTATE.GivingInput)
        {
            //if (_playerVelocityRatio >= wallLimitVelocity)
            //{
            //    wallLife = 0;
            //}
            //else if (_playerVelocityRatio < wallLimitVelocity)
            //{
                wallLife -= _playerVelocityRatio;
            //}

            _meshMaterials[0].color = Color32.Lerp(new Color32(30, 255, 0, 255), new Color32(236, 25, 25, 255), (_wallLifeMax - wallLife) / _wallLifeMax);
            if (_wallProprieties.GetIsBouncy())
            {
                _meshMaterialsBambou[0].color = Color32.Lerp(new Color32(30, 255, 0, 255), new Color32(236, 25, 25, 255), (_wallLifeMax - wallLife) / _wallLifeMax);
            }
            else
            {
                _isWallShaking = true;
                wallShakeTimer = 0;
            }
        }

        if (_wallProprieties.GetIsIndestructible())
        {
            if (_newSoundManagerScript != null)
                _newSoundManagerScript.PlaySound("IndestructibleWallHit");
        }
        else if (_wallProprieties.GetIsBouncy())
        {
            if(_newSoundManagerScript != null)
            _newSoundManagerScript.PlaySound(0);
        }
        else if (wallLife <= _wallLifeMax && wallLife >= (_wallLifeMax * 0.66))
        {
            if(_newSoundManagerScript != null)
                _newSoundManagerScript.PlaySound("WallLifeHigh");
        }
        else if (wallLife < (_wallLifeMax * 0.66) && wallLife > (_wallLifeMax * 0.33))
        {
            if(_newSoundManagerScript != null)
                _newSoundManagerScript.PlaySound("WallLifeMiddle");
        }
        else if (wallLife < (_wallLifeMax * 0.33) && wallLife > 0)
        {
            if(_newSoundManagerScript != null)
                _newSoundManagerScript.PlaySound("WallLifeLow");
        }
        else if (wallLife <= 0)
        {
            if(_newSoundManagerScript != null)
                _newSoundManagerScript.PlaySound("WallDestroyed");
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _hasPlayerCollided = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerOnCollision = collision.gameObject.GetComponent<PlayerEntity>();
        _attackTestOnCollision = collision.gameObject.GetComponent<AttackTest>();
        Rigidbody2D _playerCollisionRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if (_lastHit)
        {
            _playerCollisionRB.velocity = _playerCollisionRB.velocity.normalized * _wallManagerScript.ejectionPower;
            if (_gameManagerScript.currentPlayersOnArena > 2)
            {
             
                _wallMeshRenderer.enabled = false;
                _wallShadowMeshRenderer.enabled = false;


                _playerOnCollision.enabled = false;
                _playerOnCollision.newRound();
                _playerOnCollision.DesactiveCollider();
                _scoreManagerScript.ChangeScore(_gameManagerScript.currentPlayersOnArena, int.Parse(collision.gameObject.tag.Substring(collision.gameObject.tag.Length - 1)));
                _gameManagerScript.ThisPlayerHasLost(collision.gameObject.tag);
                _gameManagerScript.currentPlayersOnArena--;
            }
            else if (_gameManagerScript.currentPlayersOnArena <= 2)
            {
                Debug.Log("CurrentFace : " + _currentFace);
                Debug.Log("Wall qui change : " + this.gameObject.name);
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
                _wallCollider[0].enabled = false;
               // _wallCollider.isTrigger = false;
                _playerOnCollision.enabled = false;

                if (_gameManagerScript.playerList.Count > 2)
                {
                    if (_gameManagerScript.currentPlayersOnArena == 2 && !_scoreManagerScript.GetSuddenDeath())
                    {
                        _scoreManagerScript.ChangeScore(2, int.Parse(collision.gameObject.tag.Substring(collision.gameObject.tag.Length - 1)));
                        _gameManagerScript.ThisPlayerHasLost(collision.gameObject.tag);
                        _gameManagerScript.currentPlayersOnArena--;
                        _scoreManagerScript.ChangeScore(1, int.Parse(_gameManagerScript.GetFirstCurrentPlayersItem().gameObject.tag.Substring(_gameManagerScript.GetFirstCurrentPlayersItem().gameObject.tag.Length - 1)));
                    }
                    else if (_gameManagerScript.currentPlayersOnArena == 2 && _scoreManagerScript.GetSuddenDeath())
                    {
                        _scoreManagerScript.ChangeScore(2, int.Parse(collision.gameObject.tag.Substring(collision.gameObject.tag.Length - 1)));
                        _gameManagerScript.ThisPlayerHasLost(collision.gameObject.tag);
                        _gameManagerScript.currentPlayersOnArena--;
                        _scoreManagerScript.ChangeScore(1, int.Parse(_gameManagerScript.GetFirstCurrentPlayersItem().gameObject.tag.Substring(_gameManagerScript.GetFirstCurrentPlayersItem().gameObject.tag.Length - 1)));
                    }

                }
                else
                {
                    if (_gameManagerScript.currentPlayersOnArena == 2)
                    {
                        _scoreManagerScript.ChangeScore(4, int.Parse(collision.gameObject.tag.Substring(collision.gameObject.tag.Length - 1)));
                        _gameManagerScript.ThisPlayerHasLost(collision.gameObject.tag);
                        _gameManagerScript.currentPlayersOnArena--;
                        _scoreManagerScript.ChangeScore(1, int.Parse(_gameManagerScript.GetFirstCurrentPlayersItem().gameObject.tag.Substring(_gameManagerScript.GetFirstCurrentPlayersItem().gameObject.tag.Length - 1)));
                    }
                }

                _gameManagerScript.ResetCurrentPlayers();
                lastEjectedPlayer = collision.gameObject;
                

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
        //_meshMaterials[0].color = Color32.Lerp(_meshMaterialsOriginal[0].color, new Color32(236, 25, 25, 255), (wallLifeMax - wallLife) / 3);
        //if (_wallProprieties.isBouncy)
        //{
        //    _meshMaterialsBambou[0].color = Color32.Lerp(_meshMaterialsOriginal[0].color, new Color32(236, 25, 25, 255), (wallLifeMax - wallLife) / 3);
        //}
        //if (dammage >= wallLimitVelocity)
        //{
        //    wallLife = 0;
        //}
        //else if (dammage < wallLimitVelocity)
        //{
            wallLife -= dammage;
        //}
        _meshMaterials[0].color = Color32.Lerp(new Color32(30, 255, 0, 255), new Color32(236, 25, 25, 255), (_wallLifeMax - wallLife) / 3);
        if (_wallProprieties.GetIsBouncy())
        {
            _meshMaterialsBambou[0].color = Color32.Lerp(new Color32(30, 255, 0, 255), new Color32(236, 25, 25, 255), (_wallLifeMax - wallLife) / 3);
        }
        if (!_wallProprieties.GetIsIndestructible() && !_wallProprieties.GetIsBouncy())
        {
            _isWallShaking = true;
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

    public void ReEnablingWallBoxColliders()
    {

        BoxCollider2D[] boxColliderTab = GetComponents<BoxCollider2D>();
        for (int i = 0; i < boxColliderTab.Length; i++)
        {
            boxColliderTab[i].enabled = true;
        }

    }

    public void SetShaderNeededTrue()
    {
        _isShaderNeeded = true;
    }

    private void ShakeWall()
    {
        transform.localPosition = new Vector3(Mathf.PingPong(Time.time * shakeWallIntensity, maxShakeWall * 2) + transform.localPosition.x - maxShakeWall, transform.localPosition.y, transform.localPosition.z);
        wallShakeTimer += Time.deltaTime;
        if (wallShakeTimer >= wallShakeTimerMax)
        {
            transform.localPosition = wallInitialPosition;
            wallShakeTimer = 0;
            _isWallShaking = false;
        }
    }

    public void InitiateWall()
    {
        _wallManagerScript.WhichWall(_wallProprieties);
        wallAppearance = _wallManagerScript.UpdateWallAppearance(_wallProprieties);
        wallShadowAppearance = _wallManagerScript.UpdateWallShadowAppearance(_wallProprieties);


        for (int i = 0; i < transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.activeSelf == true)
            {
                _currentWallActive = transform.GetChild(i).gameObject;
                //si le mur est bouncy, c'est composant à attribuer sont diférents, il y a les pillier et le bambou, au lieu de juste le mur
                if (i == 2)
                {
                    //pilliers
                    _wallShadowMesh = _currentWallActive.transform.GetChild(1).GetChild(0).GetComponent<MeshFilter>();
                    _wallShadowMeshRenderer = _currentWallActive.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>();
                    //bambou
                    _wallBambouAppearance = _currentWallActive.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
                    _wallShadowMeshRendererBambou = _currentWallActive.transform.GetChild(1).GetChild(1).GetComponent<SkinnedMeshRenderer>();

                    _meshMaterials = _wallShadowMeshRenderer.materials;
                    _meshMaterialsOriginal = _wallShadowMeshRendererBambou.materials;
                    _meshMaterialsBambou = _wallShadowMeshRendererBambou.materials;

                    _bouncyAnimator = _currentWallActive.transform.GetChild(2).GetComponent<Animator>();
                }
                else
                {
                    _wallShadowMesh = _currentWallActive.transform.GetChild(0).GetComponent<MeshFilter>();
                    _wallShadowMeshRenderer = _currentWallActive.transform.GetChild(0).GetComponent<MeshRenderer>();
                    _meshMaterials = _wallShadowMeshRenderer.materials;
                    _meshMaterialsOriginal = _wallShadowMeshRenderer.materials;
                }
                break;
            }
        }

        _wallMesh = _currentWallActive.GetComponent<MeshFilter>();
        _wallMeshRenderer = _currentWallActive.GetComponent<MeshRenderer>();
        if (!_wallProprieties.GetIsBouncy())
        {
            _wallMesh.mesh = wallAppearance[0];
        }
        else
        {
            _wallBambouAppearance.sharedMesh = wallAppearance[0];
        }
        if (!_wallProprieties.GetIsIndestructible())
        {
            if (_wallProprieties.GetIsBouncy())
            {
                _meshMaterialsBambou[0].color = new Color32(30, 255, 0, 255);
                _meshMaterialsOriginal[0].color = new Color32(30, 255, 0, 255);
            }
            else
            {
                _meshMaterials[0].color = new Color32(30, 255, 0, 255);
                _meshMaterialsOriginal[0].color = new Color32(30, 255, 0, 255);
            }
        }


        if (gameObject.layer == 15)
        {
            gameObject.SetActive(false);
            _wallShadowMeshRenderer.enabled = false;
            if (_wallShadowMeshRendererBambou != null)
            {
                _wallShadowMeshRendererBambou.enabled = false;
            }
        }
        if (transform.GetChild(3).gameObject.activeSelf)
        {
            _shaderRenderer = transform.GetChild(3).GetComponent<MeshRenderer>();
            _shaderRenderer.material.SetFloat("_Etatdudissolve", -1);
            _shaderLerpMax = _wallManagerScript.shaderAppearanceTime;
            _shaderLerp = -1;
        }
    }
}
