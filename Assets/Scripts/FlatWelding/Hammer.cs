using UnityEngine;
using System;
using System.Collections.Generic;

namespace FlatWelding
{
    public class Hammer : MonoBehaviour
    {
        [SerializeField] SoundPlayer soundPlayer;

        internal void PlayHitSound()
        {
            soundPlayer.PlayRandomClip(soundPlayer.ClipLists[0], false);
        }
    }
}