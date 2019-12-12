using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class InMenuPlayer : MonoBehaviour
{
    public int currentFace;
    public bool isTurning;
    public Quaternion arenaRotation;
    public GameObject arena;



    //Ce script sert aux déplacements des joueurs

    //
    [Header("Speed")]
    public float speed;
    private TrailRenderer _trailRenderer;

    //
    private Rigidbody2D _myRb;

    //Variables pour input joystick
    private Vector2 _input;
    private Vector2 _inputVariableToStoreDirection;
    private float _timerDeadPoint = 0;
    private float _joyAngle;
    private float _angle;
    [HideInInspector] public int _controllerNumber;




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

    //Variables pour la vitesse
    private float _myVelocity;
    private float _velocityMax;
    private float _velocityConvertedToRatio;
    private Vector3 _lastFrameVelocity;
    private float _lastFramePower;

    [Header("Frictions")]
    public float friction = 0.1f;
    public float frictionPlayer = 15f;

    [Header("Vibration")]
    private float vibrationTreshold = 0.2f;

    [Header("Animation")]
    private Animator _animator;

    //bool sound
    private bool _mustPlayCastSound = false;
    private AudioSource _audioSource;

    private GetMenuInformation _menuInformationScript;
    private NewSoundManager _newSoundManagerScript;

    //particules
    private GameObject _particuleContact;
    
    private MenuPlayerManager _playerManagerScript;

    //Enum pour état du joystick -> donne un input, est à 0 mais toujours en input, input relaché et fin d'input
    private enum INPUTSTATE { GivingInput, EasingInput, Released, None };
    private INPUTSTATE _playerInput = INPUTSTATE.Released;

    private bool _isOptionOrPlayOpen;

    private void Awake()
    {
        _playerManagerScript = GameObject.FindWithTag("GameController").GetComponent<MenuPlayerManager>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _myRb = GetComponent<Rigidbody2D>();
        _particuleContact = this.transform.GetChild(1).gameObject;
        _newSoundManagerScript = NewSoundManager.instance;

    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindWithTag("MenuManager") != null)
        {

            _menuInformationScript = GameObject.FindWithTag("MenuManager").GetComponent<GetMenuInformation>();
        }

        currentFace = 0;
        powerJauge.fillAmount = 0;
        powerJaugeParent.gameObject.SetActive(false);
        _velocityMax = (powerMax * speed) * (powerMax * speed);


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
        }
        else if (_playerInput == INPUTSTATE.GivingInput && (_input.x == 0 || _input.y == 0) && _timerDeadPoint < 0.1)
        {
            _playerInput = INPUTSTATE.EasingInput;
        }
        else if ((_playerInput == INPUTSTATE.EasingInput && _timerDeadPoint >= 0.1))
        {
            _playerInput = INPUTSTATE.Released;
        }
        #endregion

        #region Actions depending on INPUTSTATE
        if (_playerInput == INPUTSTATE.GivingInput)
        {
            _animator.SetBool("IsSlingshoting", true);
            _angle = Mathf.Atan2(_input.x, _input.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, _angle);


            powerJaugeParent.gameObject.SetActive(true);
            powerJauge.fillAmount = _timerPower / powerMax;
            _inputVariableToStoreDirection = _input;
            _timerPower += Time.fixedDeltaTime;

            //check si ça fait pas de probs plus tard
            if (_timerPower >= powerMax)
            {
                _timerPower = powerMax;
                tooMuchPowerTimer += Time.fixedDeltaTime;
                if (tooMuchPowerTimer > tooMuchPowerTimerMax)
                {
                    tooMuchPowerTimer = 0;
                    _isTooMuchPowerGathered = true;
                    powerJaugeParent.gameObject.SetActive(false);
                    _myRb.velocity = new Vector2(_inputVariableToStoreDirection.x, -_inputVariableToStoreDirection.y).normalized * (-_timerPower * speed);

                    _inputVariableToStoreDirection = Vector2.zero;
                    _lastFramePower = _timerPower;
                    _timerPower = 0;
                    _timerDeadPoint = 0;
                    vibrationTreshold = 0.2f;
                    powerJauge.fillAmount = 0;

                    if (gameObject.tag == "Player1")
                    {

                        _playerManagerScript._player.StopVibration();
                    }
                    _playerInput = INPUTSTATE.None;
                }
            }
        }
        else if (_playerInput == INPUTSTATE.EasingInput)
        {
            _timerDeadPoint += Time.fixedDeltaTime;

        }
        else if (_playerInput == INPUTSTATE.Released)
        {
            _animator.SetBool("IsSlingshoting", false);
            powerJaugeParent.gameObject.SetActive(false);
            _myRb.velocity = new Vector2(_inputVariableToStoreDirection.x, -_inputVariableToStoreDirection.y).normalized * (-_timerPower * speed);

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
                _playerManagerScript._player.StopVibration();
            }
        }
        #endregion

        //Fait apparaitre une trail si la vitesse atteind le seuil des murs (on changera après le 0.8 par une variable)
        _myVelocity = _myRb.velocity.sqrMagnitude;
        _velocityConvertedToRatio = (_myVelocity / _velocityMax);
        if (_velocityConvertedToRatio > 0.8)
        {
            _trailRenderer.enabled = true;
        }
        else
        {
            _trailRenderer.enabled = false;
        }
        _lastFrameVelocity = _myRb.velocity;

    }


    private void Update()
    {

        Debug.Log(_isOptionOrPlayOpen);

        if (_isOptionOrPlayOpen)
        {
            _animator.SetBool("IsSlingshoting", false);
            _myRb.velocity = Vector2.zero;
            _timerPower = 0;

        }
        

        if (_playerInput == INPUTSTATE.GivingInput && _mustPlayCastSound)
        {
            _newSoundManagerScript.PlayCharge(int.Parse(gameObject.tag.Substring(gameObject.tag.Length - 1)) - 1);
            _mustPlayCastSound = false;
        }
        else if (_playerInput == INPUTSTATE.None)
        {
            _newSoundManagerScript.StopCharge(int.Parse(gameObject.tag.Substring(gameObject.tag.Length - 1)) - 1);
            _mustPlayCastSound = true;
        }

        if (_menuInformationScript == null || _menuInformationScript.GetVibrationsValue())
        {
            if (powerJauge.fillAmount > vibrationTreshold)
            {
                if (gameObject.tag == "Player1")
                {
                    _playerManagerScript.Vibration(_playerManagerScript._player, 0, 1.0f, vibrationTreshold * 0.5f);
                }
                vibrationTreshold += 0.2f;
            }
            else if (powerJauge.fillAmount == vibrationTreshold)
            {
                if (gameObject.tag == "Player1")
                {
                    _playerManagerScript.Vibration(_playerManagerScript._player, 0, 1.0f, tooMuchPowerTimerMax);
                }
            }
        }

        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        arenaRotation = arena.transform.rotation;
        if (currentFace == 0)
        {
            if (collision.name == "WallNorthEast")
            {
                currentFace = 1;
                isTurning = true;
            }
            else if (collision.name == "WallNorthWest")
            {
                currentFace = 2;
                isTurning = true;
            }

        }
        else if (currentFace == 1)
        {
            if (collision.name == "WallSouth")
            {
                currentFace = 0;
                isTurning = true;
            }
            else if (collision.name == "WallSouthWest")
            {
                currentFace = 2;
                isTurning = true;
            }
        }
        else if (currentFace == 2)
        {
            if (collision.name == "WallSouth")
            {
                currentFace = 0;
                isTurning = true;
            }
            else if (collision.name == "WallSouthEast")
            {
                currentFace = 1;
                isTurning = true;
            }
        }



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

    public void IsInOptionOrCharacterMenu(bool boolValue)
    {
        _isOptionOrPlayOpen = boolValue;
    }
}
