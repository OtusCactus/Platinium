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
    public bool isConnected = false;
    public bool isMoving = false;

    private WallManager _wallManagerScript;
    private WallChange _thisWallChange;

    private GameObject _connectedWall;
    private WallProprieties _connectedWallProprieties;
    private WallChange _connectedWallChange;

    public GameObject[] theWalls;

    // Start is called before the first frame update
    private void Awake()
    {
        theWalls = new GameObject[transform.childCount];

        for (int i = 0; i < theWalls.Length; i++)
        {
            theWalls[i] = transform.GetChild(i).gameObject;
        }
    }
    void Start()
    {
        _wallManagerScript = GameObject.FindWithTag("WallController").GetComponent<WallManager>();
        _thisWallChange = GetComponent<WallChange>();

        

        _connectedWall = _wallManagerScript.SetConnectedWall(gameObject);
        if(_connectedWall != null)
        {
            _connectedWallProprieties = _connectedWall.GetComponent<WallProprieties>();
            _connectedWallChange = _connectedWall.GetComponent<WallChange>();
        }

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
        if (isConnected)
        {
            if (player.GetPlayerINPUTSTATE() != PlayerEntity.INPUTSTATE.GivingInput)
            {
                _wallManagerScript.ConnectedtWallDammage(player.GetVelocityRatio(), gameObject, _connectedWallProprieties, _connectedWallChange);
                _thisWallChange.SetShaderNeededTrue();
                _connectedWallChange.SetShaderNeededTrue();
            }
        }
    }

}
