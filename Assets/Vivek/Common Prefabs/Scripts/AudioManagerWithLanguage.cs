using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerWithLanguage : MonoBehaviour
{
    static AudioManagerWithLanguage instance = null;
    public static AudioManagerWithLanguage Instance { get => instance; set => instance = value; }

    public AudioSource stepsAudioSource;
    [SerializeField, Tooltip("List of audio clips English and Gujarati")]
    List<AudioClip> englishClips, gujaratiClips;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        if (stepsAudioSource == null)
            stepsAudioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    /// <summary>
    ///Play audio clip english and gujarati with compare of ReadStepsAndVideoManager bool variable "isChangeFont".
    /// </summary>
    /// <param name="index">Pass the index of video clip in button click.</param>

    public void PlayStepAudio(int index)
    {
        if (stepsAudioSource.clip != null)
        {
            stepsAudioSource.Stop();
            if (ReadStepsAndVideoManager.instance.isChangeFont)
            {
                //English audio Clips
                stepsAudioSource.PlayOneShot(englishClips[index]);
            }
            else
            {
                //Gujarati audio clips
                stepsAudioSource.PlayOneShot(gujaratiClips[index]);
            }
        }
    }
}
