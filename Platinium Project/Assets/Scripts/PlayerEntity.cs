using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntity : MonoBehaviour
{

    private Vector2 _input;
    private Vector2 _inputVariableToStoreDirection;
    //
    public int speed;
    //
    private Rigidbody2D _myRb;

    private float _timerDeadPoint = 0;

    public float rotationSpeed;
    private float _joyAngle;
    private float _angle;

    [Header("Power")]
    public float powerMax;
    public Image powerJauge;
    public GameObject powerJaugeParent;
    private float _timerPower = 0;

    [HideInInspector] public int _controllerNumber;

    public float myVelocity;

    enum INPUTSTATE { GivingInput, EasingInput, Released, None };
    private INPUTSTATE _playerInput = INPUTSTATE.Released;

    // Start is called before the first frame update
    void Start()
    {
        _myRb = GetComponent<Rigidbody2D>();
        powerJauge.fillAmount = 0;
        powerJaugeParent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        #region Changement Enum
        if (_input != Vector2.zero)
        {
            _playerInput = INPUTSTATE.GivingInput;
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

        myVelocity = _myRb.velocity.sqrMagnitude;

        if (_playerInput == INPUTSTATE.GivingInput)
        {
            _angle = Mathf.Atan2(_input.x, _input.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, _angle);
            powerJaugeParent.gameObject.SetActive(true);
            powerJauge.fillAmount = _timerPower / 5;

            _inputVariableToStoreDirection = _input;
            _myRb.drag = 3;
            _timerPower += Time.deltaTime;
            if (_timerPower > powerMax)
            {
                _timerPower = powerMax;
            }
        }
        if (_playerInput == INPUTSTATE.EasingInput)
        {
                _timerDeadPoint += Time.deltaTime;
        }
        if(_playerInput == INPUTSTATE.Released)
        {
            //_myRb.velocity = new Vector2(0, 0);
            _myRb.drag = 0;
            powerJaugeParent.gameObject.SetActive(false);

            _myRb.velocity = new Vector2 (_inputVariableToStoreDirection.x, -_inputVariableToStoreDirection.y).normalized * (-_timerPower * speed);
            _inputVariableToStoreDirection = Vector2.zero;
            _timerPower = 0;
            _timerDeadPoint = 0;
            _playerInput = INPUTSTATE.None;
        }
    }

    public void SetInputX(Vector2 myInput)
    {
        _input = myInput;
    }
}
