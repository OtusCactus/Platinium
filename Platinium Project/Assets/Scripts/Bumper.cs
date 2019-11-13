using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    private WallManager _wallManagerScript;
    public float friction;

    // Start is called before the first frame update
    void Start()
    {
        _wallManagerScript = GameObject.FindWithTag("WallController").GetComponent<WallManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerEntity player = collision.gameObject.GetComponent<PlayerEntity>();
        Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

        _wallManagerScript.Bounce(player.GetLastFrameVelocity(), collision.GetContact(0).normal, playerRigidbody, player.speed, friction);
    }

}
