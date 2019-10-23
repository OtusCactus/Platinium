using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntity : MonoBehaviour
{
    //Ce script sert aux déplacements des joueurs
    //Pour l'instant on utilise la physique d'Unity pour le proto, mais on changera pour une physique personnalisée pour la semaine prochaine

    //
    [Header("Speed")]
    public int speed;
    public float rotationSpeed;

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

    //Variables pour la vitesse
    private float _myVelocity;
    private float _velocityMax;
    private float _velocityConvertedToRatio;
    private Vector3 _lastFrameVelocity;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
        else if(_playerInput == INPUTSTATE.EasingInput && _timerDeadPoint >= 0.1)
        {
            _playerInput = INPUTSTATE.Released;
        }
        #endregion

        #region Actions depending on INPUTSTATE
        if (_playerInput == INPUTSTATE.GivingInput)
        {
            _angle = Mathf.Atan2(_input.x, _input.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, _angle);
            powerJaugeParent.gameObject.SetActive(true);
            powerJauge.fillAmount = _timerPower / powerMax;

            _inputVariableToStoreDirection = _input;
            _myRb.drag = 3;
            _timerPower += Time.fixedDeltaTime;
            if (_timerPower > powerMax)
            {
                _timerPower = powerMax;
            }
        }
        else if (_playerInput == INPUTSTATE.EasingInput)
        {
                _timerDeadPoint += Time.fixedDeltaTime;
        }
        else if(_playerInput == INPUTSTATE.Released)
        {
            _myRb.drag = 0;
            powerJaugeParent.gameObject.SetActive(false);
            _myRb.velocity = new Vector2 (_inputVariableToStoreDirection.x, -_inputVariableToStoreDirection.y).normalized * (-_timerPower * speed);
            _lastFrameVelocity = _myRb.velocity;
            _inputVariableToStoreDirection = Vector2.zero;
            _timerPower = 0;
            _timerDeadPoint = 0;
            _playerInput = INPUTSTATE.None;
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag == "Player")
        {
            if (collision.gameObject.tag == "Walls")
            {
                //Bounce(collision.contacts[0].normal);
                Bounce(collision.GetContact(0).normal);
            }
        }
    }

    private void Bounce(Vector3 collisionNormal)
    {
        Vector3 direction = Vector3.Reflect(_lastFrameVelocity.normalized, collisionNormal);
        print(direction + "c'est la direction");
        //_myRb.velocity = new Vector3(direction.x * _lastFrameVelocity.normalized.x, direction.y * _lastFrameVelocity.normalized.y);
        _myRb.velocity = new Vector3(direction.x * _lastFrameVelocity.x, direction.y * _lastFrameVelocity.y);
    }

    public void SetInputX(Vector2 myInput)
    {
        _input = myInput;
    }

    public float GetVelocityRatio()
    {
        return _velocityConvertedToRatio;
    }
}
