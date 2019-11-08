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

    // Start is called before the first frame update
    void Start()
    {
        _wallManagerScript = GameObject.FindWithTag("WallController").GetComponent<WallManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBouncy)
        {
            friction = _wallManagerScript.wallBouncyFriction;
        }
        else
        {
            friction = _wallManagerScript.wallFriction;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerEntity player = collision.gameObject.GetComponent<PlayerEntity>();
        if (!isSticky)
        {
            _wallManagerScript.Bounce(player.GetLastFrameVelocity(), collision.GetContact(0).normal, collision.gameObject.GetComponent<Rigidbody2D>(), player.speed, friction);
        }
        else
        {
            _wallManagerScript.StickyWall(collision.gameObject.GetComponent<Rigidbody2D>());
        }
    }

}
