using UnityEngine;
using System;
using System.Collections.Generic;

namespace FlatWelding
{
    /// <summary>
    /// Wear the apparatus task
    /// </summary>
    public class BrushBristles : MonoBehaviour
    {
        [SerializeField] ParticleSystem dustPS;
        [SerializeField] BrushTask task;
        [SerializeField] SoundPlayer soundPlayer;

        void OnTriggerEnter(Collider other)
        {
            if (!soundPlayer.AudioSource.isPlaying)
                soundPlayer.PlayRandomClip(soundPlayer.ClipLists[0]);
            if (other.CompareTag("CleanPoint"))
            {
                //Turning off the clean point collider once the cleaning is done
                other.enabled = false;
                dustPS.Play();
                task.EdgeBrushed();
            }
        }
    }
}