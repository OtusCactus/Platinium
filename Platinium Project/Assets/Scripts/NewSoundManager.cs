﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSoundManager : MonoBehaviour
{
    public static NewSoundManager instance = null;
    
    public AudioClip endRound;
    public AudioClip[] playerChargeSounds;

    private AudioSource[] _myAudios;
    private AudioSource[] _playerCharges;

    public AudioClip[] miscSounds;
    public AudioClip[] animalSounds;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        _myAudios = GetComponents<AudioSource>();
        _playerCharges = new AudioSource[4];
        for (int i = 0; i < 4; i++)
        {
            _playerCharges[i] = _myAudios[i];
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Fonction pour sons définis. 
    /// <param name="clipName"></param>
    public void PlaySound(string clipName)
    {
        AudioSource audio = null;
        for (int i = 4; i < _myAudios.Length; i++)
        {
            if (!_myAudios[i].isPlaying)
            {
                audio = _myAudios[i];
                break;
            }
        }
        audio.pitch = Time.timeScale;
        audio.loop = false;
        for (int x = 0; x < miscSounds.Length; x++)
        {
            if (miscSounds[x].name == clipName)
            {
                audio.clip = miscSounds[x];
            }
        }

        audio.enabled = false;
        audio.enabled = true;
    }

    /// <summary>
    /// Pour sons aléatoires. 0-> murs, 1-> cris, 
    /// </summary>
    /// <param name="tabNumber"></param>
    public void PlaySound(int tabNumber)
    {
        AudioSource audio = null;
        for (int i = 4; i < _myAudios.Length; i++)
        {
            if (!_myAudios[i].isPlaying)
            {
                audio = _myAudios[i];
                break;
            }
        }
        audio.pitch = Time.timeScale;
        audio.loop = false;

        switch (tabNumber)
        {
            case 0:
                print("lesmurs df:ss");
                break;
            case 1:
                audio.clip = animalSounds[Random.Range(0, animalSounds.Length - 1)];
                break;
        }

        audio.enabled = false;
        audio.enabled = true;
    }

    public void PlayCharge(int player)
    {
        _playerCharges[player].pitch = Time.timeScale;
        _playerCharges[player].loop = false;
        _playerCharges[player].enabled = false;

        _playerCharges[player].clip = playerChargeSounds[Random.Range(0, playerChargeSounds.Length - 1)];
        
        _playerCharges[player].enabled = true;
    }
    public void StopCharge(int player)
    {
        _playerCharges[player].enabled = false;
    }
}
