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

    private bool isShockWaveButtonPressed;
    private bool isShockWavePossible;
    private bool hasWallDamageBeenAssigned;

    public LayerMask EnemyMask;
    public float pushbackIntensity;

    public int reboundWallDamage {  get;  set; }

    // Start is called before the first frame update
    void Start()
    {
        shockWaveCooldown = 0;
        shockWaveDuration = shockWaveDurationMax;
    }

    // Update is called once per frame
    void Update()
    {
        //cooldown de la shockwave
        if (!isShockWavePossible)
        {
            shockWaveSprite.SetActive(false);
            shockWaveCooldown -= Time.deltaTime;
            
            if (shockWaveCooldown <= 0 )
            {
                isShockWavePossible = true;
                shockWaveCooldown = shockWaveCooldownMax;
            }
            
        }

        //permet de faire en sorte que la shockwave dure pendant un certain temps
        if ((Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.A)) && isShockWavePossible)
        {
            isShockWaveButtonPressed = true;
        }

        //active la shockwave pendant un certains temps
        if (isShockWaveButtonPressed && isShockWavePossible)
        {
            shockWaveDuration -= Time.deltaTime;
            if(shockWaveDuration > 0)
            {
                shockWaveSprite.SetActive(true);
                //set un cercle qui check les colliders dedans, si il y a un joueur, il le rajoute dans un tableau et permet d'accéder à l'objet qui contient le collider
                Collider2D[] enemiesCollider = Physics2D.OverlapCircleAll(shockWavePosition.position, shockWaveRadius, EnemyMask);
                if(enemiesCollider.Length > 0)
                {
                    // pour chaque élément dans le tableau, lui ajoute un addforce pour la shockwave et un compteur de dommages pour les murs
                    for (int i = 0; i < enemiesCollider.Length; i++)
                    {
                        Vector3 moveDirection = enemiesCollider[i].transform.position - this.transform.position;
                        if(!hasWallDamageBeenAssigned)
                        {
                            enemiesCollider[i].GetComponent<AttackTest>().reboundWallDamage = 1;
                            hasWallDamageBeenAssigned = true;
                        }
                        //enemiesCollider[i].GetComponent<Rigidbody2D>().velocity = this.transform.forward * Time.deltaTime * _pushbackIntensity;
                        enemiesCollider[i].GetComponent<Rigidbody2D>().AddForce(moveDirection.normalized * Time.deltaTime * pushbackIntensity);
                        Debug.Log("Hit");
                    }
                }
                else
                {
                    //si rien n'est check
                    Debug.Log("Bruh");
                }

            }
            //lorsque la durée de la shockwave est finie, reset les varibles utilisées
            else
            {
                shockWaveDuration = shockWaveDurationMax;
                isShockWavePossible = false;
                isShockWaveButtonPressed = false;
                hasWallDamageBeenAssigned = false;
            }
        }
    }

    //permet de voir le cercle de la shockwave dans l'éditeur
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(shockWavePosition.position, shockWaveRadius);
    }
}
