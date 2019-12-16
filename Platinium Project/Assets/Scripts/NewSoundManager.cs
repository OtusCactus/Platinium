using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSoundManager : MonoBehaviour
{
    public static NewSoundManager instance = null;

    public AudioSource music;
    
    [Header("Charge Player Sounds")]
    public AudioClip[] playerChargeSounds;
    private AudioSource[] _myAudios;
    private AudioSource[] _playerCharges;

    [Header("Unique Sounds")]
    public AudioClip[] miscSounds;
    [Header("Animal Sounds")]
    public AudioClip[] animalSounds;
    [Header("Bouncy Wall Sounds")]
    public AudioClip[] bouncySounds;
    [Header("Ulti")]
    public AudioClip[] ultiSounds;
    [Header("Crowd")]
    public AudioClip[] crowdSounds;

    private GetMenuInformation _menuInformationScript;

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
        if (GameObject.FindWithTag("MenuManager") != null)
        {
            _menuInformationScript = GameObject.FindWithTag("MenuManager").GetComponent<GetMenuInformation>();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_menuInformationScript != null)
        {
            for (int i = 0; i < _myAudios.Length; i++)
            {
                _myAudios[i].volume = _menuInformationScript.GetSFXVolume();
            }
            music.volume = _menuInformationScript.GetMusicVolume();
        }
    }

    public int AudioLength()
    {
        return _myAudios.Length;
    }

    public AudioSource[] GetMyAudios()
    {
        AudioSource[] tab = new AudioSource[_myAudios.Length];
        for (int i = 0; i < _myAudios.Length; i++)
        {
            tab[i] = _myAudios[i];
        }
        return tab;
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
    /// Pour sons aléatoires. 0-> murs bouncy, 1-> cris, 2-> ulti, 3-> crowd
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
                audio.clip = bouncySounds[Random.Range(0, bouncySounds.Length - 1)];
                break;
            case 1:
                audio.clip = animalSounds[Random.Range(0, animalSounds.Length - 1)];
                break;
            case 2:
                audio.clip = ultiSounds[Random.Range(0, ultiSounds.Length - 1)];
                break;
            case 3:
                audio.clip = crowdSounds[Random.Range(0, crowdSounds.Length - 1)];
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
