using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveSoundManager : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, StoveCounter.OnProgressChangedEventArg e)
    {
        if (e.canBeCooked == true)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}
