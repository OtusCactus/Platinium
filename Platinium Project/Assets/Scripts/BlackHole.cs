using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float gravitationalPull = 5;



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            if (collision.transform.position == transform.position)
            {
                print("le joueur a perdu");
            }
            if (Vector3.Distance(transform.position, collision.transform.position) < 0.001f)
            {
                collision.transform.position = transform.position;
            }
            else
            {
               collision.transform.position = Vector3.MoveTowards(collision.transform.position, transform.position, gravitationalPull / Vector3.Distance(transform.position, collision.transform.position) * Time.deltaTime);
            }
        }
    }
}
