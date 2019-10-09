using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTest : MonoBehaviour
{

    public Transform uppercutPosition;
    public float uppercutRadius;

    public LayerMask EnemyMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            Collider2D[] enemiesCollider = Physics2D.OverlapCircleAll(uppercutPosition.position, uppercutRadius, EnemyMask);
            for (int i = 0; i < enemiesCollider.Length; i++)
            {
                Debug.Log("Hit");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(uppercutPosition.position, uppercutRadius);
    }
}
