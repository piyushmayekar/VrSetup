using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiyushUtils
{
    public class CuttingArea : MonoBehaviour
    {
        [SerializeField] SoundPlayer soundPlayer;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.HACKSAW_BLADE_TAG))
                soundPlayer.PlayClip(soundPlayer.Clips[0], true);
            if (other.CompareTag(_Constants.FLATFILE_TAG))
                soundPlayer.PlayClip(soundPlayer.Clips[1], true);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.HACKSAW_BLADE_TAG))
                soundPlayer.StopPlayingAllSounds();
            if (other.CompareTag(_Constants.FLATFILE_TAG))
                soundPlayer.StopPlayingAllSounds();
        }
    }
}