using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that can be attached to any gameobject that may emit sound.
/// </summary>
public class SoundPlayer : MonoBehaviour
{
    [SerializeField, Tooltip("List of audio clips")]
    List<AudioClip> clips;
    [SerializeField, Tooltip("List of list of audio clips to play randomly one from")]
    List<ClipList> clipLists;

    [SerializeField] AudioSource audioSource;

    public List<ClipList> ClipLists { get => clipLists; set => clipLists = value; }
    public List<AudioClip> Clips { get => clips; set => clips = value; }
    public AudioSource AudioSource { get => audioSource; set => audioSource = value; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (AudioSource == null)
        {
            AudioSource = gameObject.AddComponent<AudioSource>();
            AudioSource.dopplerLevel = 1f;
        }
    }

    public void PlayClip(int clipIndex) => PlayClip(clips[clipIndex]);

    public void PlayClip(AudioClip clip, bool loop = false)
    {
        if (loop)
        {
            AudioSource.clip = clip;
            AudioSource.Play();
            AudioSource.loop = loop;
        }
        else
            AudioSource.PlayOneShot(clip);
    }

    public void PlayRandomClip(ClipList clipList, bool loop = false)
    => PlayClip(clipList.clips[Random.Range(0, clipList.clips.Count)], loop);

    public void StopPlayingAllSounds() => AudioSource.Stop();
}

[System.Serializable]
public struct ClipList
{
    public List<AudioClip> clips;
}
