﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveHit : MonoBehaviour
{
    //check si le joueur est bien hit
    public bool haveIBeenHit;

    //Empêche l'autre joueur de se déplacer pendant un certain temps après être hit par la shockwave
    public float mouvementPlayerDisabledTimeMax;
    private float mouvementPlayerDisabledTime;
    // Start is called before the first frame update
    void Start()
    {
        mouvementPlayerDisabledTime = mouvementPlayerDisabledTimeMax;
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
            if(mouvementPlayerDisabledTime <= 0)
            {
                this.GetComponent<PlayerEntity>().enabled = true;
                haveIBeenHit = false;
                mouvementPlayerDisabledTime = mouvementPlayerDisabledTimeMax;
            }
        }
    }
}
