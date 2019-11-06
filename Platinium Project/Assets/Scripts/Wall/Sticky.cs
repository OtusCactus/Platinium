using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{
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
            StickyWall(collision.gameObject.GetComponent<Rigidbody2D>());
    }

    private void StickyWall(Rigidbody2D _myRb)
    {
        _myRb.velocity = Vector3.zero;
    }
}
