using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_AudioManagerWithLanguage : MonoBehaviour
{
    static A_AudioManagerWithLanguage instance = null;
    public static A_AudioManagerWithLanguage Instance { get => instance; set => instance = value; }

    public AudioSource stepsAudioSource;
    [SerializeField, Tooltip("List of audio clips English and Gujarati")]
    List<AudioClip> englishClips, gujaratiClips;

    [SerializeField, Tooltip("Other Kit")]
    List<AudioClip> englishOtherClips, gujaratiOtherClips;
    [SerializeField, Tooltip("Right Wrong")]
    List<AudioClip> englishRightWrongClips, gujaratiRightWrongClips;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        if (stepsAudioSource == null)
            stepsAudioSource = GetComponent<AudioSource>();
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
            if (A_ReadStepsAndVideoManager.instance.currentLanguage == _Language.English)
            {
                //  Debug.Log(" English call");
                //English audio Clips
                stepsAudioSource.PlayOneShot(englishClips[index]);
            }
            else
            {
                //  Debug.Log(" Gujarati call");
                //Gujarati audio clips
                stepsAudioSource.PlayOneShot(gujaratiClips[index]);
            }
        }
    }

    public void OtherAudio(int index)
    {
        if (stepsAudioSource.clip != null)
        {
            stepsAudioSource.Stop();
            if (A_ReadStepsAndVideoManager.instance.currentLanguage == _Language.English)
            {
                //  Debug.Log(" English call");
                //English audio Clips
                stepsAudioSource.PlayOneShot(englishOtherClips[index]);
            }
            else
            {
                //  Debug.Log(" Gujarati call");
                //Gujarati audio clips
                stepsAudioSource.PlayOneShot(gujaratiOtherClips[index]);
            }
        }
    }

    public void RightWrongAudio(int index)
    {
        if (stepsAudioSource.clip != null)
        {
            stepsAudioSource.Stop();
            if (A_ReadStepsAndVideoManager.instance.currentLanguage == _Language.English)
            {
                //  Debug.Log(" English call");
                //English audio Clips
                stepsAudioSource.PlayOneShot(englishRightWrongClips[index]);
            }
            else
            {
                //  Debug.Log(" Gujarati call");
                //Gujarati audio clips
                stepsAudioSource.PlayOneShot(gujaratiRightWrongClips[index]);
            }
        }
    }
}
