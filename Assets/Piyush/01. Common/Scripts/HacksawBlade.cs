using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HacksawBlade : MonoBehaviour
{
    [SerializeField] SoundPlayer soundPlayer;

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_Constants.CUTTINGPOINT_TAG) && !soundPlayer.AudioSource.isPlaying)
        {
            soundPlayer.PlayClip(soundPlayer.Clips[0]);
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_Constants.CUTTINGPOINT_TAG))
        {
            soundPlayer.StopPlayingAllSounds();
        }
    }
}
