﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetMenuInformation : MonoBehaviour
{
    public static GetMenuInformation Instance = null;


    //Grégoire à fait ce script
    public int numbersOfPlayers;
    public float musicVolume = 1;
    public float sfxVolume = 1;
    public bool vibrationBool;
    private bool[] playerMouvementMode;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        //permet de garder cet objet lors de la transition du menu à la scène de jeu
        DontDestroyOnLoad(this.gameObject);
        vibrationBool = true;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        numbersOfPlayers = 4;
        playerMouvementMode = new bool[4];
    }

    // Update is called once per frame
    void Update()
    {

        print("vibration" + vibrationBool);
    }

    //permet de changer le nombre de joeuur qui seront présent dans la scène de jeu
    public void GetPlayerNumbers(int numberPlayers)
    {
        numbersOfPlayers = numberPlayers;
    }

    public void setPlayerMouvementMode(int boolNumber, bool state)
    {
        playerMouvementMode[boolNumber] = state;
    }

    public bool[] getPlayerMouvementMode()
    {
        return playerMouvementMode;
    }

    public void SetMusicVolume(float sliderValue)
    {
        musicVolume = sliderValue;
    }
    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void SetSFXVolume(float sliderValue)
    {
        sfxVolume = sliderValue;
    }
    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public void SetVibrationsValue(bool toggleValue)
    {
        vibrationBool = toggleValue;
    }
    public bool GetVibrationsValue()
    {
        return vibrationBool;
    }

}
