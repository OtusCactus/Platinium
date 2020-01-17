using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallProprieties : MonoBehaviour
{
    private float friction = 15;
    [Header("Wall Type")]
    private bool _isBouncy = false;
    private bool _isSticky = false;
    private bool _isIndestructible = false;
    private bool _isConnected = false;
    private bool _isMoving = false;

    private WallManager _wallManagerScript;
    private WallChange _thisWallChange;

    private GameObject _connectedWall;
    private WallProprieties _connectedWallProprieties;
    private WallChange _connectedWallChange;

    public GameObject[] theWalls;
    private RandomizerArena _myBibli;
    private int _thisArenaIndex = -1;
    private int _myChildPosition = -1;

    // Start is called before the first frame update
    private void Awake()
    {
        theWalls = new GameObject[transform.childCount];
        _wallManagerScript = GameObject.FindWithTag("WallController").GetComponent<WallManager>();
        _thisWallChange = GetComponent<WallChange>();

        for (int i = 0; i < theWalls.Length; i++)
        {
            theWalls[i] = transform.GetChild(i).gameObject;
        }
    }
    void Start()
    {
        //UpdateProprieties();
        
        if(_connectedWall != null)
        {
            _connectedWallProprieties = _connectedWall.GetComponent<WallProprieties>();
            _connectedWallChange = _connectedWall.GetComponent<WallChange>();
        }

        if (_isBouncy)
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
        
        if (!_isSticky)
        {
            _wallManagerScript.Bounce(player.GetLastFrameVelocity(), collision.GetContact(0).normal, playerRigidbody, player.speed, friction);
        }
        else
        {
            _wallManagerScript.StickyWall(playerRigidbody);
        }

        //permet aux murs connectés de partagés leur dommages
        //Si un mur est connecté à celui à sa droite, alors le mur à la droite prendra les mêmes dégâts que celui à gauche, et vice-versa
        if (_isConnected)
        {
            if (player.GetPlayerINPUTSTATE() != PlayerEntity.INPUTSTATE.GivingInput)
            {
                _wallManagerScript.ConnectedtWallDammage(player.GetVelocityRatio(), gameObject, _connectedWallProprieties, _connectedWallChange);
                _thisWallChange.SetShaderNeededTrue();
                _connectedWallChange.SetShaderNeededTrue();
            }
        }
    }

    public void UpdateProprieties()
    {
        _thisArenaIndex = _wallManagerScript.GetRandomArenaIndex();
        _myBibli = _wallManagerScript.GetThisRoundBibli();
        int listCount = _myBibli.arenas[_thisArenaIndex].wallsNamesList.Count;
        for (int i = 0; i < _myBibli.arenas[_thisArenaIndex].wallsNamesList.Count; i++)
        {
            string temp = _myBibli.arenas[_thisArenaIndex].wallsNamesList[i].wallName;
            if (_myBibli.arenas[_thisArenaIndex].wallsNamesList[i].wallName == gameObject.name)
            {
                _myChildPosition = i;
                break;
            }
        }
        _isBouncy = _myBibli.arenas[_thisArenaIndex].wallsNamesList[_myChildPosition].isBounc;
        _isIndestructible = _myBibli.arenas[_thisArenaIndex].wallsNamesList[_myChildPosition].isIndestructibl;
        _isConnected = _myBibli.arenas[_thisArenaIndex].wallsNamesList[_myChildPosition].isConnecte;
        

        _connectedWall = _wallManagerScript.SetConnectedWall(gameObject);
    }

    public bool GetIsBouncy()
    {
        return _isBouncy;
    }
    public bool GetIsIndestructible()
    {
        return _isIndestructible;
    }
    public bool GetIsConnected()
    {
        return _isConnected;
    }
}
