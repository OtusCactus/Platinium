using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private PlayerEntity _playerEntityScript;
    private ShockwaveHit _shockWaveHitScript;

    //playerCameraPosition
    private Camera cameraMain;
    private Vector2 playerFromCameraPosition;
    private Vector2 playerOutPosition;
    public GameObject playerOutAnim;
    private bool hasPositionBeenTaken;
    private bool _hasAnimationEnded;
    private bool _hasRoundEnded;
    private new NewSoundManager _newSoundManagerScript;
    private bool _hasSoundPlayed = false;

    private Image _playerScoreImage;
    private GetMenuInformation _menuInformationScript;




    // Start is called before the first frame update
    void Start()
    {
        //set les paramètres de la shockwave au début pour qu'elle soit lançable
        shockWaveCooldown = 0;
        shockWaveDuration = shockWaveDurationMax;
        _playerManagerScript = GameObject.FindWithTag("GameController").GetComponent<PlayerManager>();
        _playerEntityScript = GetComponent<PlayerEntity>();
        _shockWaveHitScript = GetComponent<ShockwaveHit>();
        cameraMain = Camera.main;
        _newSoundManagerScript = NewSoundManager.instance;
        if (GameObject.FindWithTag("MenuManager") != null)
        {

            _menuInformationScript = GameObject.FindWithTag("MenuManager").GetComponent<GetMenuInformation>();
        }

        string thisTag = gameObject.tag;
        switch (thisTag)
        {
            case "Player1":
                _playerScoreImage = GameObject.FindWithTag("PlayerOneImage").GetComponent<Image>();
                break;
            case "Player2":
                _playerScoreImage = GameObject.FindWithTag("PlayerTwoImage").GetComponent<Image>();
                break;
            case "Player3":
                _playerScoreImage = GameObject.FindWithTag("PlayerThreeImage").GetComponent<Image>();
                break;
            case "Player4":
                _playerScoreImage = GameObject.FindWithTag("PlayerFourImage").GetComponent<Image>();
                break;
        }


    }



    // Update is called once per frame
    void Update()
    {
        //prends la position du joueur quand il quitte la caméra
        if(!hasPositionBeenTaken && !_hasRoundEnded)
        {
            playerFromCameraPosition = cameraMain.WorldToScreenPoint(transform.position);
            if (playerFromCameraPosition.x < 0 || playerFromCameraPosition.x > cameraMain.pixelWidth || playerFromCameraPosition.y < 0 || playerFromCameraPosition.y > cameraMain.pixelHeight)
            {
                playerOutPosition = transform.position;
                
                hasPositionBeenTaken = true;
                _hasRoundEnded = true;
            }
        }

        //active l'animation et lui donne la bonne position
        if (hasPositionBeenTaken)
        {
            playerOutAnim.transform.position = new Vector3 (playerOutPosition.x, playerOutPosition.y, transform.position.z);
            playerOutAnim.SetActive(true);
            if (!_hasSoundPlayed)
            {
                _newSoundManagerScript.PlaySound("Elimination");
                _hasSoundPlayed = true;
            }
        }

        //permet de reset les valeurs quand l'animation est finie
        if(_hasAnimationEnded)
        {
            hasPositionBeenTaken = false;
            playerOutAnim.SetActive(false);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _hasAnimationEnded = false;
            _hasSoundPlayed = false;
        }



        //active la shockwave pendant un certain temps
        if (isShockWaveButtonPressed && _playerEntityScript.GetUltiBool() && !_shockWaveHitScript.haveIBeenHit)
        {
            shockWaveDuration -= Time.deltaTime;
                _playerEntityScript.resetUltiCurrentCharge();
            if (_menuInformationScript == null || _menuInformationScript.GetVibrationsValue())
            {
                if (shockWaveDuration > 0)
                {

                    if (gameObject.tag == "Player1")
                    {
                        _playerManagerScript.Vibration(_playerManagerScript.player[0], 0, 1.0f, shockWaveDurationMax);
                    }
                    else if (gameObject.tag == "Player2")
                    {
                        _playerManagerScript.Vibration(_playerManagerScript.player[1], 0, 1.0f, shockWaveDurationMax);
                    }
                    else if (gameObject.tag == "Player3")
                    {
                        _playerManagerScript.Vibration(_playerManagerScript.player[2], 0, 1.0f, shockWaveDurationMax);

                    }
                    else if (gameObject.tag == "Player4")
                    {
                        _playerManagerScript.Vibration(_playerManagerScript.player[3], 0, 1.0f, shockWaveDurationMax);

                    }
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

                        enemiesCollider[i].GetComponent<Rigidbody2D>().velocity = moveDirection.normalized * Time.deltaTime * pushbackIntensity;
                        //enemiesCollider[i].GetComponent<Rigidbody2D>().AddForce(moveDirection.normalized * Time.deltaTime * pushbackIntensity);
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
                _playerEntityScript.SetUltiBoolFalse();
                shockWaveSprite.SetActive(false);
                //isShockWavePossible = false;
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

    public void GetAnimationEnd()
    {
        _hasAnimationEnded = true;
    }

    public bool hasPositionBeenSet()
    {
        return hasPositionBeenTaken;
    }

    public void SetHasPositionFalse()
    {
        _hasRoundEnded = false;
    }

    public Image GetPlayerScoreImage()
    {
        return _playerScoreImage;
    }
}
