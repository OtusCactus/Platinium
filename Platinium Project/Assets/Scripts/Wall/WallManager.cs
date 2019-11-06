using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    //Grégoire s'est occupé de ce script

    public float wallFriction = 15;
    public float wallBouncyFriction = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Changement de face en fonction de la face actuelle et des faces proches.
    public int WallFaceChange(int[] wallOrientFaceChangeTab, int currentFace)
    {
        int nextface;

        nextface = wallOrientFaceChangeTab[currentFace];
        
        return nextface;
    }
    public void Bounce(Vector3 playerVelocity, Vector3 collisionNormal, Rigidbody2D _myRb, float playerSpeed, float myFriction)
    {
        Vector3 direction = Vector3.Reflect(playerVelocity.normalized, collisionNormal);
        _myRb.velocity = new Vector3(direction.x, direction.y).normalized * ((playerVelocity.magnitude / myFriction) * playerSpeed);
    }
    public void StickyWall(Rigidbody2D _myRb)
    {
        _myRb.velocity = Vector3.zero;
    }
}
