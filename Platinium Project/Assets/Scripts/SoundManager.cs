using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //grégoire c'est occupé de ce script.

    public AudioClip wallHit;
    public AudioClip wallBouncyHit;
    public AudioClip playerCast;
    public AudioClip playersCollision;
    public AudioClip endRound;

    public AudioSource myAudio;


    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(AudioSource myAudioSource, AudioClip thisAudio)
    {
        myAudioSource.loop = false;
        myAudioSource.enabled = false;
        myAudioSource.clip = thisAudio;
        myAudioSource.enabled = true;
    }


    public void NoSound(AudioSource myAudioSource)
    {
         myAudioSource.enabled = false;
    }
}
