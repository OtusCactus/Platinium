﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveHit : MonoBehaviour
{
    //Grégoire s'est occupé de ce script

    //check si le joueur est bien hit
    private bool _haveIBeenHit;

    //Empêche l'autre joueur de se déplacer pendant un certain temps après être hit par la shockwave
    public float mouvementPlayerDisabledTimeMax;
    private float mouvementPlayerDisabledTime;

    //check si les murs ont été touchés
    private bool _hitWalls;

    //garde les components nécéssaires au script
    private PlayerEntity _playerEntityScript;

    public GameObject stunParticles;
    private int _currentUltChargeNumber;

    // Start is called before the first frame update
    void Start()
    {
        mouvementPlayerDisabledTime = 0;
        _playerEntityScript = GetComponent<PlayerEntity>();
        stunParticles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //si le joueur est hit par une shockwave d'un autre joueur, désactive son script de mouvement pendant un certain temps
        if (_haveIBeenHit)
        {
            for (int i = 0; i < _playerEntityScript.UltiFxStates.Length; i++)
            {
                if(_playerEntityScript.UltiFxStates[i].activeSelf == true)
                {
                    _currentUltChargeNumber = i;
                    _playerEntityScript.UltiFxStates[i].SetActive(false);
                }
            }
            stunParticles.SetActive(true);
            _playerEntityScript.powerJaugeParent.gameObject.SetActive(false);
            _playerEntityScript.IsInputDisabled(true);
            _playerEntityScript.ResetTimerPower();
            _playerEntityScript.GetPlayerAnimator().SetBool("IsSlingshoting", false);

            mouvementPlayerDisabledTime += Time.deltaTime;

            if (mouvementPlayerDisabledTime >= mouvementPlayerDisabledTimeMax)
            {
                _playerEntityScript.IsInputDisabled(false);
                _haveIBeenHit = false;
                mouvementPlayerDisabledTime = 0;
                stunParticles.SetActive(false);
                _playerEntityScript.UltiFxStates[_currentUltChargeNumber].SetActive(true);

            }
        }
    }

    private void OnColliderEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Walls"))
        {

            _hitWalls = true;

        }
    }

    public bool GetHaveIBeenHit()
    {
        return _haveIBeenHit;
    }
    public void SetHaveIBeenHitTrue()
    {
        _haveIBeenHit = true;
    }
}
