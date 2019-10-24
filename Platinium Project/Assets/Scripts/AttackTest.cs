using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class AttackTest : MonoBehaviour
{

    //Grégoire s'est occupé de ce script

    //shockwave paramètres
    [Header("Shockwave")]
    public Transform shockWavePosition;
    public float shockWaveRadius;
    public float shockWaveCooldownMax;
    public float shockWaveDurationMax;
    private float shockWaveDuration;
    [HideInInspector]
    public bool isShockWavePossible;
    private float shockWaveCooldown;
    
    //apparence shockwave
    public GameObject shockWaveSprite;


    private bool isShockWaveButtonPressed;

    public LayerMask EnemyMask;
    public float pushbackIntensity;

    private PlayerManager _playerManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        //set les paramètres de la shockwave au début pour qu'elle soit lançable
        shockWaveCooldown = 0;
        shockWaveDuration = shockWaveDurationMax;
        _playerManagerScript = GameObject.FindWithTag("GameController").GetComponent<PlayerManager>();
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


        

        //active la shockwave pendant un certain temps
        if (isShockWaveButtonPressed && isShockWavePossible && !GetComponent<ShockwaveHit>().haveIBeenHit)
        {
            shockWaveDuration -= Time.deltaTime;
            
            
            if (shockWaveDuration > 0)
            {



                if (gameObject.tag == "Player1")
                {
                    _playerManagerScript.Vibration(_playerManagerScript._player1, 0, 1.0f, shockWaveDurationMax);
                }
                else if (gameObject.tag == "Player2")
                {
                    _playerManagerScript.Vibration(_playerManagerScript._player2, 0, 1.0f, shockWaveDurationMax);
                }
                shockWaveSprite.SetActive(true);
                //set un cercle qui check les colliders dedans, si il y a un joueur, il le rajoute dans un tableau et permet d'accéder à l'objet qui contient le collider
                Collider2D[] enemiesCollider = Physics2D.OverlapCircleAll(shockWavePosition.position, shockWaveRadius, EnemyMask);
                if(enemiesCollider.Length > 0)
                {
                    // pour chaque élément dans le tableau, lui ajoute un addforce pour la shockwave et un compteur de dommages pour les murs
                    for (int i = 0; i < enemiesCollider.Length; i++)
                    {
                        Vector3 moveDirection = enemiesCollider[i].transform.position - this.transform.position;

                        enemiesCollider[i].GetComponent<ShockwaveHit>().haveIBeenHit = true;

                        //enemiesCollider[i].GetComponent<Rigidbody2D>().velocity = this.transform.forward * Time.deltaTime * _pushbackIntensity;
                        enemiesCollider[i].GetComponent<Rigidbody2D>().AddForce(moveDirection.normalized * Time.deltaTime * pushbackIntensity);
                        Debug.Log("Hit");
                    }
                }
                else
                {
                    //si rien n'est check
                    Debug.Log("Not hit");
                }

            }
            //lorsque la durée de la shockwave est finie, reset les varibles utilisées
            else
            {
                shockWaveDuration = shockWaveDurationMax;
                isShockWavePossible = false;
                isShockWaveButtonPressed = false;
            }
        }
    }

    
    public void Push()
    {
        isShockWaveButtonPressed = true;
    }

    //permet de voir le cercle de la shockwave dans l'éditeur
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(shockWavePosition.position, shockWaveRadius);
    }
}
