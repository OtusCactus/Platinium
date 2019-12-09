using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSoundManager : MonoBehaviour
{
    public static NewSoundManager instance = null;

    public AudioClip wallHitHighHp;
    public AudioClip wallHitMidHp;
    public AudioClip wallHitLowHp;
    public AudioClip wallHitNoHp;
    public AudioClip wallBouncyHit;
    public AudioClip playerCast;
    public AudioClip playersCollision;
    public AudioClip endRound;

    private AudioSource[] _myAudios;
    private AudioSource[] _playerCharges;

    public AudioClip[] _miscSounds;
    public AudioClip[] _animalSounds;


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
    /// Cette fonction pour jouer un son du sound manager. 1er param -> 0 pour sons uniques, 2 -> cri, 3-> charge ..
    /// </summary>
    /// <param name="clipName"></param>
    public void PlaySound(int tabNumber, string clipName)
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
                for(int x = 0; x < _miscSounds.Length; x++)
                {
                    if (_miscSounds[x].name == clipName)
                    {
                        audio.clip = _miscSounds[x];
                    }
                }
                break;
            case 1:
                for (int x = 0; x < _animalSounds.Length; x++)
                {
                    if (_animalSounds[x].name == clipName)
                    {
                        audio.clip = _animalSounds[x];
                    }
                }
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
        _playerCharges[player].clip = playerCast;
        _playerCharges[player].enabled = true;
    }
    public void StopCharge(int player)
    {
        _playerCharges[player].enabled = false;
    }
}
