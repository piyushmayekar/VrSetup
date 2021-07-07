using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilingSide : MonoBehaviour
{
    [SerializeField] SoundPlayer soundPlayer;
    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains(_Constants.FILING_POINT_TAG))
            soundPlayer.PlayClip(soundPlayer.Clips[0], true);

    }

    /// <summary>
    /// OnCollisionExit is called when this collider/rigidbody has
    /// stopped touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionExit(Collision other)
    {
        soundPlayer.StopPlayingAllSounds();
    }
}
