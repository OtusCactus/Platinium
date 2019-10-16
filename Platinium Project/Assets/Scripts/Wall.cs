using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int wallLife;
    public float wallLimitVelocity;
    public float wallMinVelocity;

    private WallManager wallManagerScript;

    public float _playerVelocity;
    // Start is called before the first frame update
    void Start()
    {
        wallManagerScript = GameObject.FindWithTag("GameController").GetComponent<WallManager>();
        wallLife = wallManagerScript._wallLife;
    }

    // Update is called once per frame
    void Update()
    {
        if (wallLife <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            //GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<LineRenderer>().enabled = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        _playerVelocity = collision.GetComponent<Rigidbody2D>().velocity.sqrMagnitude;
        if (_playerVelocity >= wallLimitVelocity)
        {
            wallLife = 0;
        }
        else if (_playerVelocity >= wallMinVelocity && _playerVelocity < wallLimitVelocity)
        {
            wallLife -= 1;
        }
    }

}
