using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private int wallLife;

    private WallManager wallManagerScript;
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
            this.gameObject.SetActive(false);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<AttackTest>().reboundWallDamage > 0)
        {
            collision.GetComponent<AttackTest>().reboundWallDamage -= 1;
            wallLife -= 1;
        }
    }

}
