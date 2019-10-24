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
    private float mouvementPlayerDisabledTime;
    public float reactivatingScriptVelocity = 0.2f;

    private bool _hitWalls;
    private SoundManager _soundManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        mouvementPlayerDisabledTime = mouvementPlayerDisabledTimeMax;
        _soundManagerScript = GameObject.FindWithTag("GameController").GetComponent<SoundManager>();

    }

    // Update is called once per frame
    void Update()
    {
        //si le joueur est hit par une shockwave d'un autre joueur, désactive son script de mouvement pendant un certain temps
        if (haveIBeenHit)
        {
            this.GetComponent<PlayerEntity>().powerJaugeParent.gameObject.SetActive(false);
            this.GetComponent<PlayerEntity>().enabled = false;
            mouvementPlayerDisabledTime -= Time.deltaTime;
            if(GetComponent<PlayerEntity>().GetVelocityRatio() <= reactivatingScriptVelocity || _hitWalls)
            {
                this.GetComponent<PlayerEntity>().enabled = true;
                haveIBeenHit = false;
                mouvementPlayerDisabledTime = mouvementPlayerDisabledTimeMax;
                _hitWalls = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Walls")
        {
            _hitWalls = true;
            _soundManagerScript.PlaySound(transform.GetChild(0).GetComponent<AudioSource>(),_soundManagerScript.wallHit);
           
        }
        else if (collision.tag.Contains("Player"))
        {
            _soundManagerScript.PlaySound(transform.GetChild(0).GetComponent<AudioSource>(), _soundManagerScript.playersCollision);

        }
    }
}
