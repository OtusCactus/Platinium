using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallProprieties : MonoBehaviour
{
    private float friction = 15;
    [Header("Wall Type")]
    public bool isBouncy = false;
    public bool isSticky = false;
    public bool isIndestructible = false;
    public bool isConnectedRight = false;
    public bool isConnectedLeft = false;
    public bool isMoving = false;

    private WallManager _wallManagerScript;
    private WallChange _thisWallChange;

    private GameObject _rightWall;
    private WallProprieties _rightWallProprieties;
    private WallChange _rightWallChange;

    private GameObject _leftWall;
    private WallProprieties _leftWallProprieties;
    private WallChange _leftWallChange;

    // Start is called before the first frame update
    void Start()
    {
        _wallManagerScript = GameObject.FindWithTag("WallController").GetComponent<WallManager>();
        _thisWallChange = GetComponent<WallChange>();

        _rightWall = _wallManagerScript.SetConnectedWallRight(gameObject);
        _rightWallProprieties = _rightWall.GetComponent<WallProprieties>();
        _rightWallChange = _rightWall.GetComponent<WallChange>();

        _leftWall = _wallManagerScript.SetConnectedLeftWall(gameObject);
        _leftWallProprieties = _leftWall.GetComponent<WallProprieties>();
        _leftWallChange = _leftWall.GetComponent<WallChange>();

        if (isBouncy)
        {
            friction = _wallManagerScript.wallBouncyFriction;
        }
        else
        {
            friction = _wallManagerScript.wallFriction;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerEntity player = collision.gameObject.GetComponent<PlayerEntity>();
        Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

        
        if (!isSticky)
        {
            _wallManagerScript.Bounce(player.GetLastFrameVelocity(), collision.GetContact(0).normal, playerRigidbody, player.speed, friction);
        }
        else
        {
            _wallManagerScript.StickyWall(playerRigidbody);
        }

        //permet aux murs connectés de partagés leur dommages
        //Si un mur est connecté à celui à sa droite, alors le mur à la droite prendra les mêmes dégâts que celui à gauche, et vice-versa
        if (isConnectedRight)
        {
            if (player.GetPlayerINPUTSTATE() != PlayerEntity.INPUTSTATE.GivingInput)
            {
                _wallManagerScript.ConnectedRightWall(player.GetVelocityRatio(), gameObject, _rightWallProprieties, _rightWallChange);
            }
        }
        if (isConnectedLeft)
        {
            if (player.GetPlayerINPUTSTATE() != PlayerEntity.INPUTSTATE.GivingInput)
            {
                _wallManagerScript.ConnectedLeftWall(player.GetVelocityRatio(), gameObject, _leftWallProprieties, _leftWallChange);
            }
        }
        if(_rightWallProprieties.isConnectedLeft && !_rightWallProprieties.isIndestructible)
        {
            if (player.GetPlayerINPUTSTATE() != PlayerEntity.INPUTSTATE.GivingInput)
            {
                _rightWallChange.SetDammageFromConnect(_thisWallChange.GetPlayerVelocityRatio());
            }
        }
        if (_leftWallProprieties.isConnectedRight && !_leftWallProprieties.isIndestructible)
        {
            if (player.GetPlayerINPUTSTATE() != PlayerEntity.INPUTSTATE.GivingInput)
            {
                _leftWallChange.SetDammageFromConnect(_thisWallChange.GetPlayerVelocityRatio());
            }
        }
    }

}
