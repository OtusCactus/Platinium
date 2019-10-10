using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTest : MonoBehaviour
{

    public Transform shockWavePosition;
    public float shockWaveRadius;

    public LayerMask EnemyMask;
    public float _pushbackIntensity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.A))
        {

            Collider2D[] enemiesCollider = Physics2D.OverlapCircleAll(shockWavePosition.position, shockWaveRadius, EnemyMask);
            if(enemiesCollider.Length > 0)
            {
                
                for (int i = 0; i < enemiesCollider.Length; i++)
                {
                    Vector3 moveDirection = enemiesCollider[i].transform.position - this.transform.position;
                    //enemiesCollider[i].GetComponent<Rigidbody2D>().velocity = this.transform.forward * Time.deltaTime * _pushbackIntensity;
                    enemiesCollider[i].GetComponent<Rigidbody2D>().AddForce(moveDirection.normalized * Time.deltaTime * _pushbackIntensity);
                    Debug.Log("Hit");
                }
            }
            else
            {
                Debug.Log("Bruh");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(shockWavePosition.position, shockWaveRadius);
    }
}
