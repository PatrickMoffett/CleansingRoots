using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandomize : MonoBehaviour
{
    public AudioClip[] randOptions;
    void Start()
    {
        AudioSource mySource = GetComponent<AudioSource>();
        mySource.clip = randOptions[Random.Range(0, randOptions.Length)];
    }
}
