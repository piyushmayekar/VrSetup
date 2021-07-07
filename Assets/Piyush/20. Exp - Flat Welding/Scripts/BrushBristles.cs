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
        [SerializeField] SoundPlayer soundPlayer;
        [SerializeField] Rigidbody parentBrushRb;
        void OnTriggerEnter(Collider other)
        {
            if (!other.name.Contains("Hand"))
                PlayBrushStrokeSound();
            if (other.CompareTag(_Constants.CLEANPOINT_TAG))
            {
                //Turning off the clean point collider once the cleaning is done
                dustPS.Play();
            }
            if (other.CompareTag(_Constants.SLAG_TAG))
            {
                other.GetComponent<Rigidbody>().velocity = parentBrushRb.velocity;
                Destroy(other.gameObject, 5f);
            }
        }

        public void PlayBrushStrokeSound()
        {
            if (!soundPlayer.AudioSource.isPlaying)
                soundPlayer.PlayRandomClip(soundPlayer.ClipLists[0]);
        }
    }
}