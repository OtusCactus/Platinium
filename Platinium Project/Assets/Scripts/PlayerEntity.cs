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
    

    //particules
    private GameObject _particuleContact;

    private SoundManager _soundManagerScript;
    private PlayerManager _playerManagerScript;

    //Enum pour état du joystick -> donne un input, est à 0 mais toujours en input, input relaché et fin d'input
    private enum INPUTSTATE { GivingInput, EasingInput, Released, None };
    private INPUTSTATE _playerInput = INPUTSTATE.Released;

    // Start is called before the first frame update
    void Start()
    {
        _myRb = GetComponent<Rigidbody2D>();
        powerJauge.fillAmount = 0;
        powerJaugeParent.gameObject.SetActive(false);
        _velocityMax = (powerMax * speed) * (powerMax * speed);
        _particuleContact = this.transform.GetChild(1).gameObject;

        _soundManagerScript = GameObject.FindWithTag("GameController").GetComponent<SoundManager>();
        _playerManagerScript = GameObject.FindWithTag("GameController").GetComponent<PlayerManager>();
        _animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {

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
        else if((_playerInput == INPUTSTATE.EasingInput && _timerDeadPoint >= 0.1))
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
            //_myRb.drag = 3;
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
                    //_myRb.drag = 0;
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

                        _playerManagerScript._player1.StopVibration();
                    }
                    else if (gameObject.tag == "Player2")
                    {
                        _playerManagerScript._player2.StopVibration();
                    }
                    else if (gameObject.tag == "Player3")
                    {
                        _playerManagerScript._player3.StopVibration();
                    }
                    else if (gameObject.tag == "Player4")
                    {
                        _playerManagerScript._player4.StopVibration();
                    }
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
            _animator.SetBool("IsSlingshoting", false);
            //_myRb.drag = 0;
            powerJaugeParent.gameObject.SetActive(false);
            _myRb.velocity = new Vector2 (_inputVariableToStoreDirection.x, -_inputVariableToStoreDirection.y).normalized * (-_timerPower * speed);

            _inputVariableToStoreDirection = Vector2.zero;
            _lastFramePower = _timerPower;
            _timerPower = 0;
            _timerDeadPoint = 0;
            
            //_soundManagerScript.NoSound();
            _playerInput = INPUTSTATE.None;
        }
        else if (_playerInput == INPUTSTATE.None)
        {
            vibrationTreshold = 0.2f;
            powerJauge.fillAmount = 0;
            if (gameObject.tag == "Player1")
            {
                _playerManagerScript._player1.StopVibration();
            }
            else if (gameObject.tag == "Player2")
            {
                _playerManagerScript._player2.StopVibration();
            }
            else if (gameObject.tag == "Player3")
            {
                _playerManagerScript._player3.StopVibration();
            }
            else if (gameObject.tag == "Player4")
            {
                _playerManagerScript._player4.StopVibration();
            }
        }
        #endregion

        //Fait apparaitre une trail si la vitesse atteind le seuil des murs (on changera après le 0.8 par une variable)
        _myVelocity = _myRb.velocity.sqrMagnitude;
        _velocityConvertedToRatio = (_myVelocity / _velocityMax);
        if (_velocityConvertedToRatio > 0.8)
        {
            GetComponent<TrailRenderer>().enabled = true;
        }
        else
        {
            GetComponent<TrailRenderer>().enabled = false;
        }
        _lastFrameVelocity = _myRb.velocity;

    }


    private void Update()
    {
        if(_playerInput == INPUTSTATE.GivingInput && _mustPlayCastSound)
        {
           _soundManagerScript.PlaySound(GetComponent<AudioSource>(), _soundManagerScript.playerCast);
            _mustPlayCastSound = false;
        }
        else if (_playerInput == INPUTSTATE.None)
        {
            _soundManagerScript.NoSound(GetComponent<AudioSource>());
            _mustPlayCastSound = true;
        }


        if (powerJauge.fillAmount > vibrationTreshold)
        {
            if (gameObject.tag == "Player1")
            {
                _playerManagerScript.Vibration(_playerManagerScript._player1, 0, 1.0f, vibrationTreshold * 0.5f);
            }
            else if (gameObject.tag == "Player2")
            {
                _playerManagerScript.Vibration(_playerManagerScript._player2, 0, 1.0f, vibrationTreshold * 0.5f);
            }
            if (gameObject.tag == "Player3")
            {
                _playerManagerScript.Vibration(_playerManagerScript._player3, 0, 1.0f, vibrationTreshold * 0.5f);
            }
            else if (gameObject.tag == "Player4")
            {
                _playerManagerScript.Vibration(_playerManagerScript._player4, 0, 1.0f, vibrationTreshold * 0.5f);
            }
            vibrationTreshold += 0.2f;
        }
        else if (powerJauge.fillAmount == vibrationTreshold)
        {
            if (gameObject.tag == "Player1")
            {
                _playerManagerScript.Vibration(_playerManagerScript._player1, 0, 1.0f, tooMuchPowerTimerMax);
            }
            else if (gameObject.tag == "Player2")
            {
                _playerManagerScript.Vibration(_playerManagerScript._player2, 0, 1.0f, tooMuchPowerTimerMax);
            }
            if (gameObject.tag == "Player3")
            {
                _playerManagerScript.Vibration(_playerManagerScript._player3, 0, 1.0f, tooMuchPowerTimerMax);
            }
            else if (gameObject.tag == "Player4")
            {
                _playerManagerScript.Vibration(_playerManagerScript._player4, 0, 1.0f, tooMuchPowerTimerMax);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            PlayerEntity otherPlayer = collision.gameObject.GetComponent<PlayerEntity>();
            Rebound(otherPlayer.GetLastFrameVelocity(), collision.GetContact(0).normal, frictionPlayer);
            otherPlayer.Rebound(_lastFrameVelocity, collision.GetContact(0).normal, frictionPlayer);
        }

            _particuleContact.transform.position = new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y, _particuleContact.transform.position.z);
            _particuleContact.GetComponent<ParticleSystem>().Play();
    }

    private void Rebound(Vector3 reboundVelocity, Vector3 collisionNormal, float friction)
    {
        Vector3 direction = Vector3.Reflect(-reboundVelocity.normalized, collisionNormal);
        print(name + " rebound direction = " + direction);
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

    
}
