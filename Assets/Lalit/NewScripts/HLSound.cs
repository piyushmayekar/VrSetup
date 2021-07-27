using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HLSound : MonoBehaviour
{
    public static HLSound player;
    private AudioSource sound;

    private void Awake()
    {
        player = this;
    }

    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void PlayHighlightSnapSound()
    {
        sound.Play();
    }

}
