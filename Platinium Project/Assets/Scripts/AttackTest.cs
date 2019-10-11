using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTest : MonoBehaviour
{

    public Transform shockWavePosition;

    public float shockWaveRadius;
    private float shockWaveCooldown;
    public float shockWaveCooldownMax;

    public GameObject shockWaveSprite;

    public float shockWaveDurationMax;
    private float shockWaveDuration;

    private bool _isShockWaveButtonPressed;
    private bool _isShockWavePossible;

    public LayerMask EnemyMask;
    public float _pushbackIntensity;



    // Start is called before the first frame update
    void Start()
    {
        shockWaveCooldown = 0;
        shockWaveDuration = shockWaveDurationMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isShockWavePossible)
        {
            shockWaveSprite.SetActive(false);
            shockWaveCooldown -= Time.deltaTime;
            
            if (shockWaveCooldown <= 0 )
            {
                _isShockWavePossible = true;
                shockWaveCooldown = shockWaveCooldownMax;
            }
            
        }

        if (Input.GetKeyDown("joystick button 0") && _isShockWavePossible)
        {
            _isShockWaveButtonPressed = true;
        }

        if (_isShockWaveButtonPressed && _isShockWavePossible)
        {
            shockWaveDuration -= Time.deltaTime;
            if(shockWaveDuration > 0)
            {
                shockWaveSprite.SetActive(true);
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
            else
            {
                shockWaveDuration = shockWaveDurationMax;
                _isShockWavePossible = false;
                _isShockWaveButtonPressed = false;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(shockWavePosition.position, shockWaveRadius);
    }
}
