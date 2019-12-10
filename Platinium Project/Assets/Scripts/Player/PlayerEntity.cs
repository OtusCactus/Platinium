using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PlayerEntity : MonoBehaviour
{
    //Ce script sert aux déplacements des joueurs
    //Pour l'instant on utilise la physique d'Unity pour le proto, mais on changera pour une physique personnalisée pour la semaine prochaine

    //
    [Header("Speed")]
    public float speed;

    //
    private Rigidbody2D _myRb;

    //Variables pour input joystick
    private Vector2 _input;
    private Vector2 _inputVariableToStoreDirection;
    private float _timerDeadPoint = 0;
    private float _joyAngle;
    private float _angle;
    [HideInInspector] public int _controllerNumber;
    private float inputXSign;
    private float inputYSign;




    [Header("Power")]
    public float powerMax;
    public Image powerJauge;
    public GameObject powerJaugeParent;
    private float _timerPower = 0;

    //si on maintiens trop longtemps le tir, le relache au bout d'un certain temps
    [Header("TooMuchPower")]
    public float tooMuchPowerTimerMax;
    private float tooMuchPowerTimer;
    private bool _isTooMuchPowerGathered;
    public GameObject sweatParticles;

    //Variables pour la vitesse
    private float _myVelocityFloat;
    private float _velocityMax;
    private float _velocityConvertedToRatio;
    private Vector3 _lastFrameVelocity;
    private float _lastFramePower;

    [Header("Frictions")]
    public float friction = 0.1f;
    public float frictionWhenCharging = 0.1f;
    public float frictionPlayer = 15f;
    public float reboundPourcentageOfSpeedIfImFaster = 25;
    public float reboundPourcentageOfSpeedIfImSlower = 75;

    [Header("Vibration")]
    private float vibrationTreshold = 0.2f;

    [Header("Animation")]
    private Animator _animator;

    //sound
    
    private bool _mustPlayCastSound = false;
    private SoundManager _soundManagerScript;
    private PlayerManager _playerManagerScript;
    [SerializeField]
    [Header("Sound")]
    private AudioSource[] _playerAudio;




    [Header("Onomatopées")]
    public Sprite[] onomatopeesTab;
    public SpriteRenderer onomatopéesSprite;
    private float onomatopéeTimer;
    public float onomatopéeTimerMax;

    [Header("WallSprite")]
    private Transform wallSpriteTransform;
    private float wallHitSpriteTimer;
    private float wallHitSpriteTimerMax;
    private Vector3 wallSpritePosition;

    [Header("UltiCharge")]
    public int ultiChargeMax;
    public float ultiChargeRatio;
    private float _ultiCurrentCharge;
    private bool _isUltiPossible;
    public GameObject[] UltiFxStates;
    [Header("Trail")]
    public float trailDuration = 2;
    public float trailApparitionTreshold = 0.8f;
    private TrailRenderer _playerTrail;
    private bool _needTrail = false;
    private float _trailTimer = 0;


    //score
    private ScoreManager _scoreManagerScript;
    private NewSoundManager _newSoundManagerScript;

    private AudioSource _childAudioSource;


    //playerScaling on wallhit
    private bool playerScaleHitWall = false;
    private float timerScale = 0;
    private float timerScaleMax = 0.08f;
    private float timerRescale = 0;
    private float timerRescaleMax = 0.08f;
    private GameObject playerSprite;


    //Enum pour état du joystick -> donne un input, est à 0 mais toujours en input, input relaché et fin d'input
    public enum INPUTSTATE { GivingInput, EasingInput, Released, None };
    private INPUTSTATE _playerInput = INPUTSTATE.Released;

    private bool _touchedByPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        _myRb = GetComponent<Rigidbody2D>();

        powerJauge.fillAmount = 0;
        powerJaugeParent.gameObject.SetActive(false);

        _velocityMax = (powerMax * speed) * (powerMax * speed);

        _playerTrail = GetComponent<TrailRenderer>();

        _soundManagerScript = SoundManager.instance;

        _newSoundManagerScript = NewSoundManager.instance;
        _playerAudio = GetComponents<AudioSource>();

        _scoreManagerScript = GameObject.FindWithTag("GameController").GetComponent<ScoreManager>();

        _playerManagerScript = GameObject.FindWithTag("GameController").GetComponent<PlayerManager>();

        _animator = GetComponent<Animator>();

        //_childAudioSource = transform.GetChild(0).GetComponent<AudioSource>();


        onomatopéesSprite.enabled = false;
        sweatParticles.SetActive(false);
        UltiFxStates[0].SetActive(false);
        UltiFxStates[1].SetActive(false);
        UltiFxStates[2].SetActive(false);
        wallHitSpriteTimerMax = onomatopéeTimerMax;

        string thisTag = gameObject.tag;
        switch(thisTag)
        {
            case "Player1":
                wallSpriteTransform = GameObject.FindWithTag("WallHitSprite1").transform;
                break;
            case "Player2":
                wallSpriteTransform = GameObject.FindWithTag("WallHitSprite2").transform;
                break;
            case "Player3":
                wallSpriteTransform = GameObject.FindWithTag("WallHitSprite3").transform;
                break;
            case "Player4":
                wallSpriteTransform = GameObject.FindWithTag("WallHitSprite4").transform;
                break;
        }
        wallSpriteTransform.gameObject.SetActive(false);

        playerSprite = transform.GetChild(0).gameObject;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
         if (_myRb.velocity != Vector2.zero)
        {
            Vector2 frictionDir = _myRb.velocity.normalized;
            float frictionToApply = friction * Time.fixedDeltaTime;
            if (_myRb.velocity.sqrMagnitude <= frictionToApply * frictionToApply)
            {
                _myRb.velocity = Vector2.zero;
            }
            else
            {
                _myRb.velocity -= frictionToApply * frictionDir;
            }
        }

        //Dicte quand on passe d'un enum à l'autre
        #region Change Enum
        if (_input != Vector2.zero)
        {
            _playerInput = INPUTSTATE.GivingInput;

            _timerDeadPoint = 0;
            inputXSign = _inputVariableToStoreDirection.x;
            inputYSign = _inputVariableToStoreDirection.y;
        }
        else if (_playerInput == INPUTSTATE.GivingInput && (_input.x == 0 || _input.y == 0) && _timerDeadPoint < 0.1)
        {

            _playerInput = INPUTSTATE.Released;

        }
        else if((_playerInput == INPUTSTATE.EasingInput && _timerDeadPoint >= 0.1))
        {

            _playerInput = INPUTSTATE.Released;
        }
        #endregion

        #region Actions depending on INPUTSTATE
        if (_playerInput == INPUTSTATE.GivingInput)
        {
            _animator.SetBool("IsSlingshoting", true);
            _animator.SetBool("isHit", false);
            _animator.SetBool("isHitting", false);
            _angle = Mathf.Atan2(_input.x, _input.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, _angle);


            powerJaugeParent.gameObject.SetActive(true);
            powerJauge.fillAmount = _timerPower / powerMax;
            _inputVariableToStoreDirection = _input;
            Vector2 frictionDir = _myRb.velocity.normalized;
            if (_myRb.velocity.sqrMagnitude >= (frictionWhenCharging * Time.fixedDeltaTime) * (frictionWhenCharging * Time.fixedDeltaTime))
            {
                _myRb.velocity -= (frictionWhenCharging * Time.fixedDeltaTime) * frictionDir;
            }
            else
            {
                _myRb.velocity = Vector2.zero;
            }
            _timerPower += Time.fixedDeltaTime;

            //check si ça fait pas de probs plus tard
            if (_timerPower >= powerMax)
            {
                _timerPower = powerMax;
                tooMuchPowerTimer += Time.fixedDeltaTime;
                sweatParticles.SetActive(true);
                if (tooMuchPowerTimer > tooMuchPowerTimerMax)
                {
                    tooMuchPowerTimer = 0;
                    _isTooMuchPowerGathered = true;
                    powerJaugeParent.gameObject.SetActive(false);
                    sweatParticles.SetActive(false);

                    _myRb.velocity = new Vector2(_inputVariableToStoreDirection.x, -_inputVariableToStoreDirection.y).normalized * (-_timerPower * speed);

                    _inputVariableToStoreDirection = Vector2.zero;
                    _lastFramePower = _timerPower;
                    _timerPower = 0;
                    _timerDeadPoint = 0;
                    vibrationTreshold = 0.2f;
                    powerJauge.fillAmount = 0;

                    if (gameObject.tag == "Player1")
                    {

                        _playerManagerScript.player[0].StopVibration();
                    }
                    else if (gameObject.tag == "Player2")
                    {
                        _playerManagerScript.player[1].StopVibration();
                    }
                    else if (gameObject.tag == "Player3")
                    {
                        _playerManagerScript.player[2].StopVibration();
                    }
                    else if (gameObject.tag == "Player4")
                    {
                        _playerManagerScript.player[3].StopVibration();
                    }
                    _animator.SetBool("IsSlingshoting", false);
                    //_soundManagerScript.NoSound();
                    _playerInput = INPUTSTATE.None;
                }
            }
        }
        else if (_playerInput == INPUTSTATE.EasingInput)
        {
                _timerDeadPoint += Time.fixedDeltaTime;

        }
        else if(_playerInput == INPUTSTATE.Released)
        {
            if (_touchedByPlayer)
            {
                _touchedByPlayer = false;
            }

            sweatParticles.SetActive(false);
            _animator.SetBool("IsSlingshoting", false);
            powerJaugeParent.gameObject.SetActive(false);
            //_myRb.velocity = new Vector2 (_inputVariableToStoreDirection.x, -_inputVariableToStoreDirection.y).normalized * (-_timerPower * speed);
            _myRb.velocity = new Vector2 (inputXSign, -inputYSign).normalized * (-_timerPower * speed);

            _inputVariableToStoreDirection = Vector2.zero;
            _lastFramePower = _timerPower;
            _timerPower = 0;
            _timerDeadPoint = 0;
            
            _playerInput = INPUTSTATE.None;
        }
        else if (_playerInput == INPUTSTATE.None)
        {

            vibrationTreshold = 0.2f;
            powerJauge.fillAmount = 0;
            if (gameObject.tag == "Player1")
            {
                _playerManagerScript.player[0].StopVibration();
            }
            else if (gameObject.tag == "Player2")
            {
                _playerManagerScript.player[1].StopVibration();
            }
            else if (gameObject.tag == "Player3")
            {
                _playerManagerScript.player[2].StopVibration();
            }
            else if (gameObject.tag == "Player4")
            {
                _playerManagerScript.player[3].StopVibration();
            }
        }
        #endregion

        //Fait apparaitre une trail si la vitesse atteind le seuil des murs 
        _myVelocityFloat = _myRb.velocity.sqrMagnitude;
        _velocityConvertedToRatio = (_myVelocityFloat / _velocityMax);
        if (_velocityConvertedToRatio > trailApparitionTreshold)
        {
            _playerTrail.enabled = true;
            _needTrail = true;
        }
        if (_needTrail)
        {
            _trailTimer += Time.deltaTime;
        }
        if (_trailTimer >= trailDuration)
        {
            _needTrail = false;
        }
        if (!_needTrail)
        {
            _playerTrail.enabled = false;
            _trailTimer = 0;
        }

        _lastFrameVelocity = _myRb.velocity;

    }


    private void Update()
    {
        if (playerScaleHitWall)
        {
            timerScale += Time.deltaTime;
            float lerpScaleRatio = timerScale / timerScaleMax;
            playerSprite.transform.localScale = Vector3.Lerp(playerSprite.transform.localScale, new Vector3(0.17f, 0.17f, playerSprite.transform.localScale.z), lerpScaleRatio);
            if (lerpScaleRatio >= 1)
            {
                timerRescale += Time.deltaTime;
                float lerpRescaleRatio = timerRescale / timerRescaleMax;
                playerSprite.transform.localScale = Vector3.Lerp(playerSprite.transform.localScale, new Vector3(0.2f, 0.2f, 0.2f), lerpRescaleRatio);
                if (lerpRescaleRatio >= 1)
                {
                    timerScale = 0;
                    timerRescale = 0;
                    playerScaleHitWall = false;
                }
            }
        }

        //si les onomatopées sont activés, lance le timer de désactivation
        if (onomatopéesSprite.enabled)
        {
            onomatopéeTimer += Time.deltaTime;
            if(onomatopéeTimer >= onomatopéeTimerMax)
            {
                onomatopéeTimer = 0;
                _animator.SetBool("isHit", false);
                _animator.SetBool("isHitting", false);
                onomatopéesSprite.enabled = false;
            }
        }

        //si la jauge d'ulti est inférieure à 1/3 du max, ne fait pas apparaitre les FXs
        if(_ultiCurrentCharge < ultiChargeMax * 0.33f)
        {
            UltiFxStates[0].SetActive(false);
            UltiFxStates[1].SetActive(false);
            UltiFxStates[2].SetActive(false);
        }

        //permet de changer la position du sprite qui apparait quand on touche le mur et de lui lancer son timer de désactivation
        if (wallSpriteTransform.gameObject.activeSelf)
        {
            wallSpriteTransform.position = wallSpritePosition;
            wallSpriteTransform.localPosition = new Vector3(wallSpriteTransform.localPosition.x, wallSpriteTransform.localPosition.y, 8);
            wallHitSpriteTimer += Time.deltaTime;
            if(wallHitSpriteTimer >= wallHitSpriteTimerMax)
            {
                wallHitSpriteTimer = 0;
                wallSpriteTransform.gameObject.SetActive(false);

            }
        }

        //si on a utilisé l'ulti, reset la jauge.
        if(_ultiCurrentCharge == ultiChargeMax && !_isUltiPossible)
        {
            _ultiCurrentCharge = 0;
        }

        //active et désactive le son de charge.
        if(_playerInput == INPUTSTATE.GivingInput && _mustPlayCastSound)
        {
            if(_newSoundManagerScript != null)
            _newSoundManagerScript.PlayCharge(int.Parse(gameObject.tag.Substring(gameObject.tag.Length - 1)) -1);
            _mustPlayCastSound = false;
        }
        else if (_playerInput == INPUTSTATE.None)
        {
            if(_newSoundManagerScript != null)
            _newSoundManagerScript.StopCharge(int.Parse(gameObject.tag.Substring(gameObject.tag.Length - 1)) -1);
            _mustPlayCastSound = true;
        }

        //active les différents stades de vibrations
        if (powerJauge.fillAmount > vibrationTreshold)
        {
            if (gameObject.tag == "Player1")
            {
                _playerManagerScript.Vibration(_playerManagerScript.player[0], 0, 1.0f, vibrationTreshold * 0.5f);
            }
            else if (gameObject.tag == "Player2")
            {
                _playerManagerScript.Vibration(_playerManagerScript.player[1], 0, 1.0f, vibrationTreshold * 0.5f);
            }
            if (gameObject.tag == "Player3")
            {
                _playerManagerScript.Vibration(_playerManagerScript.player[2], 0, 1.0f, vibrationTreshold * 0.5f);
            }
            else if (gameObject.tag == "Player4")
            {
                _playerManagerScript.Vibration(_playerManagerScript.player[3], 0, 1.0f, vibrationTreshold * 0.5f);
            }
            vibrationTreshold += 0.2f;
        }
        else if (powerJauge.fillAmount == vibrationTreshold)
        {
            if (gameObject.tag == "Player1")
            {
                _playerManagerScript.Vibration(_playerManagerScript.player[0], 0, 1.0f, tooMuchPowerTimerMax);
            }
            else if (gameObject.tag == "Player2")
            {
                _playerManagerScript.Vibration(_playerManagerScript.player[1], 0, 1.0f, tooMuchPowerTimerMax);
            }
            if (gameObject.tag == "Player3")
            {
                _playerManagerScript.Vibration(_playerManagerScript.player[2], 0, 1.0f, tooMuchPowerTimerMax);
            }
            else if (gameObject.tag == "Player4")
            {
                _playerManagerScript.Vibration(_playerManagerScript.player[3], 0, 1.0f, tooMuchPowerTimerMax);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //si on touche un mur on un joueur, joue un son différent
        if (collision.gameObject.tag.Contains("Walls"))
        {
            WallProprieties collisionScript = collision.gameObject.GetComponent<WallProprieties>();
            onomatopéeTimer = 0;

            wallHitSpriteTimer = 0;
            wallSpritePosition = new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y, 8);
            wallSpriteTransform.position = wallSpritePosition;
            wallSpriteTransform.gameObject.SetActive(true);
            playerScaleHitWall = true;

        }
        //son collision avec joueurs
        else if (collision.gameObject.tag.Contains("Player"))
        {
            if(_newSoundManagerScript != null)
            _newSoundManagerScript.PlaySound(1);
            wallSpriteTransform.gameObject.SetActive(false);
            
            onomatopéesSprite.enabled = true;
            onomatopéesSprite.sprite = onomatopeesTab[Random.Range(0, onomatopeesTab.Length - 1)];
            onomatopéeTimer = 0;
            _touchedByPlayer = true;
            PlayerEntity otherPlayer = collision.gameObject.GetComponent<PlayerEntity>();
            //charge la jauge d'ulti et active les Fxs correspondant à l'état de la jauge
            if (_lastFrameVelocity.magnitude > otherPlayer._lastFrameVelocity.magnitude)
            {
                _animator.SetBool("isHitting", true);
                _ultiCurrentCharge += ultiChargeRatio * _lastFrameVelocity.magnitude;

                if ( _ultiCurrentCharge > ultiChargeMax/3 && _ultiCurrentCharge <ultiChargeMax * 0.66f)
                {
                    UltiFxStates[0].SetActive(true);
                    UltiFxStates[1].SetActive(false);
                    UltiFxStates[2].SetActive(false);
                }
                else if (_ultiCurrentCharge >= ultiChargeMax * 0.66f && _ultiCurrentCharge < ultiChargeMax)
                {
                    UltiFxStates[0].SetActive(false);
                    UltiFxStates[1].SetActive(true);
                    UltiFxStates[2].SetActive(false);
                }
                else if (_ultiCurrentCharge >= ultiChargeMax)
                {
                    UltiFxStates[0].SetActive(false);
                    UltiFxStates[1].SetActive(false);
                    UltiFxStates[2].SetActive(true);
                }

                if (_ultiCurrentCharge >= ultiChargeMax)
                {
                    _ultiCurrentCharge = ultiChargeMax;
                    _isUltiPossible = true;
                }
                //rebonds
                if(otherPlayer._lastFrameVelocity.magnitude <= new Vector3(0.2f, 0.2f, 0.2f).magnitude)
                {
                    Rebound((-_lastFrameVelocity * reboundPourcentageOfSpeedIfImFaster)/100, collision.GetContact(0).normal, frictionPlayer);
                }
                else
                {
                    Rebound((otherPlayer.GetLastFrameVelocity() * reboundPourcentageOfSpeedIfImFaster) / 100, collision.GetContact(0).normal, frictionPlayer);
                }
                otherPlayer.Rebound((_lastFrameVelocity * otherPlayer.reboundPourcentageOfSpeedIfImSlower) / 100, collision.GetContact(0).normal, otherPlayer.frictionPlayer);
            }
            else
            {
                _animator.SetBool("isHit", true);

                if (_lastFrameVelocity.magnitude <= new Vector3(0.2f, 0.2f, 0.2f).magnitude)
                {
                    otherPlayer.Rebound((-otherPlayer.GetLastFrameVelocity() * otherPlayer.reboundPourcentageOfSpeedIfImFaster) / 100, collision.GetContact(0).normal, otherPlayer.frictionPlayer);
                }
                else
                {
                    otherPlayer.Rebound((_lastFrameVelocity * otherPlayer.reboundPourcentageOfSpeedIfImFaster) / 100, collision.GetContact(0).normal, otherPlayer.frictionPlayer);
                }
                Rebound((otherPlayer.GetLastFrameVelocity() * reboundPourcentageOfSpeedIfImSlower) / 100, collision.GetContact(0).normal, frictionPlayer);
            }
        }

    }

    //fonciton reset des variables quand new round
    public void newRound()
    {
        GetComponent<AttackTest>().SetHasPositionFalse();
        _animator.SetBool("IsSlingshoting", false);
        _ultiCurrentCharge = 0;
        UltiFxStates[0].SetActive(false);
        UltiFxStates[1].SetActive(false);
        UltiFxStates[2].SetActive(false);
        //_soundManagerScript.NoSound(_playerAudio[0]);
        if (_newSoundManagerScript != null)
        _newSoundManagerScript.StopCharge(int.Parse(gameObject.tag.Substring(gameObject.tag.Length - 1)) - 1);
        _mustPlayCastSound = true;
        onomatopéesSprite.enabled = false;
        wallSpriteTransform.gameObject.SetActive(false);
        sweatParticles.SetActive(false);
        if (gameObject.tag == "Player1")
        {
            _playerManagerScript.StopVibration(_playerManagerScript.player[0]);
        }
        else if (gameObject.tag == "Player2")
        {
            _playerManagerScript.StopVibration(_playerManagerScript.player[1]);
        }
        if (gameObject.tag == "Player3")
        {
            _playerManagerScript.StopVibration(_playerManagerScript.player[2]);
        }
        else if (gameObject.tag == "Player4")
        {
            _playerManagerScript.StopVibration(_playerManagerScript.player[3]);
        }
    }

    private void Rebound(Vector3 reboundVelocity, Vector3 collisionNormal, float friction)
    {
        Vector3 direction = Vector3.Reflect(-reboundVelocity.normalized, collisionNormal);
        _myRb.velocity = new Vector3(direction.x , direction.y).normalized * ((reboundVelocity.magnitude / friction) * speed);
    }

    public void SetInputX(Vector2 myInput)
    {
        _input = myInput;
    }

    public float GetVelocityRatio()
    {
        return _velocityConvertedToRatio;
    }

    public Vector3 GetLastFrameVelocity()
    {
        return _lastFrameVelocity;
    }

    public INPUTSTATE GetPlayerINPUTSTATE()
    {
        return _playerInput;
    }

    public void DesactiveCollider()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void ReactiveCollider()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public bool GetUltiBool()
    {
        return _isUltiPossible;
    }

    public void SetUltiBoolFalse()
    {
        _isUltiPossible = false;
    }

    public AudioSource[] GetAudioSourceTab()
    {
        return _playerAudio;
    }

    public void resetUltiCurrentCharge()
    {
        _ultiCurrentCharge = 0;
    }

}
