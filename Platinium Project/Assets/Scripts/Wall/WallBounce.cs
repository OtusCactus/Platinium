using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounce : MonoBehaviour
{
    public float friction = 15;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerEntity player = collision.gameObject.GetComponent<PlayerEntity>();
        Bounce(collision.gameObject.GetComponent<PlayerEntity>().GetLastFrameVelocity(), collision.GetContact(0).normal, collision.gameObject.GetComponent<Rigidbody2D>(), player.speed);
    }

    private void Bounce(Vector3 playerVelocity, Vector3 collisionNormal, Rigidbody2D _myRb, float playerSpeed)
    {
        Vector3 direction = Vector3.Reflect(playerVelocity.normalized, collisionNormal);
        _myRb.velocity = new Vector3(direction.x, direction.y).normalized * ((playerVelocity.magnitude / friction) * playerSpeed);
    }
}
