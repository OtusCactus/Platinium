using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveHit : MonoBehaviour
{
    //Grégoire s'est occupé de ce script

    //check si le joueur est bien hit
    public bool haveIBeenHit;

    //Empêche l'autre joueur de se déplacer pendant un certain temps après être hit par la shockwave
    public float mouvementPlayerDisabledTimeMax;
    //private float mouvementPlayerDisabledTime;
    public float reactivatingScriptVelocity = 0.2f;

    //check si les murs ont été touchés
    private bool _hitWalls;

    //garde les components nécéssaires au script
    private SoundManager _soundManagerScript;
    private PlayerEntity _playerEntityScript;

    private AudioSource _childAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        //mouvementPlayerDisabledTime = mouvementPlayerDisabledTimeMax;
        _soundManagerScript = GameObject.FindWithTag("GameController").GetComponent<SoundManager>();
        _playerEntityScript = GetComponent<PlayerEntity>();
        _childAudioSource = transform.GetChild(0).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //si le joueur est hit par une shockwave d'un autre joueur, désactive son script de mouvement pendant un certain temps
        if (haveIBeenHit)
        {
            _playerEntityScript.powerJaugeParent.gameObject.SetActive(false);
            _playerEntityScript.enabled = false;
            //mouvementPlayerDisabledTime -= Time.deltaTime;
            if(_playerEntityScript.GetVelocityRatio() <= reactivatingScriptVelocity || _hitWalls)
            {
                _playerEntityScript.enabled = true;
                haveIBeenHit = false;
                //mouvementPlayerDisabledTime = mouvementPlayerDisabledTimeMax;
                _hitWalls = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //si on touche un mur on un joueur, joue un son différent
        if (collision.tag.Contains("Walls"))
        {
            _hitWalls = true;
            _soundManagerScript.PlaySound(_childAudioSource, _soundManagerScript.wallHit);
           
        }
        else if (collision.tag.Contains("Player"))
        {
            _soundManagerScript.PlaySound(_childAudioSource, _soundManagerScript.playersCollision);

        }
    }
}
