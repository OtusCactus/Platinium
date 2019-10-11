using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveHit : MonoBehaviour
{
    public bool haveIBeenHit;
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
        if (haveIBeenHit)
        {
            this.GetComponent<MouvementPlayer>().powerSlider.gameObject.SetActive(false);
            this.GetComponent<MouvementPlayer>().enabled = false;
            mouvementPlayerDisabledTime -= Time.deltaTime;
            if(mouvementPlayerDisabledTime <= 0)
            {
                this.GetComponent<MouvementPlayer>().enabled = true;
                this.GetComponent<AttackTest>().reboundWallDamage = 0;
                haveIBeenHit = false;
                mouvementPlayerDisabledTime = mouvementPlayerDisabledTimeMax;
            }
        }
    }
}
